/*
 * Copyright(c) 2017 Samsung Electronics Co., Ltd.
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

namespace Tizen.NUI.Xaml.UIComponents
{

    /// <summary>
    /// The CheckBoxButton provides a checkbox button which the user can check or uncheck.<br />
    /// By default, the CheckBoxButton emits a Button.Clicked event when the button changes its state to selected or unselected.<br />
    /// The button's appearance could be modified by Button.UnselectedImage, Button.BackgroundImage, Button.SelectedImage, Button.SelectedBackgroundImage, Button.DisabledBackgroundImage, Button.DisabledImage, and Button.DisabledSelectedImage.<br />
    /// When the button is not disabled, if it's not selected, it only shows the background image.<br />
    /// The selected image is shown over the background image when the box is selected (background image is not replaced by \e selected image).<br />
    /// When the button is disabled, the background image and the selected image are replaced by disabled images.<br />
    /// /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public class CheckBoxButton : Button
    {
        private Tizen.NUI.UIComponents.CheckBoxButton _checkBoxButton;
        internal Tizen.NUI.UIComponents.CheckBoxButton checkBoxButton
        {
            get
            {
                if (null == _checkBoxButton)
                {
                    _checkBoxButton = handleInstance as Tizen.NUI.UIComponents.CheckBoxButton;
                }

                return _checkBoxButton;
            }
        }

        /// <summary>
        /// Creates an initialized CheckBoxButton.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public CheckBoxButton() : this(new Tizen.NUI.UIComponents.CheckBoxButton())
        {
        }

        internal CheckBoxButton(Tizen.NUI.UIComponents.CheckBoxButton nuiInstance) : base(nuiInstance)
        {
            SetNUIInstance(nuiInstance);
        }
    }
}