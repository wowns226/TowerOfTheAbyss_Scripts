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
using Newtonsoft.Json;
using UnityEngine;

namespace ProjectL
{
    [SerializeField]
    public class QualityData : ISaveAndLoadToJson<QualityData>, IEquatable<QualityData>, ICloneable
    {
        public QualityData()
        {
            qualityType = QualityType.High;
            isEnableSkillEffect = true;
        }
        
        public QualityType qualityType;
        public bool isEnableSkillEffect;

        [JsonIgnore]
        public bool isIgnoreSaveBool; // 저장 안되는지 확인용 변수
        
        public bool Equals(QualityData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return qualityType == other.qualityType 
                   && isEnableSkillEffect == other.isEnableSkillEffect;
        }

        public object Clone()
        {
            return new QualityData
                   {
                       qualityType = this.qualityType,
                       isEnableSkillEffect = this.isEnableSkillEffect
                   };
        }
    }

    public class QualityOption : Option<QualityData>
    {
        public QualityOption(bool isApplyImmediately) : base(isApplyImmediately) { }
        
        protected override void _Reset()
        {
        }

        protected override void _Apply()
        {
            base._Apply();
            SkillManager.Instance.isShowSkillEffect = CurrentData.isEnableSkillEffect;
            VisualManager.Instance.SetQuality(CurrentData.qualityType);
        }
    }
}