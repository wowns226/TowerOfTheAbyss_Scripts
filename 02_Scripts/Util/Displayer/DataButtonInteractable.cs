// Copyright (C) <2023>  
//     Authors : Shin YongUk <dyddyddnr@naver.com>
//     Lim jaejun <wowns226@naver.com>
//  
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//  
//     You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>

using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class DataButtonInteractable : DataObserver
    {
        [SerializeField]
        private string targetData;

        [SerializeField]
        private bool defaultState;

        [SerializeField]
        private Button button;

        public override void UpdateData(object dataValue)
        {
            if (ReferenceEquals(dataValue, null))
                Toggle(defaultState);

            if (targetData.ToLower().Equals(dataValue.ToString().ToLower()))
                Toggle(true);
            else
                Toggle(false);
        }

        private void Toggle(bool isOn)
        {
            if (button)
                ToggleButtonInteractable(isOn);
        }

        private void ToggleButtonInteractable(bool isOn)
        {
            button.interactable = isOn;
        }
    }
}