/*
 * Copyright(c) 2019 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System.ComponentModel;

namespace Tizen.NUI.Binding
{

    /// <summary>
    /// GraphicsTypeManager class to manage various types.
    /// </summary>
    /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
    /// <since_tizen> 5 </since_tizen>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GraphicsTypeManager
    {
        internal void RegisterCustomConverterForDynamicResourceBinding(global::System.Type type, Tizen.NUI.Binding.TypeConverter userConverter)
        {
            if (Tizen.NUI.Binding.BindableProperty.UserCustomConvertTypes.ContainsKey(type) == false)
            {
                Tizen.NUI.Binding.BindableProperty.UserCustomConvertTypes.Add(type, userConverter);
            }
        }
    }

}