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
using System.Text;
using UnityEngine;

namespace ProjectL
{
    public abstract class Relic : MonoBehaviour, IRelic, ISetting ,IBagItem
    {
        protected Player player;
        public Player Player => player;

        private GradeType gradeType;
        public GradeType GradeType
        {
            get => gradeType;
            private set
            {
                gradeType = value;
                
                SetMouseOverDescription();
            }
        }

        public virtual bool CompletedDev { get; protected set; } = true;

        [SettingValue]
        protected bool isGradeType;
        public bool IsGradeType => isGradeType;
        
        StringBuilder sb = new StringBuilder();
        
        [SettingValue]
        protected Sprite icon;
        public Sprite Icon => icon;

        [SettingValue]
        protected string displayName;
        public string DisplayName => Localization.GetLocalizedString(displayName);

        [SettingValue]
        protected string description;
        public string Description => GetDescription(GradeType);

        private string mouseOverDescription;
        public string MouseOverDescription => mouseOverDescription;

        public virtual string CommonDetailDescription { get; }
        public virtual string RareDetailDescription { get; }
        public virtual string UniqueDetailDescription { get; }
        public virtual string EpicDetailDescription { get; }
        public virtual string SpecialDetailDescription { get; }
        public virtual string LegendaryDetailDescription { get; }
        public virtual string AncientDetailDescription { get; }

        protected int commonSellGold = 5;
        protected int rareSellGold = 10;
        protected int uniqueSellGold = 20;
        protected int epicSellGold = 30;
        protected int specialSellGold = 50;
        protected int legendarySellGold = 100;
        protected int ancientSellGold = 200;

        public bool IsActive { get; private set; }
        
        private List<RelicSet> relicSetList = new List<RelicSet>();
        public List<RelicSet> RelicSetList => relicSetList;

        public CustomAction onActiveRelic = new CustomAction();
        public CustomAction onInActiveRelic = new CustomAction();

        public virtual void Init(Player player)
        {
            this.SetValue();
            
            this.player = player;
            InitRelicSet();
            GradeType = GradeType.Common;
            
            Localization.onLocalizeChanged.Add(SetMouseOverDescription);
        }
        
        protected abstract void InitRelicSet();
        
        protected void AddRelicSet(RelicSet relicSet)
        {
            relicSetList.Add(relicSet);
            relicSet.AddRelic(this);
        }

        public void Activate(GradeType gradeType)
        {
            if (IsActive)
            {
                return;
            }
            
            Debug.Log($"{DisplayName}.Activate()");

            IsActive = true;
            GradeType = gradeType;

            ActivateByGradeType();
            UpdateRelicSetActivate();
            
            onActiveRelic.Invoke();
        }

        private void ActivateByGradeType()
        {
            switch (GradeType)
            {
                case GradeType.Ancient:
                    _ActivateAncient();
                    break;
                case GradeType.Legendary:
                    _ActivateLegendary();
                    break;
                case GradeType.Special:
                    _ActivateSpecial();
                    break;
                case GradeType.Epic:
                    _ActivateEpic();
                    break;
                case GradeType.Unique:
                    _ActivateUnique();
                    break;
                case GradeType.Rare:
                    _ActivateRare();
                    break;
                default:
                    _ActivateCommon();
                    break;
            }
        }

        public void InActivate()
        {
            if(IsActive == false)
                return;

            Debug.Log($"{DisplayName}.InActivate()");
            
            InActivateByGradeType();
            UpdateRelicSetActivate();
            
            IsActive = false;
            GradeType = GradeType.Common;
            
            onInActiveRelic.Invoke();
        }
        
        private void InActivateByGradeType()
        {
            switch (GradeType)
            {
                case GradeType.Ancient:
                    _InActivateAncient();
                    break;
                case GradeType.Legendary:
                    _InActivateLegendary();
                    break;
                case GradeType.Special:
                    _InActivateSpecial();
                    break;
                case GradeType.Epic:
                    _InActivateEpic();
                    break;
                case GradeType.Unique:
                    _InActivateUnique();
                    break;
                case GradeType.Rare:
                    _InActivateRare();
                    break;
                default:
                    _InActivateCommon();
                    break;
            }
        }
        
        protected abstract void _ActivateCommon();
        protected abstract void _ActivateRare();
        protected abstract void _ActivateUnique();
        protected abstract void _ActivateEpic();
        protected abstract void _ActivateSpecial();
        protected abstract void _ActivateLegendary();
        protected abstract void _ActivateAncient();
        protected abstract void _InActivateCommon();
        protected abstract void _InActivateRare();
        protected abstract void _InActivateUnique();
        protected abstract void _InActivateEpic();
        protected abstract void _InActivateSpecial();
        protected abstract void _InActivateLegendary();
        protected abstract void _InActivateAncient();

        private void UpdateRelicSetActivate()
        {
            foreach(var relicSet in relicSetList)
            {
                relicSet.UpdateRelicSetActivate();
            }
        }
        
        public int GetSellGold() => GetSellGold(GradeType);
        public int GetSellGold(GradeType gradeType)
        {
            switch (gradeType)
            {
                case GradeType.Ancient:
                    return ancientSellGold;
                case GradeType.Legendary:
                    return legendarySellGold;
                case GradeType.Special:
                    return specialSellGold;
                case GradeType.Epic:
                    return epicSellGold;
                case GradeType.Unique:
                    return uniqueSellGold;
                case GradeType.Rare:
                    return rareSellGold;
            }
            
            return commonSellGold;
        }
        
        private void SetMouseOverDescription()
        {
            if (IsGradeType == true)
                _SetMouseOverDescriptionAllGradeType();
            else
                _SetMouseOverDescription();
        }
        
        private void _SetMouseOverDescriptionAllGradeType()
        {
            sb.Clear();

            var enumValues = Enum.GetValues(typeof(GradeType));

            for (int i = enumValues.Length - 1; i >= 0; i--)
            {
                GradeType type = (GradeType)enumValues.GetValue(i);

                if (type != GradeType)
                {
                    sb.AppendLine(GetDescription(type));
                }
            }

            mouseOverDescription = sb.ToString();
        }

        private void _SetMouseOverDescription()
        {
            sb.Clear();

            var enumValues = GradeType.Special;

            sb.AppendLine(GetDescription(enumValues));

            mouseOverDescription = sb.ToString();
        }

        public string GetDescription(GradeType gradeType)
        {
            if(!IsGradeType)
                return $"<color=orange>Single :</color> {SpecialDetailDescription}";

            switch (gradeType)
            {
                case GradeType.Ancient:
                    return $"<color=white>Ancient :</color> {AncientDetailDescription}";
                case GradeType.Legendary:
                    return $"<color=yellow>Legendary :</color> {LegendaryDetailDescription}";
                case GradeType.Special:
                    return $"<color=green>Special :</color> {SpecialDetailDescription}";
                case GradeType.Epic:
                    return $"<color=purple>Epic :</color> {EpicDetailDescription}";
                case GradeType.Unique:
                    return $"<color=red>Unique :</color> {UniqueDetailDescription}";
                case GradeType.Rare:
                    return $"<color=blue>Rare :</color> {RareDetailDescription}";
                case GradeType.Common:
                    return $"<color=grey>Common :</color> {CommonDetailDescription}";
            }
            
            return CommonDetailDescription;
        }
        
    }
}

