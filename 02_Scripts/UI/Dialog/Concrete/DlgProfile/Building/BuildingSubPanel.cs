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
    public class BuildingSubPanel : DataContainer
    {
        [SerializeField]
        private List<SkillInfoPopup> skillInfoPopups = new List<SkillInfoPopup>();
        [SerializeField]
        private List<BuildingItemInfo> buildingTabItems = new List<BuildingItemInfo>();
        [SerializeField]
        private GameObject starEffect;
        
        private Building focusBuilding;
        private Building FocusBuilding
        {
            get => focusBuilding;
            set
            {
                focusBuilding = value;
                InitSkills();
                ReplayAnimation();
                this.NotifyObserver();
            }
        }
        
        [DataObservable]
        private int BuildingCount => D.SelfPlayer.Buildings.Count;
        
        [DataObservable]
        private bool IsBuildingCountOverOne => BuildingCount >= 1;
        [DataObservable]
        private bool IsBuildingCountOverTwo => BuildingCount >= 2;
        [DataObservable]
        private bool IsBuildingCountOverThree => BuildingCount >= 3;
        [DataObservable]
        private bool IsBuildingCountOverFour => BuildingCount >= 4;
        [DataObservable]
        private bool IsBuildingCountOverFive => BuildingCount >= 5;
        [DataObservable]
        private bool IsBuildingCountOverSix => BuildingCount >= 6;
        [DataObservable]
        private bool IsBuildingCountOverSeven => BuildingCount >= 7;
        [DataObservable]
        private bool IsBuildingCountOverEight => BuildingCount >= 8;
        [DataObservable]
        private bool IsBuildingCountOverNine => BuildingCount >= 9;
        [DataObservable]
        private bool IsBuildingCountOverTen => BuildingCount >= 10;

        [DataObservable]
        private string BuildingName => $"{FocusBuilding?.DisplayName}";
        [DataObservable]
        private Sprite BuildingIcon => FocusBuilding?.Icon;
        [DataObservable]
        private Sprite BuildingMiniImage => FocusBuilding?.UnitMiniImage;
        [DataObservable]
        private Sprite BuildingImage => FocusBuilding?.UnitImage;
        
        [DataObservable]
        private bool IsNotLessThanCommon => FocusBuilding && FocusBuilding.GradeType >= GradeType.Common;
        [DataObservable]
        private bool IsNotLessThanRare => FocusBuilding && FocusBuilding.GradeType >= GradeType.Rare;
        [DataObservable]
        private bool IsNotLessThanUnique => FocusBuilding && FocusBuilding.GradeType >= GradeType.Unique;
        [DataObservable]
        private bool IsNotLessThanEpic => FocusBuilding && FocusBuilding.GradeType >= GradeType.Epic;
        [DataObservable]
        private bool IsNotLessThanSpecial => FocusBuilding && FocusBuilding.GradeType >= GradeType.Special;
        [DataObservable]
        private bool IsNotLessThanLegendary => FocusBuilding && FocusBuilding.GradeType >= GradeType.Legendary;
        [DataObservable]
        private bool IsNotLessThanAncient => FocusBuilding && FocusBuilding.GradeType >= GradeType.Ancient;
        
        [DataObservable]
        private bool IsAttack => FocusBuilding && FocusBuilding.BuildingType == BuildingType.Attack;
        [DataObservable]
        private bool IsDefense => FocusBuilding && FocusBuilding.BuildingType == BuildingType.Defense;
        [DataObservable]
        private bool IsHeal => FocusBuilding && FocusBuilding.BuildingType == BuildingType.Heal;
        [DataObservable]
        private bool IsCastle => FocusBuilding && FocusBuilding == D.SelfPlayer.Castle;

        #region Stats

        [DataObservable]
        private string Hp => $"<B>{FocusBuilding?.BaseMaxHp ?? 0:#,##0}</B><size=80%>({FocusBuilding?.MaxHpAddedDisplayValue})</size>";
        [DataObservable]
        private string HpUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.Hp) ?? 0}";
        [DataObservable]
        private string NextGoldHpUpgrade => $"{(FocusBuilding ? D.SelfPlayer.GetHpNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType) : "0")}";
        [DataObservable]
        private bool IsEnoughGoldHpUpgrade => FocusBuilding && D.SelfPlayer.Gold >= D.SelfPlayer.GetHpNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);

        [DataObservable]
        private string Damage => $"<B>{FocusBuilding?.BaseDamage ?? 0:#,##0}</B><size=80%>({FocusBuilding?.DamageAddedDisplayValue})</size>";
        [DataObservable]
        private string DamageUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.Damage) ?? 0}";
        [DataObservable]
        private string NextGoldDamageUpgrade => $"{(FocusBuilding ? D.SelfPlayer.GetDamageNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType) : "0")}";
        [DataObservable]
        private bool IsEnoughGoldDamageUpgrade => FocusBuilding && D.SelfPlayer.Gold >= D.SelfPlayer.GetDamageNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);

        [DataObservable]
        private string Heal => $"<B>{FocusBuilding?.BaseHeal ?? 0:#,##0}</B><size=80%>({FocusBuilding?.HealAddedDisplayValue})</size>";
        [DataObservable]
        private string HealUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.Heal) ?? 0}";
        [DataObservable]
        private string NextGoldHealUpgrade => $"{(FocusBuilding ? D.SelfPlayer.GetHealNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType) : "0")}";
        [DataObservable]
        private bool IsEnoughGoldHealUpgrade => FocusBuilding && D.SelfPlayer.Gold >= D.SelfPlayer.GetHealNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);

        [DataObservable]
        private string Defense => $"<B>{FocusBuilding?.BaseDefense ?? 0:#,##0}</B><size=80%>({FocusBuilding?.DefenseAddedDisplayValue})</size>";
        [DataObservable]
        private string DefenseUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.Defense) ?? 0}";
        [DataObservable]
        private string NextGoldDefenseUpgrade => $"{(FocusBuilding ?D.SelfPlayer.GetDefenseNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType) : 0)}";
        [DataObservable]
        private bool IsEnoughGoldDefenseUpgrade => FocusBuilding && D.SelfPlayer.Gold >= D.SelfPlayer.GetDefenseNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);
        [DataObservable]
        private string DefenseMouseOver =>
            string.Format(Localization.GetLocalizedString("Unit/Defense/ReducePercentage"),
                FocusBuilding?.ReductionByDefenseRate * 100f ?? 0);
        
        [DataObservable]
        private string CriticalProbability => $"<B>{FocusBuilding?.BaseCriticalProbability ?? 0:#,##0}</B><size=80%>({FocusBuilding?.CriticalProbabilityAddedDisplayValue})</size>";
        [DataObservable]
        private string CriticalProbabilityUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.CriticalProbability) ?? 0}";
        
        [DataObservable]
        private string CriticalDamage => $"<B>{FocusBuilding?.BaseCriticalDamagePercentage ?? 0:#,##0}</B><size=80%>({FocusBuilding?.CriticalDamagePercentageAddedDisplayValue})</size>";
        [DataObservable]
        private string CriticalDamageUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.CriticalDamagePercentage) ?? 0}";
        
        [DataObservable]
        private string Range => $"<B>{FocusBuilding?.BaseRange ?? 0:#,##0}</B><size=80%>({FocusBuilding?.RangeAddedDisplayValue})</size>";
        [DataObservable]
        private string RangeUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.Range) ?? 0}";
        
        [DataObservable]
        private string Drain => $"<B>{FocusBuilding?.BaseDrainProbability ?? 0:#,##0}</B><size=80%>({FocusBuilding?.DrainProbabilityAddedDisplayValue})</size>";
        [DataObservable]
        private string DrainUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.DrainProbability) ?? 0}";
        
        [DataObservable]
        private string Speed => $"<B>{FocusBuilding?.BaseSpeed ?? 0:#,##0}</B><size=80%>({FocusBuilding?.SpeedAddedDisplayValue})</size>";
        [DataObservable]
        private string SpeedUpgradeCount => $"+{FocusBuilding?.GetUpgradeCount(StatType.Speed) ?? 0}";
        
        #endregion
        
        [DataObservable]
        private string DroneCreateCost => $"{D.SelfPlayer.DroneCreateCost}";
        [DataObservable]
        private bool CanCreateDrone =>
            IsEnoughMoneyDroneCreateCost
            && D.SelfPlayer.MaxDroneCount > D.SelfPlayer.CreatedDroneCount;
        private bool IsEnoughMoneyDroneCreateCost => D.SelfPlayer.DroneCreateCost <= D.SelfPlayer.Gold; 

        [DataObservable]
        private string DisplayDroneCount
        {
            get
            {
                if(D.SelfPlayer.MaxDroneCount <= D.SelfPlayer.CreatedDroneCount)
                {
                    return $"<color=red>{string.Format(Localization.GetLocalizedString("DlgProfile/Building/OverMaxCount"), D.SelfPlayer.MaxDroneCount)}</color>";
                }
                
                return string.Format(Localization.GetLocalizedString("DlgBuilding/Drone/Create/MouseText"),
                    D.SelfPlayer.CreatedDroneCount, D.SelfPlayer.MaxDroneCount);
            }
        }
        
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
            FocusBuilding = D.SelfPlayer.Castle;
            InitTabUnits();
            
            D.SelfPlayer.onAddUnit += NotifyObserver;
            D.SelfPlayer.onRemoveUnit += NotifyObserver;
            D.SelfPlayer.onClearedUnit += NotifyObserverMine;
        }

        private void OnEnable()
        {
            this.NotifyObserver();
        }

        private void OnDestroy()
        {
            D.SelfPlayer.onAddUnit -= NotifyObserver;
            D.SelfPlayer.onRemoveUnit -= NotifyObserver;
            D.SelfPlayer.onClearedUnit -= NotifyObserverMine;
        }

        private void NotifyObserver(Unit unit)
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
            for (int i = 0; i < buildingTabItems.Count; i++)
            {
                if (D.SelfPlayer.Buildings?.Count <= i)
                {
                    break;
                }
                
                buildingTabItems[i].Building = D.SelfPlayer.Buildings?[i] as Building;
            }
            
            FocusBuilding = buildingTabItems[0].Building;
        }
        
        private void InitSkills()
        {
            InitBasicSkills();
        }

        private void InitBasicSkills()
        {
            var skills = FocusBuilding.SkillGroup.GetAllSkills();

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

        public void ChangeToggleUnit(int index)
        {
            FocusBuilding = D.SelfPlayer.Buildings[index] as Building;
        }

        public void ClickDroneCreate()
        {
            Debug.Log("$DlgProfile.ClickDroneCreate()");
            
            if (CanCreateDrone)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.DroneCreateCost;
                D.SelfPlayer.CreatedDroneCount += 1;
                DroneFactory.Instance.CreateDrone("RepairDrone");
                this.NotifyObserver();
            }
            else
            {
                if (IsEnoughMoneyDroneCreateCost == false)
                {
                    DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                        dialog =>
                        {
                            dialog.Text =
                                $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.DroneCreateCost}";
                        });
                }
            }
        }
        
        public void ClickBuildingHpUpgrade()
        {
            Debug.Log("DlgProfile.ClickBuildingHpUpgrade()");
            
            if (IsEnoughGoldHpUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetHpNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusBuilding.UnitType, StatType.Hp, FocusBuilding.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetHpNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType)}";
                    });
            }
        }
        
        public void ClickBuildingDamageUpgrade()
        {
            Debug.Log("DlgProfile.ClickBuildingDamageUpgrade()");
            
            if (IsEnoughGoldDamageUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetDamageNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusBuilding.UnitType, StatType.Damage, FocusBuilding.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetDamageNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType)}";
                    });
            }
        }

        public void ClickBuildingHealUpgrade()
        {
            Debug.Log("DlgProfile.ClickBuildingHealUpgrade()");
            
            if (IsEnoughGoldHealUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetHealNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusBuilding.UnitType, StatType.Heal, FocusBuilding.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetHealNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType)}";
                    });
            }
        }
        
        public void ClickBuildingDefenseUpgrade()
        {
            Debug.Log("DlgProfile.ClickBuildingDefenseUpgrade()");
            
            if (IsEnoughGoldDefenseUpgrade)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.GetDefenseNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType);
                D.SelfPlayer.UpgradeByGold(FocusBuilding.UnitType, StatType.Defense, FocusBuilding.AttackType, 1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip",
                    dialog =>
                    {
                        dialog.Text =
                            $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.GetDefenseNextUpgradeCost(FocusBuilding.UnitType, FocusBuilding.AttackType)}";
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