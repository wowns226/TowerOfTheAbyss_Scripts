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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public enum InputType
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
        MoveSpeedUp,
        MoveReset,

        Profile,
        Technology,
        CustomUnit,
        Relic,
        Upgrade,
        Building,
        System,
        Exit,

        Focus1,
        Focus2,
        Focus3,
        Focus4,
        Focus5,

        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Skill5,

        ScreenShot,
    }
    
    [Serializable]
    public class KeyBindingDatas : ISaveAndLoadToJson<KeyBindingDatas>, IEquatable<KeyBindingDatas>, ICloneable
    {
        public KeyBindingDatas()
        {
            bindingDatas = new Dictionary<InputType, KeyCode>();
        }
        
        public Dictionary<InputType, KeyCode> bindingDatas;

        public bool Equals(KeyBindingDatas other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return bindingDatas.OrderBy(x => x.Key).SequenceEqual(other.bindingDatas.OrderBy(y => y.Key));
        }

        public object Clone()
        {
            return new KeyBindingDatas
                   {
                       bindingDatas = new Dictionary<InputType, KeyCode>(this.bindingDatas)
                   };
        }
    }

    public class KeyBindingOption : Option<KeyBindingDatas>
    {
        public KeyBindingOption(bool isApplyImmediately) : base(isApplyImmediately) { }
        
        protected override void _Reset()
        {
            Bind(InputType.MoveForward, KeyCode.W);
            Bind(InputType.MoveBackward, KeyCode.S);
            Bind(InputType.MoveLeft, KeyCode.A);
            Bind(InputType.MoveRight, KeyCode.D);
            Bind(InputType.MoveSpeedUp, KeyCode.LeftShift);
            Bind(InputType.MoveReset, KeyCode.Space);

            Bind(InputType.Profile, KeyCode.P);
            Bind(InputType.Technology, KeyCode.T);
            Bind(InputType.CustomUnit, KeyCode.C);
            Bind(InputType.Relic, KeyCode.R);
            Bind(InputType.Upgrade, KeyCode.U);
            Bind(InputType.Building, KeyCode.B);
            Bind(InputType.System, KeyCode.O);
            Bind(InputType.Exit, KeyCode.Escape);

            Bind(InputType.Focus1, KeyCode.F1);
            Bind(InputType.Focus2, KeyCode.F2);
            Bind(InputType.Focus3, KeyCode.F3);
            Bind(InputType.Focus4, KeyCode.F4);
            Bind(InputType.Focus5, KeyCode.F5);

            Bind(InputType.Skill1, KeyCode.Alpha1);
            Bind(InputType.Skill2, KeyCode.Alpha2);
            Bind(InputType.Skill3, KeyCode.Alpha3);
            Bind(InputType.Skill4, KeyCode.Alpha4);
            Bind(InputType.Skill5, KeyCode.Alpha5);

            Bind(InputType.ScreenShot, KeyCode.F12);
        }

        public void Bind(InputType inputType, KeyCode keyCode)
        {
            var dataClone = NewDataClone;
            var bindingDatasCopy = dataClone.bindingDatas;
            
            foreach (var key in bindingDatasCopy.Keys.ToList())
            {
                if (bindingDatasCopy[key].Equals(keyCode))
                {
                    bindingDatasCopy[key] = KeyCode.None;
                }
            }

            bindingDatasCopy.TryAdd(inputType, KeyCode.None);
            bindingDatasCopy[inputType] = keyCode;
            
            NewData = dataClone;
        }

        public KeyCode GetKey(InputType inputType) => CurrentData.bindingDatas[inputType];
    }
}