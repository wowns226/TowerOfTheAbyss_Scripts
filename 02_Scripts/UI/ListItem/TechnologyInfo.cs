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
using UnityEngine.UI;

namespace ProjectL
{
    public class TechnologyInfo : DataContainer
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private Technology technology;
        public Technology Technology
        {
            get => technology;
            private set
            { 
                technology = value;
                this.NotifyObserver();
            }
        }

        public Toggle toggle;
        public Action<Technology> onToggleAction;
        
        #region Observable


        [DataObservable]
        private Sprite TechnologyIcon => Technology.Icon;
        [DataObservable]
        private string TechnologyName => Technology == null ? string.Empty : Technology.DisplayName;

        
        [DataObservable]
        private bool IsTechnologyBuy => (Technology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.Activate;
        
        
        [DataObservable]
        private bool IsTechnologyFirstTripodBuy => (Technology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.FirstTripod;
        [DataObservable]
        private bool IsTechnologyFirstTripodFirstExist => (Technology?.firstTripods?.Count ?? 0) > 0;
        
        
        [DataObservable]
        private bool IsTechnologySecondTripodBuy => (Technology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.SecondTripod;
        [DataObservable]
        private bool IsTechnologySecondTripodFirstExist => (Technology?.secondTripods?.Count ?? 0) > 0;
        
        
        [DataObservable]
        private bool IsTechnologyThirdTripodBuy => (Technology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.ThirdTripod;
        [DataObservable]
        private bool IsTechnologyThirdTripodFirstExist => (Technology?.thirdTripods?.Count ?? 0) > 0;
        
        
        #endregion

        private void OnDestroy()
        {
            Technology.onChangedStatus -= this.NotifyObserver;
        }

        public void Init(Technology technology, ToggleGroup toggleGroup)
        {
            Technology = technology;
            toggle.group = toggleGroup;

            Technology.onChangedStatus += this.NotifyObserver;
            this.NotifyObserver();
        }

        public void OnToggleChange(bool isOn)
        {
            if (isOn)
            {
                Debug.Log($"TechnologyInfo.OnToggleChange(), Technology : {Technology.name}");
                onToggleAction?.Invoke(Technology);
            }
        }
        
        
    }
}

