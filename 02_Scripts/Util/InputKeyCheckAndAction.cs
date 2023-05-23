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

using System;
using UnityEngine;

namespace ProjectL
{
    public class InputKeyCheckAndAction : MonoBehaviour
    {
        CustomAction<KeyCode> action = new CustomAction<KeyCode>();

        private void Update()
        {
            if (Input.anyKeyDown) 
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) 
                {
                    if (Input.GetKeyDown(keyCode)) 
                    {
                        // 마우스 클릭은 방지
                        if (keyCode == KeyCode.Mouse0
                            || keyCode == KeyCode.Mouse1
                            || keyCode == KeyCode.Mouse2
                            || keyCode == KeyCode.Mouse3
                            || keyCode == KeyCode.Mouse4
                            || keyCode == KeyCode.Mouse5
                            || keyCode == KeyCode.Mouse6)
                        {
                            return;
                        }
                        
                        action.Invoke(keyCode);
                        action.Clear();
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
        
        public void RegisterAction(Action<KeyCode> action)
        {
            this.action.Add(action);
            this.gameObject.SetActive(true);
        }

    }
}