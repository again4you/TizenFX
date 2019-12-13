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

namespace Tizen.MachineLearning.Inference
{
    public class Pipeline : IDisposable
    {
        private IntPtr _handle = IntPtr.Zero;
        private bool _disposed = false;

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
            if (_disposed)
                return;

            if (disposing)
            {
                // release managed object
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
    }
}
