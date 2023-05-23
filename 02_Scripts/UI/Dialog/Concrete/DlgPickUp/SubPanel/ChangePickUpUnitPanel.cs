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
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ProjectL
{
    public class ChangePickUpUnitPanel : DataContainer
    {
        [SerializeField]
        private DlgPickUp parentPanel;
        [SerializeField]
        private SkillInfoPopup focusUnitIndividualSkillInfoPopup;
        [SerializeField]
        private SkillInfoPopup focusUnitExtraSkillInfoPopup;
        [SerializeField]
        private List<SkillInfoPopup> focusUnitSkillInfoPopups = new List<SkillInfoPopup>(); 
        [SerializeField]
        private SkillInfoPopup changeUnitIndividualSkillInfoPopup;
        [SerializeField]
        private SkillInfoPopup changeUnitExtraSkillInfoPopup;
        [SerializeField]
        private List<SkillInfoPopup> changeUnitSkillInfoPopups = new List<SkillInfoPopup>();
        [SerializeField]
        private List<UnitItemInfo> unitTabItems = new List<UnitItemInfo>();
        [SerializeField]
        private DefaultSetToggleGroup defaultSetToggleGroup;
        
        private Unit focusUnit;
        private Unit FocusUnit
        {
            get => focusUnit;
            set
            {
                focusUnit = value;
                InitFocusUnitSkills();
                this.NotifyObserver();
            }
        }
        
        private Unit changedUnit;
        public Unit ChangedUnit
        {
            get => changedUnit;
            set
            {
                changedUnit = value;
                InitChangeUnitSkills();
                this.NotifyObserver();
            }
        }
        
        [DataObservable]
        private int UnitCount => D.SelfPlayer.Mobs.Count;
        
        [DataObservable]
        private bool IsUnitCountOverZero => UnitCount >= 0;
        [DataObservable]
        private bool IsUnitCountOverOne => UnitCount >= 1;
        [DataObservable]
        private bool IsUnitCountOverTwo => UnitCount >= 2;
        [DataObservable]
        private bool IsUnitCountOverThree => UnitCount >= 3;
        [DataObservable]
        private bool IsUnitCountOverFour => UnitCount >= 4;
        [DataObservable]
        private bool IsUnitCountOverFive => UnitCount >= 5;


        #region FocusUnit

        [DataObservable]
        private string UnitName => $"{FocusUnit?.DisplayName}";
        [DataObservable]
        private Sprite UnitIcon => FocusUnit?.Icon;
        [DataObservable]
        private Sprite UnitMiniImage => FocusUnit?.UnitMiniImage;
        [DataObservable]
        private Sprite UnitImage => FocusUnit?.UnitImage;
        
        [DataObservable]
        private bool IsCommonUnit => FocusUnit && FocusUnit.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRareUnit => FocusUnit && FocusUnit.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUniqueUnit => FocusUnit && FocusUnit.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpicUnit => FocusUnit && FocusUnit.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecialUnit => FocusUnit && FocusUnit.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendaryUnit => FocusUnit && FocusUnit.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncientUnit => FocusUnit && FocusUnit.GradeType == GradeType.Ancient;

        [DataObservable]
        private bool IsNotLessThanCommon => FocusUnit && FocusUnit.GradeType >= GradeType.Common;
        [DataObservable]
        private bool IsNotLessThanRare => FocusUnit && FocusUnit.GradeType >= GradeType.Rare;
        [DataObservable]
        private bool IsNotLessThanUnique => FocusUnit && FocusUnit.GradeType >= GradeType.Unique;
        [DataObservable]
        private bool IsNotLessThanEpic => FocusUnit && FocusUnit.GradeType >= GradeType.Epic;
        [DataObservable]
        private bool IsNotLessThanSpecial => FocusUnit && FocusUnit.GradeType >= GradeType.Special;
        [DataObservable]
        private bool IsNotLessThanLegendary => FocusUnit && FocusUnit.GradeType >= GradeType.Legendary;
        [DataObservable]
        private bool IsNotLessThanAncient => FocusUnit && FocusUnit.GradeType >= GradeType.Ancient;

        [DataObservable]
        private bool IsHealer => FocusUnit && FocusUnit.AttackType == AttackType.Heal;
        
        [DataObservable]
        private string Hp => $"<B>{FocusUnit?.BaseMaxHp ?? 0:#,##0}</B><size=80%>({FocusUnit?.MaxHpAddedDisplayValue})</size>";
        [DataObservable]
        private string Damage => $"<B>{FocusUnit?.BaseDamage ?? 0:#,##0}</B><size=80%>({FocusUnit?.DamageAddedDisplayValue})</size>";
        [DataObservable]
        private string Heal => $"<B>{FocusUnit?.BaseHeal ?? 0:#,##0}</B><size=80%>({FocusUnit?.HealAddedDisplayValue})</size>";
        [DataObservable]
        private string Defense => $"<B>{FocusUnit?.BaseDefense ?? 0:#,##0}</B><size=80%>({FocusUnit?.DefenseAddedDisplayValue})</size>";
        [DataObservable]
        private string CriticalProbability => $"<B>{FocusUnit?.BaseCriticalProbability ?? 0:#,##0}</B><size=80%>({FocusUnit?.CriticalProbabilityAddedDisplayValue})</size>";
        [DataObservable]
        private string CriticalDamage => $"<B>{FocusUnit?.BaseCriticalDamagePercentage ?? 0:#,##0}</B><size=80%>({FocusUnit?.CriticalDamagePercentageAddedDisplayValue})</size>";
        [DataObservable]
        private string Range => $"<B>{FocusUnit?.BaseRange ?? 0:#,##0}</B><size=80%>({FocusUnit?.RangeAddedDisplayValue})</size>";
        [DataObservable]
        private string Drain => $"<B>{FocusUnit?.BaseDrainProbability ?? 0:#,##0}</B><size=80%>({FocusUnit?.DrainProbabilityAddedDisplayValue})</size>";
        [DataObservable]
        private string Speed => $"<B>{FocusUnit?.BaseSpeed ?? 0:#,##0}</B><size=80%>({FocusUnit?.SpeedAddedDisplayValue})</size>";
        [DataObservable]
        private string DefenseMouseOver =>
            string.Format(Localization.GetLocalizedString("Unit/Defense/ReducePercentage"),
                FocusUnit?.ReductionByDefenseRate * 100f ?? 0);
        
        [DataObservable]
        private bool IsExistExtraSkill => FocusUnit?.SkillGroup.ExtraSkill != null;
        [DataObservable]
        private bool IsExistIndividualSkill => FocusUnit?.SkillGroup.PassiveSkill != null;

        #endregion

        #region ChangedUnit

        [DataObservable]
        private string ChangedUnitName => $"{ChangedUnit?.DisplayName}";
        [DataObservable]
        private Sprite ChangedUnitIcon => ChangedUnit?.Icon;
        [DataObservable]
        private Sprite ChangedUnitMiniImage => ChangedUnit?.UnitMiniImage;
        [DataObservable]
        private Sprite ChangedUnitImage => ChangedUnit?.UnitImage;
        
        [DataObservable]
        private bool IsChangedUnitCommonUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsChangedUnitRareUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsChangedUnitUniqueUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsChangedUnitEpicUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsChangedUnitSpecialUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsChangedUnitLegendaryUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsChangedUnitAncientUnit => ChangedUnit && ChangedUnit.GradeType == GradeType.Ancient;

        [DataObservable]
        private bool IsChangedUnitNotLessThanCommon => ChangedUnit && ChangedUnit.GradeType >= GradeType.Common;
        [DataObservable]
        private bool IsChangedUnitNotLessThanRare => ChangedUnit && ChangedUnit.GradeType >= GradeType.Rare;
        [DataObservable]
        private bool IsChangedUnitNotLessThanUnique => ChangedUnit && ChangedUnit.GradeType >= GradeType.Unique;
        [DataObservable]
        private bool IsChangedUnitNotLessThanEpic => ChangedUnit && ChangedUnit.GradeType >= GradeType.Epic;
        [DataObservable]
        private bool IsChangedUnitNotLessThanSpecial => ChangedUnit && ChangedUnit.GradeType >= GradeType.Special;
        [DataObservable]
        private bool IsChangedUnitNotLessThanLegendary => ChangedUnit && ChangedUnit.GradeType >= GradeType.Legendary;
        [DataObservable]
        private bool IsChangedUnitNotLessThanAncient => ChangedUnit && ChangedUnit.GradeType >= GradeType.Ancient;

        [DataObservable]
        private bool IsChangedUnitHealer => ChangedUnit && ChangedUnit.AttackType == AttackType.Heal;
        
        [DataObservable]
        private string ChangedUnitHp => $"<B>{ChangedUnit?.BaseMaxHp ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitDamage => $"<B>{ChangedUnit?.BaseDamage ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitHeal => $"<B>{ChangedUnit?.BaseHeal ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitDefense => $"<B>{ChangedUnit?.BaseDefense ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitCriticalProbability => $"<B>{ChangedUnit?.BaseCriticalProbability ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitCriticalDamage => $"<B>{ChangedUnit?.BaseCriticalDamagePercentage ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitRange => $"<B>{ChangedUnit?.BaseRange ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitDrain => $"<B>{ChangedUnit?.BaseDrainProbability ?? 0:#,##0}</B>";
        [DataObservable]
        private string ChangedUnitSpeed => $"<B>{ChangedUnit?.BaseSpeed ?? 0:#,##0}</B>";
        
        [DataObservable]
        private bool IsExistChangedUnitExtraSkill => ChangedUnit?.SkillGroup.ExtraSkill != null;
        [DataObservable]
        private bool IsExistChangedUnitIndividualSkill => ChangedUnit?.SkillGroup.PassiveSkill != null;

        #endregion
        
        private void Start()
        {
            FocusUnit = D.SelfPlayer.Mobs[0];
            this.NotifyObserver();
            
            D.SelfPlayer.onAddUnit += NotifyObserver;
            D.SelfPlayer.onClearedUnit += NotifyObserverMine;
        }

        private void OnDestroy()
        {
            D.SelfPlayer.onAddUnit -= NotifyObserver;
            D.SelfPlayer.onClearedUnit -= NotifyObserverMine;
        }

        private void OnEnable()
        {
            InitTabUnits();
            defaultSetToggleGroup.defaultToggle.isOn = true;
            this.NotifyObserver();
        }

        private void NotifyObserver(Unit unit)
        {
            if (unit.UnitType != UnitType.Mob)
            {
                return;
            }
            
            InitTabUnits();
            defaultSetToggleGroup.defaultToggle.isOn = true;
            this.NotifyObserver();
        }

        public void NotifyObserverMine()
        {
            this.NotifyObserver();
        }

        private void InitTabUnits()
        {
            for (int i = 0; i < unitTabItems.Count; i++)
            {
                var mobs = D.SelfPlayer.Mobs;
                
                if (mobs?.Count <= i)
                {
                    break;
                }
                
                unitTabItems[i].Unit = mobs?[i];
            }
            
            FocusUnit = D.SelfPlayer.Mobs[0];
            unitTabItems.ForEach(item => item.NotifyObserver());
        }

        private void InitSkills()
        {
            InitFocusUnitSkills();
            InitChangeUnitSkills();
        }

        private void InitFocusUnitSkills()
        {
            InitBasicSkills(FocusUnit, focusUnitSkillInfoPopups);
            InitExtraSkill(FocusUnit, focusUnitExtraSkillInfoPopup);
            InitIndividualSkill(FocusUnit, focusUnitIndividualSkillInfoPopup);
        }
        
        private void InitChangeUnitSkills()
        {
            InitBasicSkills(changedUnit, changeUnitSkillInfoPopups);
            InitExtraSkill(changedUnit, changeUnitExtraSkillInfoPopup);
            InitIndividualSkill(changedUnit, changeUnitIndividualSkillInfoPopup);
        }

        private void InitBasicSkills(Unit unit, List<SkillInfoPopup> skillInfoPopups)
        {
            var skills = unit.SkillGroup.GetAllSkills();

            for (int i = 0; i < skillInfoPopups.Count; i++)
            {
                if (skills.Count > i)
                {
                    skillInfoPopups[i].gameObject.SetActive(true);
                    skillInfoPopups[i].Init(skills[i]);
                }
                else
                {
                    skillInfoPopups[i].gameObject.SetActive(false);
                }
            }
        }

        private void InitExtraSkill(Unit unit, SkillInfoPopup skillInfoPopup)
        {
            var extraSkill = unit.SkillGroup.ExtraSkill;

            if (extraSkill == null)
            {
                return;
            }
            
            skillInfoPopup.Init(extraSkill);
        }
        
        private void InitIndividualSkill(Unit unit, SkillInfoPopup skillInfoPopup)
        {
            var individualSkill = unit.SkillGroup.PassiveSkill;

            if (individualSkill == null)
            {
                return;
            }
            
            skillInfoPopup.Init(individualSkill);
        }
        
        public void ChangeToggleFocusUnit(int index)
        {
            if (index >= D.SelfPlayer.Mobs.Count)
            {
                Debug.LogError($"ChangePickUpUnitPanel.ChangeToggleFocusUnit(), index out of range, index : {index}");
                return;
            }
            
            FocusUnit = D.SelfPlayer.Mobs?[index];
        }

        public void ClickUnLockHeroUnit()
        {
            Debug.Log($"ChangePickUpUnitPanel.ClickUnLockHeroUnit()");
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgProfile/Unit/BuyHeroSlot/Title");
                dialog.Content = $"{Localization.GetLocalizedString("DlgProfile/Unit/BuyHeroSlot/Content")}{D.SelfPlayer.NextUnitUnLockCost}";
                dialog.AddOKEvent(() => 
                {
                    if (D.SelfPlayer.IsEnoughNextUnitUnLockCost)
                    {
                        D.SelfPlayer.CreateHeroUnit();
                    }
                    else
                    {
                        DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.NextUnitUnLockCost}"; });
                    }
                });
            });
        }

        public void ClickChangeUnit()
        {
            Debug.Log("ChangePickUpUnitPanel.ChangeUnit()");
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                var focusUnitDisplayNameWithColor = FocusUnit.DisplayName;
                
                dialog.Title = Localization.GetLocalizedString("DlgPickUp/ChangeUnit/Title");
                
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgPickUp/ChangeUnit/Content"),
                    $"<color={GetGradeColor(FocusUnit.GradeType)}>{FocusUnit.DisplayName}</color>",
                    $"<color={GetGradeColor(ChangedUnit.GradeType)}>{ChangedUnit.DisplayName}</color>");
                
                dialog.AddOKEvent(() => 
                {
                    FocusUnit.Clear();
                    
                    ChangedUnit.Init(D.SelfPlayer);
                    ClickBack();
                });
            });
            
        }

        private string GetGradeColor(GradeType gradeType)
        {
            switch (gradeType)
            {
                case GradeType.Ancient:
                    return "white";
                case GradeType.Legendary:
                    return "yellow";
                case GradeType.Special:
                    return "green";
                case GradeType.Epic:
                    return "purple";
                case GradeType.Unique:
                    return "red";
                case GradeType.Rare:
                    return "blue";
                default:
                    return "grey";
            }
        }
        
        public void ClickBack()
        {
            parentPanel.OpenPanel("Pick");
            parentPanel.StartOpenedAllPickUpUnits();
        }
    }
}