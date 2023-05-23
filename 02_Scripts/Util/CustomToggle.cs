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

using UnityEngine.UI;

namespace ProjectL
{
    public class CustomToggle : Toggle
    {
        private DefaultSetToggleGroup defaultSetToggleGroup;
        
        protected override void Awake()
        {
            base.Awake();
            
            var defaultGroup = (group as DefaultSetToggleGroup);
            
            defaultSetToggleGroup = defaultGroup;
        }

        protected override void Start()
        {
            base.Start();
            
            this.onValueChanged.AddListener(SetDefaultToggleInToggleGroup);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            this.onValueChanged.RemoveListener(SetDefaultToggleInToggleGroup);
        }
        
        protected override void OnDisable() { }

        private void SetDefaultToggleInToggleGroup(bool value)
        {
            if (value == false)
            {
                return;
            }
            
            if (defaultSetToggleGroup != null)
            {
                if (defaultSetToggleGroup.isNotSavedToggleState == false)
                {
                    defaultSetToggleGroup.defaultToggle = this;
                }
            }
        }
    }
}