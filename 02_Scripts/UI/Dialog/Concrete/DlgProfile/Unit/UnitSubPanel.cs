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
    public class UnitSubPanel : DataContainer
    {
        [SerializeField]
        private SkillInfoPopup extraSkillInfoPopup;
        [SerializeField]
        private SkillInfoPopup individualSkillInfoPopup;
        [SerializeField]
        private List<SkillInfoPopup> skillInfoPopups = new List<SkillInfoPopup>();        
        [SerializeField]
        private List<UnitItemInfo> unitTabItems = new List<UnitItemInfo>();
        [SerializeField]
        private GameObject starEffect;
        
        private Unit focusUnit;
        private Unit FocusUnit
        {
            get => focusUnit;
            set
            {
                focusUnit = value;
                InitSkills();
                ReplayAnimation();
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
        private bool IsMelee => FocusUnit && FocusUnit.AttackType == AttackType.Melee;
        [DataObservable]
        private bool IsHealer => FocusUnit && FocusUnit.AttackType == AttackType.Heal;
        [DataObservable]
        private bool IsRanged => FocusUnit && FocusUnit.AttackType == AttackType.Ranged;

        [DataObservable]
        private string UnitAttackType => $"{Localization.GetLocalizedString(FocusUnit?.AttackType.ToString())}";
        [DataObservable]
        private string UnitRaceType => $"{Localization.GetLocalizedString((FocusUnit as Mob)?.RaceType.ToString())}";

        #region Stats

        [DataObservable]
        private string Hp => $"<B>{FocusUnit?.BaseMaxHp ?? 0:#,##0}</B><size=80%>({FocusUnit?.MaxHpAddedDisplayValue})</size>";
        [DataObservable]
        private string HpUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.Hp) ?? 0}";
        [DataObservable]
        private string NextGoldHpUpgrade => FocusUnit == null ? String.Empty : $"{D.SelfPlayer.GetHpNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
        [DataObservable]
        private bool IsEnoughGoldHpUpgrade => FocusUnit && D.SelfPlayer.Gold >= D.SelfPlayer.GetHpNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);

        [DataObservable]
        private string Damage => $"<B>{FocusUnit?.BaseDamage ?? 0:#,##0}</B><size=80%>({FocusUnit?.DamageAddedDisplayValue})</size>";
        [DataObservable]
        private string DamageUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.Damage) ?? 0}";
        [DataObservable]
        private string NextGoldDamageUpgrade => FocusUnit == null ? String.Empty : $"{D.SelfPlayer.GetDamageNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
        [DataObservable]
        private bool IsEnoughGoldDamageUpgrade => FocusUnit && D.SelfPlayer.Gold >= D.SelfPlayer.GetDamageNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);

        [DataObservable]
        private string Heal => $"<B>{FocusUnit?.BaseHeal ?? 0:#,##0}</B><size=80%>({FocusUnit?.HealAddedDisplayValue})</size>";
        [DataObservable]
        private string HealUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.Heal) ?? 0}";
        [DataObservable]
        private string NextGoldHealUpgrade => FocusUnit == null ? String.Empty : $"{D.SelfPlayer.GetHealNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
        [DataObservable]
        private bool IsEnoughGoldHealUpgrade => FocusUnit && D.SelfPlayer.Gold >= D.SelfPlayer.GetHealNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);

        [DataObservable]
        private string Defense => $"<B>{FocusUnit?.BaseDefense ?? 0:#,##0}</B><size=80%>({FocusUnit?.DefenseAddedDisplayValue})</size>";
        [DataObservable]
        private string DefenseUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.Defense) ?? 0}";
        [DataObservable]
        private string NextGoldDefenseUpgrade => FocusUnit == null ? String.Empty : $"{D.SelfPlayer.GetDefenseNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
        [DataObservable]
        private bool IsEnoughGoldDefenseUpgrade => FocusUnit && D.SelfPlayer.Gold >= D.SelfPlayer.GetDefenseNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);
        [DataObservable]
        private string DefenseMouseOver =>
            string.Format(Localization.GetLocalizedString("Unit/Defense/ReducePercentage"),
                FocusUnit?.ReductionByDefenseRate * 100f ?? 0);
        
        [DataObservable]
        private string CriticalProbability => $"<B>{FocusUnit?.BaseCriticalProbability ?? 0:#,##0}</B><size=80%>({FocusUnit?.CriticalProbabilityAddedDisplayValue})</size>";
        [DataObservable]
        private string CriticalProbabilityUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.CriticalProbability) ?? 0}";
        
        [DataObservable]
        private string CriticalDamage => $"<B>{FocusUnit?.BaseCriticalDamagePercentage ?? 0:#,##0}</B><size=80%>({FocusUnit?.CriticalDamagePercentageAddedDisplayValue})</size>";
        [DataObservable]
        private string CriticalDamageUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.CriticalDamagePercentage) ?? 0}";
        
        [DataObservable]
        private string Range => $"<B>{FocusUnit?.BaseRange ?? 0:#,##0}</B><size=80%>({FocusUnit?.RangeAddedDisplayValue})</size>";
        [DataObservable]
        private string RangeUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.Range) ?? 0}";
        
        [DataObservable]
        private string Drain => $"<B>{FocusUnit?.BaseDrainProbability ?? 0:#,##0}</B><size=80%>({FocusUnit?.DrainProbabilityAddedDisplayValue})</size>";
        [DataObservable]
        private string DrainUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.DrainProbability) ?? 0}";
        
        [DataObservable]
        private string Speed => $"<B>{FocusUnit?.BaseSpeed ?? 0:#,##0}</B><size=80%>({FocusUnit?.SpeedAddedDisplayValue})</size>";
        [DataObservable]
        private string SpeedUpgradeCount => $"+{FocusUnit?.GetUpgradeCount(StatType.Speed) ?? 0}";
        
        #endregion

        [DataObservable]
        private bool IsExistExtraSkill => FocusUnit?.SkillGroup.ExtraSkill != null;
        [DataObservable]
        private bool IsExistIndividualSkill => FocusUnit?.SkillGroup.PassiveSkill != null;
        
        [DataObservable]
        private string UnitPickUpCostOne => $"{D.SelfPlayer.UnitPickUpCost}";
        [DataObservable]
        private bool IsEnoughMoneyUnitPickUpOne => D.SelfPlayer.UnitPickUpCost <= D.SelfPlayer.Gold;
        [DataObservable]
        private string UnitPickUpCostTen => $"{D.SelfPlayer.UnitPickUpCost * 10}";
        [DataObservable]
        private bool IsEnoughMoneyUnitPickUpTen => D.SelfPlayer.UnitPickUpCost * 10 <= D.SelfPlayer.Gold;

        private bool isStatPanelActive = true;
        [DataObservable]
        private bool IsStatPanelActive
        {
            get => isStatPanelActive;
            set
            {
                isStatPanelActive = value;
                this.NotifyObserver("IsStatPanelActive");
            }
        }
        
        private void Start()
        {
            FocusUnit = D.SelfPlayer.Mobs?[0];
            InitTabUnits();
            
            D.SelfPlayer.onAddUnit += OnAddUnit;
            D.SelfPlayer.onClearedUnit += NotifyObserverMine;
        }

        private void OnDestroy()
        {
            D.SelfPlayer.onAddUnit -= OnAddUnit;
            D.SelfPlayer.onClearedUnit -= NotifyObserverMine;
        }

        private void OnEnable()
        {
            this.NotifyObserver();
        }

        private void OnAddUnit(Unit unit)
        {
            InitTabUnits();
            this.NotifyObserver();
        }
        
        private void OnRemoveUnit(Unit unit)
        {
            InitTabUnits();
            this.NotifyObserver();
        }
        
        public void NotifyObserverMine()
        {
            InitTabUnits();
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

            FocusUnit = unitTabItems[0].Unit;
            
            unitTabItems.ForEach(item => item.NotifyObserver());
        }
        
        private void InitSkills()
        {
            InitBasicSkills();
            InitExtraSkill();
            InitIndividualSkill();
        }

        private void InitBasicSkills()
        {
            var skills = FocusUnit.SkillGroup.GetAllSkills();

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

        private void InitExtraSkill()
        {
            var extraSkill = FocusUnit.SkillGroup.ExtraSkill;

            if (extraSkill == null)
            {
                return;
            }
            
            extraSkillInfoPopup.Init(extraSkill);
        }

        private void InitIndividualSkill()
        {
            var individualSkill = FocusUnit.SkillGroup.PassiveSkill;

            if (individualSkill == null)
            {
                return;
            }
            
            individualSkillInfoPopup.Init(individualSkill);
        }
        
        public void ChangeToggleUnit(int index)
        {
            FocusUnit = D.SelfPlayer.Mobs?[index];
        }

        public void ClickUnLockHeroUnit()
        {
            Debug.Log("$DlgProfile.ClickUnLockHeroUnit()");
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgProfile/Unit/BuyHeroSlot/Title");
                dialog.Content = $"{Localization.GetLocalizedString("DlgProfile/Unit/BuyHeroSlot/Content")}{D.SelfPlayer.NextUnitUnLockCost}";
                dialog.AddOKEvent(() => 
                {
                    if (D.SelfPlayer.IsEnoughNextUnitUnLockCost)
                    {
                        D.SelfPlayer.CreateHeroUnit();
                        this.NotifyObserver();
                    }
                    else
                    {
                        DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.NextUnitUnLockCost}"; });
                    }
                });
            });
        }

        public void ClickUnitPickUpOne()
        {
            Debug.Log("$DlgProfile.ClickUnitPickUpOne()");
            
            if (IsEnoughMoneyUnitPickUpOne)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.UnitPickUpCost;
                DialogManager.Instance.OpenDialog<DlgPickUp>("DlgPickUp", (dlg) =>
                {
                    dlg.OpenPanel("Pick");
                    dlg.PickUpUnits(1);
                    this.NotifyObserver();
                });
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.UnitPickUpCost}"; });
            }
        }
        
        public void ClickUnitPickUpTen()
        {
            Debug.Log("$DlgProfile.ClickUnitPickUpTen()");
            
            if (IsEnoughMoneyUnitPickUpTen)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.UnitPickUpCost * 10;
                
                DialogManager.Instance.OpenDialog<DlgPickUp>("DlgPickUp", (dlg) =>
                {
                    dlg.OpenPanel("Pick");
                    dlg.PickUpUnits(10);
                    this.NotifyObserver();
                });
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.UnitPickUpCost * 10}"; });
            }
        }
        
        public void ClickUnitHpUpgrade()
        {
            Debug.Log("DlgProfile.ClickUnitHpUpgrade()");
            
            if (IsEnoughGoldHpUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetHpNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusUnit.UnitType, StatType.Hp, FocusUnit.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetHpNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
                    });
            }
        }
        
        public void ClickUnitDamageUpgrade()
        {
            Debug.Log("DlgProfile.ClickUnitDamageUpgrade()");
            
            if (IsEnoughGoldDamageUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetDamageNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusUnit.UnitType, StatType.Damage, FocusUnit.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetDamageNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
                    });
            }
        }

        public void ClickUnitHealUpgrade()
        {
            Debug.Log("DlgProfile.ClickUnitHealUpgrade()");
            
            if (IsEnoughGoldHealUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetHealNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusUnit.UnitType, StatType.Heal, FocusUnit.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetHealNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
                    });
            }
        }
        
        public void ClickUnitDefenseUpgrade()
        {
            Debug.Log("DlgProfile.ClickUnitDefenseUpgrade()");
            
            if (IsEnoughGoldDefenseUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetDefenseNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusUnit.UnitType, StatType.Defense, FocusUnit.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetDefenseNextUpgradeCost(FocusUnit.UnitType, FocusUnit.AttackType)}";
                    });
            }
        }

        public void ChangePage()
        {
            IsStatPanelActive = !IsStatPanelActive;
        }

        private void ReplayAnimation()
        {
            starEffect.gameObject.SetActive(false);
            starEffect.gameObject.SetActive(true);
        }
    }
}