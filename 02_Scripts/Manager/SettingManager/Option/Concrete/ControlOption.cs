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

namespace ProjectL
{
    [Serializable]
    public class ControlData : ISaveAndLoadToJson<ControlData>, IEquatable<ControlData>, ICloneable
    {
        public ControlData()
        {
            keyboardSensitivity = 50f;
            mouseSensitivity = 50f;
            zoomSensitivity = 50f;
        }
        
        public float keyboardSensitivity;
        public float mouseSensitivity;
        public float zoomSensitivity;

        public bool Equals(ControlData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return keyboardSensitivity.Equals(other.keyboardSensitivity) &&
                   mouseSensitivity.Equals(other.mouseSensitivity) && zoomSensitivity.Equals(other.zoomSensitivity);
        }

        public object Clone()
        {
            return new ControlData
                   {
                       keyboardSensitivity = this.keyboardSensitivity,
                       mouseSensitivity = this.mouseSensitivity,
                       zoomSensitivity = this.zoomSensitivity
                   };
        }
    }

    public class ControlOption : Option<ControlData>
    {
        public ControlOption(bool isApplyImmediately) : base(isApplyImmediately) { }
        
        protected override void _Reset()
        {
        }
    }
}