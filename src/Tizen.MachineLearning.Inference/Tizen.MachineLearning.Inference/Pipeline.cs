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
using System.Collections.Generic;

namespace Tizen.MachineLearning.Inference
{
    public class Pipeline : IDisposable
    {
        private IntPtr _handle = IntPtr.Zero;
        private bool _disposed = false;
        private IDictionary<string, SinkCallbackEvent> _sinkEventList;
        private IDictionary<string, NodeHandleInfo> _nodeHandleList;

        private EventHandler<StateChangeEventArgs> _stateChanged;
        private Interop.Pipeline.StateChangeCallback _stateChangeCallback;

        public Pipeline(string description)
        {
            NNStreamer.CheckNNStreamerSupport();

            if (description == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Pipeline description is null");

            NNStreamerError ret = NNStreamerError.None;
            ret = Interop.Pipeline.Construct(description, null, IntPtr.Zero, out _handle);
            NNStreamer.CheckException(ret, "fail to create Pipeline instance");

            _sinkEventList = new Dictionary<string, SinkCallbackEvent>();
            _nodeHandleList = new Dictionary<string, NodeHandleInfo>();
        }

        public Pipeline(string description, EventHandler<StateChangeEventArgs> stateChanged)
        {
            NNStreamer.CheckNNStreamerSupport();

            if (description == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Parameter is null");

            _stateChangeCallback = (state, _) =>
            {
                _stateChanged?.Invoke(this, new StateChangeEventArgs(state));
            };

            NNStreamerError ret = NNStreamerError.None;
            ret = Interop.Pipeline.Construct(description, _stateChangeCallback, IntPtr.Zero, out _handle);
            NNStreamer.CheckException(ret, "fail to create Pipeline instance");

            /* Need to check */
            _stateChanged += stateChanged;

            _sinkEventList = new Dictionary<string, SinkCallbackEvent>();
            _nodeHandleList = new Dictionary<string, NodeHandleInfo>();
        }

        ~Pipeline()
        {

            Dispose(false);
        }

        public PipelineState GetState()
        {
            NNStreamer.CheckNNStreamerSupport();

            PipelineState retState = PipelineState.Unknown;
            NNStreamerError ret = NNStreamerError.None;

            ret = Interop.Pipeline.GetState(_handle, out retState);
            NNStreamer.CheckException(ret, "fail to get Pipeline State");

            return retState;
        }

        public void Start()
        {
            NNStreamer.CheckNNStreamerSupport();

            NNStreamerError ret = NNStreamerError.None;

            ret = Interop.Pipeline.Start(_handle);
            NNStreamer.CheckException(ret, "fail to start Pipeline");
        }

        public void Stop()
        {
            NNStreamer.CheckNNStreamerSupport();

            NNStreamerError ret = NNStreamerError.None;

            ret = Interop.Pipeline.Stop(_handle);
            NNStreamer.CheckException(ret, "fail to stop Pipeline");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Log.Error(NNStreamer.TAG, "Pipeline - Dispose - 1");

            if (_disposed)
                return;

            if (disposing)
            {
                // release managed object
            }

            /* Close all opened handle and remove all*/
            foreach (NodeHandleInfo hInfo in _nodeHandleList.Values)
            {
                switch (hInfo.Type)
                {
                    case HandleType.Source:
                        Log.Error(NNStreamer.TAG, "Pipeline - Dispose - 2: " + hInfo.Handle.ToString());
                        Interop.Pipeline.ReleaseSrcHandle(hInfo.Handle);
                        break;
                }
            }
            _nodeHandleList.Clear();
            Log.Error(NNStreamer.TAG, "Pipeline - Dispose - 3");

            // release unmanaged objects
            if (_handle != IntPtr.Zero)
            {
                Log.Error(NNStreamer.TAG, "Pipeline - Dispose - 4");
                NNStreamerError ret = NNStreamerError.None;
                ret = Interop.Pipeline.Destroy(_handle);
                if (ret != NNStreamerError.None)
                {
                    Log.Error(NNStreamer.TAG, "failed to close Pipeline instance");
                }
                _handle = IntPtr.Zero;

                Log.Error(NNStreamer.TAG, "Pipeline - Dispose - 5");
            }

            _disposed = true;
        }

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

        public void UnregisterSinkCallback(string sinkNodeName, EventHandler<NewDataEventArgs> newDataCreated)
        {
            NNStreamer.CheckNNStreamerSupport();

            /* Check the argument */
            if (string.IsNullOrEmpty(sinkNodeName) || (_sinkEventList.ContainsKey(sinkNodeName) != true))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            if (newDataCreated == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Event Handler is invalid");

            /* todo: Exception occurs */
            /*
            if (_sinkEventList.ContainsKey(sinkNodeName) != true)
                return;
            */

            SinkCallbackEvent sce = _sinkEventList[sinkNodeName];
            sce.NewDataCreated -= newDataCreated;
        }

        public void InputData(string srcNodeName, TensorsData data)
        {
            NNStreamer.CheckNNStreamerSupport();

            if (string.IsNullOrEmpty(srcNodeName))
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "Node Name is invalid");

            if (data == null)
                throw NNStreamerExceptionFactory.CreateException(NNStreamerError.InvalidParameter, "TensorsData is invalid");

            Log.Error(NNStreamer.TAG, "Pipeline - InputData - 1");

            NNStreamerError ret = NNStreamerError.None;

            Log.Error(NNStreamer.TAG, "Pipeline - InputData - 2");

            /* Get src node handle */
            NodeHandleInfo hInfo;
            if (_nodeHandleList.ContainsKey(srcNodeName))
            {
                Log.Error(NNStreamer.TAG, "Pipeline - InputData - 3");

                hInfo = _nodeHandleList[srcNodeName];

                Log.Error(NNStreamer.TAG, "Pipeline - InputData - 4");
            }
            else
            {
                Log.Error(NNStreamer.TAG, "Pipeline - InputData - 5");

                IntPtr srcHandle = IntPtr.Zero;
                ret = Interop.Pipeline.GetSrcHandle(_handle, srcNodeName, out srcHandle);
                NNStreamer.CheckException(ret, "Failed to get Source Node handle");

                Log.Error(NNStreamer.TAG, "Pipeline - InputData - 6: " + srcHandle.ToString());

                hInfo = new NodeHandleInfo(srcHandle, HandleType.Source);
                _nodeHandleList.Add(srcNodeName, hInfo);

                Log.Error(NNStreamer.TAG, "Pipeline - InputData - 7");
            }

            /* Prepare TensorsData */
            data.PrepareInvoke();

            Log.Error(NNStreamer.TAG, "Pipeline - InputData - 8: " + hInfo.Handle);

            /* Input data */
            ret = Interop.Pipeline.InputSrcData(hInfo.Handle, data.GetHandle(), PipelineBufferPolicy.NotFreed);
            NNStreamer.CheckException(ret, "Failed to input tensors data to source node: " + srcNodeName);

            Log.Error(NNStreamer.TAG, "Pipeline - InputData - 9");
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
                    TensorsData data = TensorsData.CreateFromNativeHandle(data_handle, Info_handle, true);
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
