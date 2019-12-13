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
using System.Runtime.InteropServices;
using Tizen.MachineLearning.Inference;

internal static partial class Interop
{
    internal static partial class Libraries
    {
        public const string Nnstreamer = "libcapi-nnstreamer.so.0";
    }

    internal static partial class Pipeline
    {
        /* typedef void (*ml_pipeline_state_cb) (ml_pipeline_state_e state, void *user_data); */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void StateChangeCallback(PipelineState state, IntPtr user_data);

        /* int ml_pipeline_construct (const char *pipeline_description, ml_pipeline_state_cb cb, void *user_data, ml_pipeline_h *pipe); */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_pipeline_construct", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError Construct(string pipeline_description, StateChangeCallback callback, IntPtr user_data, out IntPtr pipeline_handle);

        /* int ml_pipeline_destroy (ml_pipeline_h pipe); */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_pipeline_destroy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError Destroy(IntPtr pipeline_handle);

        /* int ml_pipeline_get_state (ml_pipeline_h pipe, ml_pipeline_state_e *state) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_pipeline_get_state", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetState(IntPtr pipeline_handle, out PipelineState state);

        /* int ml_pipeline_start (ml_pipeline_h pipe); */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_pipeline_start", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError Start(IntPtr pipeline_handle);

        /* int ml_pipeline_stop (ml_pipeline_h pipe); */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_pipeline_stop", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError Stop(IntPtr pipeline_handle);
    }

    internal static partial class SingleShot
    {
        /* int ml_single_open (ml_single_h *single, const char *model, const ml_tensors_info_h input_info, const ml_tensors_info_h output_info, ml_nnfw_type_e nnfw, ml_nnfw_hw_e hw) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_open", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError OpenSingle(out IntPtr single_handle, string model_path, IntPtr input_info, IntPtr output_info, NNFWType nn_type, HWType hw_type);

        /* int ml_single_close (ml_single_h single) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_close", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError CloseSingle(IntPtr single_handle);

        /* int ml_single_invoke (ml_single_h single, const ml_tensors_data_h input, ml_tensors_data_h *output) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_invoke", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError InvokeSingle(IntPtr single_handle, IntPtr input_data, out IntPtr output_data);

        /* int ml_single_invoke_dynamic (ml_single_h single, const ml_tensors_data_h input, const ml_tensors_info_h in_info, ml_tensors_data_h * output, ml_tensors_info_h * out_info) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_invoke_dynamic", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError InvokeSingleDynamic(IntPtr single_handle, IntPtr input_data, IntPtr input_info, out IntPtr output_data, out IntPtr output_info);

        /* int ml_single_get_input_info (ml_single_h single, ml_tensors_info_h *info) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_get_input_info", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetInputTensorsInfo(IntPtr single_handle, out IntPtr input_info);

        /* int ml_single_get_output_info (ml_single_h single, ml_tensors_info_h *info) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_get_output_info", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetOutputTensorsInfo(IntPtr single_handle, out IntPtr output_info);

        /* int ml_single_set_input_info (ml_single_h single, const ml_tensors_info_h info) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_set_input_info", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetInputInfo(IntPtr single_handle, IntPtr in_handle);

        /* int ml_single_set_timeout (ml_single_h single, unsigned int timeout)*/
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_single_set_timeout", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetTimeout(IntPtr single_handle, int time_ms);
    }

    internal static partial class Util
    {
        /* int ml_tensors_info_create (ml_tensors_info_h *info) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError CreateTensorsInfo(out IntPtr info);
            
        /* int ml_tensors_info_destroy (ml_tensors_info_h info) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_destroy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError DestroyTensorsInfo(IntPtr info);

        /* int ml_tensors_info_set_count (ml_tensors_info_h info, unsigned int count) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_set_count", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetTensorsCount(IntPtr info, int count);

        /* int ml_tensors_info_get_count (ml_tensors_info_h info, unsigned int *count) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_get_count", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetTensorsCount(IntPtr info, out int count);

        /* int ml_tensors_info_set_tensor_name (ml_tensors_info_h info, unsigned int index, const char *name) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_set_tensor_name", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetTensorName(IntPtr info, int index, string name);

        /* int ml_tensors_info_get_tensor_name (ml_tensors_info_h info, unsigned int index, char **name) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_get_tensor_name", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetTensorName(IntPtr info, int index, out string name);

        /* int ml_tensors_info_set_tensor_type (ml_tensors_info_h info, unsigned int index, const ml_tensor_type_e type) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_set_tensor_type", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetTensorType(IntPtr info, int index, TensorType type);

        /* int ml_tensors_info_get_tensor_type (ml_tensors_info_h info, unsigned int index, ml_tensor_type_e *type) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_get_tensor_type", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetTensorType(IntPtr info, int index, out TensorType type);

        /* int ml_tensors_info_set_tensor_dimension (ml_tensors_info_h info, unsigned int index, const ml_tensor_dimension dimension) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_set_tensor_dimension", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetTensorDimension(IntPtr info, int index, int[] dimension);

        /* int ml_tensors_info_get_tensor_dimension (ml_tensors_info_h info, unsigned int index, ml_tensor_dimension dimension) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_info_get_tensor_dimension", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetTensorDimension(IntPtr info, int index, [In, Out] uint[] dimension);

        /* int ml_tensors_data_create (const ml_tensors_info_h info, ml_tensors_data_h *data) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_data_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError CreateTensorsData(IntPtr info, out IntPtr data);

        /* int ml_tensors_data_destroy (ml_tensors_data_h data) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_data_destroy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError DestroyTensorsData(IntPtr data);

        /* int ml_tensors_data_get_tensor_data (ml_tensors_data_h data, unsigned int index, void **raw_data, size_t *data_size) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_data_get_tensor_data", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError GetTensorData(IntPtr data, int index, out IntPtr raw_data, out int data_size);

        /* int ml_tensors_data_set_tensor_data (ml_tensors_data_h data, unsigned int index, const void *raw_data, const size_t data_size) */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_tensors_data_set_tensor_data", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError SetTensorData(IntPtr data, int index, byte[] raw_data, int data_size);

        /* int ml_check_nnfw_availability (ml_nnfw_type_e nnfw, ml_nnfw_hw_e hw, bool *available); */
        [DllImport(Libraries.Nnstreamer, EntryPoint = "ml_check_nnfw_availability", CallingConvention = CallingConvention.Cdecl)]
        internal static extern NNStreamerError CheckNNFWAvailability(NNFWType nnfw, HWType hw, out bool available);

        internal static byte[] IntPtrToByteArray(IntPtr unmanagedByteArray, int size)
        {
            byte[] retByte = new byte[size];
            Marshal.Copy(unmanagedByteArray, retByte, 0, size);
            return retByte;
        }
    }
}
