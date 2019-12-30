/*
* Copyright (c) 2019 Samsung Electronics Co., Ltd All Rights Reserved
*
* Licensed under the Apache License, Version 2.0 (the License);
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an AS IS BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace Tizen.MachineLearning.Inference
{
    /// <summary>
    /// The Pipeline class provides interfaces to create and execute stream pipelines with neural networks.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    public class Pipeline : IDisposable
    {
        private IntPtr _handle = IntPtr.Zero;
        private bool _disposed = false;
        private IDictionary<string, SinkCallbackEvent> _sinkEventList;
        private IDictionary<string, NodeHandleInfo> _nodeHandleList;

        private EventHandler<StateChangedEventArgs> _stateChanged;
        private Interop.Pipeline.StateChangedCallback _stateChangedCallback;

        /// <summary>
        /// Creates a new Pipeline instance with the given pipeline description
        /// </summary>
        /// <param name="description">The pipeline description. Refer to GStreamer manual or NNStreamer documentation for examples and the grammar.</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="InvalidDataException">Thrown when the wrong pipeline description or initialization failure.</exception>
        /// <since_tizen> 8 </since_tizen>
        public Pipeline(string description)
        {
            NNStreamer.CheckNNStreamerSupport();

            if (description == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Pipeline description is null");

            CreatePipeplie(description, null);
        }

        /// <summary>
        /// Creates a new Pipeline instance with the given pipeline description and State Changed callback method
        /// </summary>
        /// <param name="description">The pipeline description. Refer to GStreamer manual or NNStreamer documentation for examples and the grammar.</param>
        /// <param name="stateChanged">The Event Handler to be called when the pipeline state is changed.</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="InvalidDataException">Thrown when the wrong pipeline description or initialization failure.</exception>
        /// <since_tizen> 8 </since_tizen>
        public Pipeline(string description, EventHandler<StateChangedEventArgs> stateChanged)
        {
            NNStreamer.CheckNNStreamerSupport();

            if (description == null || stateChanged == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Parameter is null");

            CreatePipeplie(description, stateChanged);
        }

        private void CreatePipeplie(string description, EventHandler<StateChangedEventArgs> stateChanged)
        {
            NNStreamerError ret = NNStreamerError.None;
            if (stateChanged == null) {
                ret = Interop.Pipeline.Construct(description, null, IntPtr.Zero, out _handle);
            }
            else {
                _stateChangedCallback = (state, _) =>
                {
                    _stateChanged?.Invoke(this, new StateChangedEventArgs(state));
                };

                ret = Interop.Pipeline.Construct(description, _stateChangedCallback, IntPtr.Zero, out _handle);
                _stateChanged += stateChanged;
            }

            if (ret == NNStreamerError.StreamsPipe)
                throw new InvalidDataException("fail to create Pipeline instance because of wrong parameter or initialization failure.");

            /* Init Sink & Node list */
            _sinkEventList = new Dictionary<string, SinkCallbackEvent>();
            _nodeHandleList = new Dictionary<string, NodeHandleInfo>();
        }

        /// <summary>
        /// Destructor of the Pipeline instance.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        ~Pipeline()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the state of pipeline.
        /// </summary>
        /// <returns>PipelineState enumeration of Pipeline instance</returns>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="SystemException">Thrown when failed to get state from the pipeline because of internal engine.</exception>
        /// <since_tizen> 8 </since_tizen>
        public PipelineState GetState()
        {
            NNStreamer.CheckNNStreamerSupport();

            PipelineState retState = PipelineState.Unknown;
            NNStreamerError ret = NNStreamerError.None;

            ret = Interop.Pipeline.GetState(_handle, out retState);
            NNStreamer.CheckException(ret, "fail to get Pipeline State because of internal system error");

            return retState;
        }

        /// <summary>
        /// Starts the pipeline, asynchronously. (The state would be changed to PipelineState.Playing)
        /// </summary>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="SystemException">Thrown when failed to get state from the pipeline because of internal engine.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void Start()
        {
            NNStreamer.CheckNNStreamerSupport();

            NNStreamerError ret = NNStreamerError.None;

            ret = Interop.Pipeline.Start(_handle);
            NNStreamer.CheckException(ret, "fail to start Pipeline State because of internal system error");
        }

        /// <summary>
        /// Stops the pipeline, asynchronously. (The state would be changed to PipelineState.Paused)
        /// </summary>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="SystemException">Thrown when failed to get state from the pipeline because of internal engine.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void Stop()
        {
            NNStreamer.CheckNNStreamerSupport();

            NNStreamerError ret = NNStreamerError.None;

            ret = Interop.Pipeline.Stop(_handle);
            NNStreamer.CheckException(ret, "fail to stop Pipeline State because of internal system error");
        }

        /// <summary>
        /// Releases any unmanaged resources used by this object.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases any unmanaged resources used by this object including opened handle
        /// </summary>
        /// <param name="disposing">If true, disposes any disposable objects. If false, does not dispose disposable objects.</param>
        /// <since_tizen> 8 </since_tizen>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // release managed object
            }

            /* Close all opened handle and remove all*/
            if (_nodeHandleList != null)
            {
                foreach (NodeHandleInfo hInfo in _nodeHandleList.Values)
                {
                    switch (hInfo.Type)
                    {
                        case HandleType.Source:
                            Interop.Pipeline.ReleaseSrcHandle(hInfo.Handle);
                            break;

                        case HandleType.Valve:
                            Interop.Pipeline.ReleaseValveHandle(hInfo.Handle);
                            break;
                    }
                }
                _nodeHandleList.Clear();
            }

            // release unmanaged objects
            if (_handle != IntPtr.Zero)
            {
                NNStreamerError ret = NNStreamerError.None;
                ret = Interop.Pipeline.Destroy(_handle);
                if (ret != NNStreamerError.None)
                {
                    Log.Error(NNStreamer.TAG, "failed to close Pipeline instance");
                }
                _handle = IntPtr.Zero;
            }

            _disposed = true;
        }

        /// <summary>
        /// Registers new data event handler to sink node. 
        /// </summary>
        /// <param name="sinkNodeName">The name of sink node</param>
        /// <param name="newDataCreated">Event Handler for new data</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="SystemException">Thrown when failed to register sink callback because of internal engine.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void RegisterSinkCallback(string sinkNodeName, EventHandler<NewDataEventArgs> newDataCreated)
        {
            NNStreamer.CheckNNStreamerSupport();

            /* Check the argument */
            if (string.IsNullOrEmpty(sinkNodeName))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            if (newDataCreated == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Event Handler is invalid");

            SinkCallbackEvent sce;
            if (_sinkEventList.ContainsKey(sinkNodeName) == true)
            {
                sce = _sinkEventList[sinkNodeName];
            }
            else
            {
                sce = new SinkCallbackEvent(sinkNodeName, _handle);
                _sinkEventList.Add(sinkNodeName, sce);
            }
            sce.NewDataCreated += newDataCreated;
        }

        /// <summary>
        /// Unregisters new data event handler from sink node.
        /// </summary>
        /// <param name="sinkNodeName">The name of sink node</param>
        /// <param name="newDataCreated">Event Handler to be unregistered</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="SystemException">Thrown when failed to unregister sink callback because of internal engine.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void UnregisterSinkCallback(string sinkNodeName, EventHandler<NewDataEventArgs> newDataCreated)
        {
            NNStreamer.CheckNNStreamerSupport();

            /* Check the argument */
            if (string.IsNullOrEmpty(sinkNodeName) || (_sinkEventList.ContainsKey(sinkNodeName) != true))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            if (newDataCreated == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Event Handler is invalid");

            SinkCallbackEvent sce = _sinkEventList[sinkNodeName];
            sce.NewDataCreated -= newDataCreated;
        }

        /// <summary>
        /// Adds an input data frame to source node.
        /// </summary>
        /// <param name="srcNodeName">The name of source node</param>
        /// <param name="data">TensorsData to be inputed</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <exception cref="SystemException">Thrown when failed to input TensorsData because of internal engine.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void InputData(string srcNodeName, TensorsData data)
        {
            NNStreamer.CheckNNStreamerSupport();

            if (string.IsNullOrEmpty(srcNodeName))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            if (data == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "TensorsData is invalid");

            NNStreamerError ret = NNStreamerError.None;

            /* Get src node handle */
            NodeHandleInfo hInfo;
            if (_nodeHandleList.ContainsKey(srcNodeName))
            {
                hInfo = _nodeHandleList[srcNodeName];
            }
            else
            {
                IntPtr srcHandle = IntPtr.Zero;
                ret = Interop.Pipeline.GetSrcHandle(_handle, srcNodeName, out srcHandle);
                NNStreamer.CheckException(ret, "Failed to get Source Node handle");

                hInfo = new NodeHandleInfo(srcHandle, HandleType.Source);
                _nodeHandleList.Add(srcNodeName, hInfo);
            }

            /* Prepare TensorsData */
            data.PrepareInvoke();

            /* Input data */
            ret = Interop.Pipeline.InputSrcData(hInfo.Handle, data.GetHandle(), PipelineBufferPolicy.NotFreed);
            NNStreamer.CheckException(ret, "Failed to input tensors data to source node: " + srcNodeName);
        }

        /// <summary>
        /// Controls the valve. Set the flag true to open, false to close.
        /// </summary>
        /// <param name="valveNodeName">The name of valve node</param>
        /// <param name="open">The flag to control the flow (True for open)</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void ControlValve(string valveNodeName, bool open)
        {
            NNStreamer.CheckNNStreamerSupport();

            /* Check the parameter */
            if (string.IsNullOrEmpty(valveNodeName))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            /* Get valve node handle */
            NNStreamerError ret = NNStreamerError.None;
            NodeHandleInfo hInfo;
            if (_nodeHandleList.ContainsKey(valveNodeName))
            {
                hInfo = _nodeHandleList[valveNodeName];
            }
            else
            {
                IntPtr valveHandle = IntPtr.Zero;
                ret = Interop.Pipeline.GetValveHandle(_handle, valveNodeName, out valveHandle);
                NNStreamer.CheckException(ret, "Failed to get Valve Node handle: " + valveNodeName);

                hInfo = new NodeHandleInfo(valveHandle, HandleType.Valve);
                _nodeHandleList.Add(valveNodeName, hInfo);
            }

            /* Set valvue status */
            ret = Interop.Pipeline.OpenValve(hInfo.Handle, open);
            NNStreamer.CheckException(ret, "Failed to set vavle status: " + valveNodeName);
        }

        /// <summary>
        /// Controls the switch to select input/output nodes (pads).
        /// </summary>
        /// <param name="switchNodeName">The name of switch node.</param>
        /// <param name="padName">The name of the chosen pad to be activated.</param>
        /// <feature>http://tizen.org/feature/machine_learning.inference</feature>
        /// <exception cref="NotSupportedException">Thrown when the feature is not supported.</exception>
        /// <exception cref="ArgumentException">Thrown when the method failed due to an invalid parameter.</exception>
        /// <since_tizen> 8 </since_tizen>
        public void SelectSwitchPad(string switchNodeName, string padName)
        {
            NNStreamer.CheckNNStreamerSupport();

            /* Check the parameter */
            if (string.IsNullOrEmpty(switchNodeName))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            if (string.IsNullOrEmpty(padName))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Pad Name is invalid");

            /* Get switch node handle */
            NNStreamerError ret = NNStreamerError.None;
            NodeHandleInfo hInfo;
            if (_nodeHandleList.ContainsKey(switchNodeName))
            {
                hInfo = _nodeHandleList[switchNodeName];
            }
            else
            {
                IntPtr switchHandle = IntPtr.Zero;
                SwitchType switchType;
                ret = Interop.Pipeline.GetSwitchHandle(_handle, switchNodeName, out switchType, out switchHandle);
                NNStreamer.CheckException(ret, "Failed to get Switch Node handle: " + switchNodeName);

                if (switchType == SwitchType.OutputSelector)
                    hInfo = new NodeHandleInfo(switchHandle, HandleType.SwitchOut);
                else
                    hInfo = new NodeHandleInfo(switchHandle, HandleType.SwitchIn);

                _nodeHandleList.Add(switchNodeName, hInfo);
            }

            /* Select switch pad */
            ret = Interop.Pipeline.SelectSwitchPad(hInfo.Handle, padName);
            NNStreamer.CheckException(ret, "Failed to select pad: " + padName);
        }

        private class NodeHandleInfo
        {
            public IntPtr Handle { get; }
            public HandleType Type { get; }

            public NodeHandleInfo(IntPtr handle, HandleType type)
            {
                Handle = handle;
                Type = type;
            }
        }

        private enum HandleType
        {
            Source = 0,
            Sink = 1,
            Valve = 2,
            SwitchIn = 3,
            SwitchOut = 4,

            Unknown,
        };

        private class SinkCallbackEvent
        {
            private EventHandler<NewDataEventArgs> _newDataCreated;
            private Interop.Pipeline.NewDataCallback _newDataCreatedCallback;
            private IntPtr _pipelineHandle = IntPtr.Zero;
            private IntPtr _callbackHandle = IntPtr.Zero;

            private readonly object _eventLock = new object();

            public SinkCallbackEvent(string sinkNodeName, IntPtr pipelineHandle)
            {
                Name = sinkNodeName;
                _pipelineHandle = pipelineHandle;

                _newDataCreatedCallback = (data_handle, Info_handle, _) =>
                {
                    TensorsData data = TensorsData.CreateFromNativeHandle(data_handle, Info_handle, true, true);
                    _newDataCreated?.Invoke(this, new NewDataEventArgs(data));
                };
            }

            public event EventHandler<NewDataEventArgs> NewDataCreated
            {
                add
                {
                    if (value == null)
                        return;

                    lock (_eventLock)
                    {
                        if (_newDataCreated == null)
                        {
                            NNStreamerError ret = NNStreamerError.None;
                            ret = Interop.Pipeline.RegisterSinkCallback(_pipelineHandle, Name, _newDataCreatedCallback, IntPtr.Zero, out _callbackHandle);
                            NNStreamer.CheckException(ret, "fail to register NewDataCreate Event Handler");
                        }
                        _newDataCreated += value;
                    }
                }
                remove
                {
                    if (value == null)
                        return;

                    lock (_eventLock)
                    {
                        if (_newDataCreated == value)
                        {
                            NNStreamerError ret = NNStreamerError.None;
                            ret = Interop.Pipeline.UnregisterSinkCallback(_callbackHandle);
                            NNStreamer.CheckException(ret, "fail to unregister NewDataCreate Event Handler");
                        }
                        _newDataCreated -= value;
                    }
                }
            }

            public string Name { get; }
        }
    }
}
