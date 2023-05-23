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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public abstract class RelicSet : MonoBehaviour, IRelic, ISetting, IBagItem
    {
        protected Player player;
        public Player Player => player;
    
        [SettingValue]
        protected Sprite icon;
        public Sprite Icon => icon;
        
        [SettingValue]
        protected string displayName;
        public string DisplayName => Localization.GetLocalizedString(displayName);
        
        [SettingValue]
        protected string detailDescription;
        public virtual string DetailDescription => Localization.GetLocalizedString(detailDescription);

        [SettingValue]
        protected int activeRelicCount;
        public int ActiveRelicCount => activeRelicCount;
        
        public bool IsActive { get; private set; }

        public List<Relic> relics = new List<Relic>();
        public int CurrentActiveRelicCount => relics.Count(relic => relic.IsActive);

        public void Init(Player player)
        {
            this.player = player;
            this.SetValue();
        }
        
        public void AddRelic(Relic relic)
        {
            relics.Add(relic);
        }

        public void UpdateRelicSetActivate()
        {
            if(CanActivate())
                Activate();
            else
                InActivate();
        }

        private bool CanActivate() => CurrentActiveRelicCount >= activeRelicCount;

        private void Activate()
        {
            if (IsActive)
            {
                Debug.Log($"{DisplayName}.Activate(), Already Activated");
                return;
            }

            Debug.Log($"{DisplayName}.Activate()");

            IsActive = true;

            _Activate();
        }

        private void InActivate()
        {
            if (IsActive == false)
            {
                Debug.Log($"{DisplayName}.InActivate(), Already InActivated");
                return;
            }

            Debug.Log($"{DisplayName}.InActivate()");

            IsActive = false;

            _InActivate();
        }

        protected abstract void _Activate();
        protected abstract void _InActivate();

    }
}
