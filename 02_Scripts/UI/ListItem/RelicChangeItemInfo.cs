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

namespace ProjectL
{
    public class RelicChangeItemInfo : DataContainer
    {
        private Relic relic;
        public Relic Relic
        {
            get => relic;
            private set
            { 
                relic = value;
                this.NotifyObserver();
            }
        }

        public CustomAction<Relic> onToggleAction = new CustomAction<Relic>();
        
        #region Observable

        [DataObservable]
        private Sprite RelicIcon => Relic?.Icon;
        [DataObservable]
        private string RelicName => Relic?.DisplayName;
        
        [DataObservable]
        private bool IsCommon => Relic?.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => Relic?.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => Relic?.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => Relic?.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => Relic?.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => Relic?.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => Relic?.GradeType == GradeType.Ancient;
        
        #endregion

        public void Init(Relic relic)
        {
            Relic = relic;
            
            this.NotifyObserver();
        }
        
        public void OnToggleChange(bool isOn)
        {
            if (isOn)
            {
                Debug.Log($"RelicInfo.OnToggleChange(), Relic : {Relic?.name}");
                onToggleAction?.Invoke(Relic);
            }
        }
        
    }
}

