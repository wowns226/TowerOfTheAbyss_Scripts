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
using UnityEngine;

namespace ProjectL
{
    public enum TechnologyActiveStatus
    {
        None = 0,
        Activate = 1,
        FirstTripod = 2,
        SecondTripod = 3,
        ThirdTripod = 4,
    }

    public abstract class Technology : MonoBehaviour, ISetting, IBagItem
    {
        protected Player player;
        public Player Player => player;
        
        [SettingValue]
        protected string displayName;
        public string DisplayName => Localization.GetLocalizedString(displayName);
        
        [SettingValue]
        protected Sprite icon;

        public Sprite Icon => icon;
        
        public string Name => GetType().Name;

        private TechnologyActiveStatus status;
        public TechnologyActiveStatus Status
        {
            get => status;
            private set
            {
                status = value;
                onChangedStatus?.Invoke();
            }
        }

        [SettingValue]
        protected int activatePrice;
        public int ActivatePrice => activatePrice;
        [SettingValue]
        protected int firstTripodPrice;
        public int FirstTripodPrice => firstTripodPrice;
        [SettingValue]
        protected int secondTripodPrice;
        public int SecondTripodPrice => secondTripodPrice;
        [SettingValue]
        protected int thirdTripodPrice;
        public int ThirdTripodPrice => thirdTripodPrice;

        [SettingValue]
        protected string shortDescription;
        public virtual string ShortDescription => string.Format(Localization.GetLocalizedString(shortDescription));

        [SettingValue]
        protected string fullDescription;
        public virtual string FullDescription => string.Format(Localization.GetLocalizedString(fullDescription));

        public List<Tripod> firstTripods = new List<Tripod>();
        private int firstTripodIndex = -1;
        public int FirstTripodIndex => firstTripodIndex;

        public List<Tripod> secondTripods = new List<Tripod>();
        private int secondTripodIndex = -1;
        public int SecondTripodIndex => secondTripodIndex;

        public List<Tripod> thirdTripods = new List<Tripod>();
        private int thirdTripodIndex = -1;
        public int ThirdTripodIndex => thirdTripodIndex;

        public Action onChangedStatus;
        
        public void Init(Player player)
        {
            this.player = player;

            this.SetValue();
            
            InitTripods();
        }
        
        protected abstract void InitTripods();

        public int NextActivatePrice()
        {
            switch (Status)
            {
                case TechnologyActiveStatus.None:
                    return ActivatePrice;
                case TechnologyActiveStatus.Activate:
                    return FirstTripodPrice;
                case TechnologyActiveStatus.FirstTripod:
                    return SecondTripodPrice;
                case TechnologyActiveStatus.SecondTripod:
                    return ThirdTripodPrice;
                case TechnologyActiveStatus.ThirdTripod:
                    return int.MaxValue;
            }

            return int.MaxValue;
        }

        public void ActiveNextStatus()
        {
            switch (Status)
            {
                case TechnologyActiveStatus.None:
                    Status = TechnologyActiveStatus.Activate;
                    Active();
                    break;
                case TechnologyActiveStatus.Activate:
                    Status = TechnologyActiveStatus.FirstTripod;
                    firstTripodIndex = 0;
                    ActiveFirstTripod();
                    break;
                case TechnologyActiveStatus.FirstTripod:
                    Status = TechnologyActiveStatus.SecondTripod;
                    secondTripodIndex = 0;
                    ActiveSecondTripod();
                    break;
                case TechnologyActiveStatus.SecondTripod:
                    Status = TechnologyActiveStatus.ThirdTripod;
                    thirdTripodIndex = 0;
                    ActiveThirdTripod();
                    break;
                case TechnologyActiveStatus.ThirdTripod:
                    break;
            }
        }

        public void DeActivateAll()
        {
            switch (Status)
            {
                case TechnologyActiveStatus.None:
                    break;
                case TechnologyActiveStatus.Activate:
                    DeActive();
                    break;
                case TechnologyActiveStatus.FirstTripod:
                    DeActive();
                    DeActiveFirstTripod();
                    break;
                case TechnologyActiveStatus.SecondTripod:
                    DeActive();
                    DeActiveFirstTripod();
                    DeActiveSecondTripod();
                    break;
                case TechnologyActiveStatus.ThirdTripod:
                    DeActive();
                    DeActiveFirstTripod();
                    DeActiveSecondTripod();
                    DeActiveThirdTripod();
                    break;
            }

            Status = TechnologyActiveStatus.None;
            firstTripodIndex = -1;
            secondTripodIndex = -1;
            thirdTripodIndex = -1;
        }

        public void SelectTripod(TechnologyActiveStatus technologyActiveStatus, int index)
        {
            if(Status < technologyActiveStatus)
            {
                Debug.Log($"Technology.SelectTripod(), not Active target Status : {technologyActiveStatus.ToString()}");
                return;
            }

            switch (technologyActiveStatus)
            {
                case TechnologyActiveStatus.FirstTripod:
                    ChangeFirstTripod(index);
                    break;
                case TechnologyActiveStatus.SecondTripod:
                    ChangeSecondTripod(index);
                    break;
                case TechnologyActiveStatus.ThirdTripod:
                    ChangeThirdTripod(index);
                    break;
            }
        }

        protected abstract void Active();
        protected abstract void DeActive();

        private void ActiveFirstTripod() => firstTripods[firstTripodIndex].Activate();
        private void ActiveSecondTripod() => secondTripods[secondTripodIndex].Activate();
        private void ActiveThirdTripod() => thirdTripods[thirdTripodIndex].Activate();
        private void DeActiveFirstTripod() => firstTripods[firstTripodIndex].DeActivate();
        private void DeActiveSecondTripod() => secondTripods[secondTripodIndex].DeActivate();
        private void DeActiveThirdTripod() => thirdTripods[thirdTripodIndex].DeActivate();
        private void ChangeFirstTripod(int index)
        {
            if(index >= firstTripods.Count)
            {
                Debug.Log($"Technology.ChangeFirstTripod(), index over the count : {index}");
                return;
            }

            if (firstTripodIndex == index)
            {
                return;
            }
            
            DeActiveFirstTripod();
            firstTripodIndex = index;
            ActiveFirstTripod();
        }
        private void ChangeSecondTripod(int index)
        {
            if (index >= secondTripods.Count)
            {
                Debug.Log($"Technology.ChangeSecondTripod(), index over the count : {index}");
                return;
            }
            
            if (secondTripodIndex == index)
            {
                return;
            }

            DeActiveSecondTripod();
            secondTripodIndex = index;
            ActiveSecondTripod();
        }
        private void ChangeThirdTripod(int index)
        {
            if (index >= thirdTripods.Count)
            {
                Debug.Log($"Technology.ChangeThirdTripod(), index over the count : {index}");
                return;
            }
            
            if (thirdTripodIndex == index)
            {
                return;
            }

            DeActiveThirdTripod();
            thirdTripodIndex = index;
            ActiveThirdTripod();
        }
    }
}

