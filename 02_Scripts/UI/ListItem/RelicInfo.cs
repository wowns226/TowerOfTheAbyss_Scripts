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
    public class RelicInfo : DataContainer
    {
        private Relic relic;
        public Relic Relic
        {
            get => relic;
            private set
            { 
                RemoveEvent();
                relic = value;
                this.NotifyObserver();
                RegisterEvent();
            }
        }

        public Toggle toggle;
        public CustomAction<Relic> onToggleAction = new CustomAction<Relic>();
        
        #region Observable

        [DataObservable]
        private bool IsToggleOn => toggle.isOn;
        
        [DataObservable]
        private Sprite RelicIcon => Relic?.Icon;
        [DataObservable]
        private string RelicName => Relic?.DisplayName;
        
        [DataObservable]
        private bool IsActive => Relic?.IsActive ?? false;

        [DataObservable]
        private bool IsCommon => Relic && Relic.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => Relic && Relic.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => Relic && Relic.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => Relic && Relic.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => Relic && Relic.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => Relic && Relic.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => Relic && Relic.GradeType == GradeType.Ancient;

        #endregion

        public void Init(Relic relic, ToggleGroup toggleGroup)
        {
            Relic = relic;
            toggle.group = toggleGroup;

            this.NotifyObserver();
        }

        public void Init(Relic relic)
        {
            Relic = relic;
            
            this.NotifyObserver();
        }
        
        private void RegisterEvent()
        {
            if (Relic != null)
            {
                Relic.onActiveRelic.Add(this.NotifyObserver);
                Relic.onInActiveRelic.Add(this.NotifyObserver);
            }
        }

        private void RemoveEvent()
        {
            if (Relic != null)
            {
                Relic.onActiveRelic.Remove(this.NotifyObserver);
                Relic.onInActiveRelic.Remove(this.NotifyObserver);
            }
        }
        
        public void OnToggleChange(bool isOn)
        {
            if (isOn)
            {
                Debug.Log($"RelicInfo.OnToggleChange(), Relic : {Relic.name}");
                onToggleAction?.Invoke(Relic);
            }
            
            this.NotifyObserver("IsToggleOn");
        }
        
    }
}

