﻿/*
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
    /// <summary>
    /// An extended EventArgs class that contains the received new TensorsData
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    public class NewDataEventArgs : EventArgs
    {
        internal NewDataEventArgs(TensorsData data)
        {
            Data = data;
        }

        /// <summary>
        /// The received new TensorsData object
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public TensorsData Data { get; }
    }
}