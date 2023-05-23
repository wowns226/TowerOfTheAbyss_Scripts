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
    public enum ResolutionType
    {
        R1920x1080 = 0,
        R1600x900 = 1,
        R1440x900 = 2,
        R1280x1024 = 3,
        R1280x960 = 4,
        R1280x720 = 5,
        R1024x768 = 6,
    }

    [Serializable]
    public class ResolutionData : ISaveAndLoadToJson<ResolutionData>, IEquatable<ResolutionData>, ICloneable
    {
        public ResolutionData()
        {
            resolutionType = ResolutionType.R1920x1080;
            isFullScreen = true;
        }
        
        public ResolutionType resolutionType;
        
        public bool isFullScreen;

        public bool Equals(ResolutionData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return resolutionType == other.resolutionType && isFullScreen == other.isFullScreen;
        }

        public object Clone()
        {
            return new ResolutionData
                   {
                       resolutionType = this.resolutionType,
                       isFullScreen = this.isFullScreen
                   };
        }
    }

    public class ResolutionOption : Option<ResolutionData>
    {
        public ResolutionOption(bool isApplyImmediately) : base(isApplyImmediately) { }
        
        protected override void _Reset()
        {
        }

        protected override void _Apply()
        {
            base._Apply();
            
            switch (NewData.resolutionType)
            {
                case ResolutionType.R1920x1080:
                    Screen.SetResolution(1920, 1080, CurrentData.isFullScreen);
                    break;
                case ResolutionType.R1600x900:
                    Screen.SetResolution(1600, 900, CurrentData.isFullScreen);
                    break;
                case ResolutionType.R1440x900:
                    Screen.SetResolution(1440, 900, CurrentData.isFullScreen);
                    break;
                case ResolutionType.R1280x1024:
                    Screen.SetResolution(1280, 1024, CurrentData.isFullScreen);
                    break;
                case ResolutionType.R1280x960:
                    Screen.SetResolution(1280, 960, CurrentData.isFullScreen);
                    break;
                case ResolutionType.R1280x720:
                    Screen.SetResolution(1280, 720, CurrentData.isFullScreen);
                    break;
                case ResolutionType.R1024x768:
                    Screen.SetResolution(1024, 768, CurrentData.isFullScreen);
                    break;
            }
        }
    }
}