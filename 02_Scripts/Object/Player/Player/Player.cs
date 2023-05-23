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
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public enum RaceType
    {
        None,
        Elf,
        Human,
        Orc,
    }

    public class Player : MonoBehaviour, ISetting
    {
        [HideInInspector]
        public List<Unit> units = new List<Unit>();
        public List<Unit> SpawnUnits => units.FindAll(unit => unit.IsSpawn);
        public List<Unit> Mobs => units.FindAll(unit => unit.UnitType == UnitType.Mob);
        public List<Unit> SpawnMobs => units.FindAll(unit => unit.UnitType == UnitType.Mob && unit.IsSpawn);
        public List<Unit> Buildings => units.FindAll(unit => unit.UnitType == UnitType.Building);
        public Castle Castle { get; set; }
        [HideInInspector]
        public List<Drone> drones = new List<Drone>();
        public List<RepairDrone> repairDrones = new List<RepairDrone>();
        
        private RepairDroneState repairDroneState;
        public RepairDroneState RepairDroneState
        {
            get => repairDroneState;
            set
            {
                repairDroneState = value;
                repairDrones.ForEach(drone => drone.CurrentState = value);
            }
        }

        public bool isDroneModeChangeDevelopmentCompleted;
        public bool IsDroneModeChangeDevelopmentCompleted
        {
            get => isDroneModeChangeDevelopmentCompleted;
            set
            {
                isDroneModeChangeDevelopmentCompleted = value;
                onDroneModeDevelop.Invoke(value);
            }
        }
        public CustomAction<bool> onDroneModeDevelop = new CustomAction<bool>();

        private Dictionary<(UnitType, StatType, AttackType), int> statUpgradeLevels = new Dictionary<(UnitType, StatType, AttackType), int>();
        private Dictionary<(UnitType, StatType, AttackType), int> statUpgradeLevelsByGold = new Dictionary<(UnitType, StatType, AttackType), int>();
        private Dictionary<(UnitType, StatType, AttackType), float> statLevels = new Dictionary<(UnitType, StatType, AttackType), float>();
        private Dictionary<(UnitType, StatType, AttackType), float> statPercentageLevels = new Dictionary<(UnitType, StatType, AttackType), float>();

        public float SkillCooldownReduceRate { get; set; }
        
        public CustomAction<Mob> onSharedAttackMob = new CustomAction<Mob>();
        public CustomAction<Mob> onSharedHitMob = new CustomAction<Mob>();
        public CustomAction<Mob> onSharedHealMob = new CustomAction<Mob>();
        public CustomAction<Mob> onSharedCriticalMob = new CustomAction<Mob>();
        public CustomAction<Mob> onSharedDrainHpMob = new CustomAction<Mob>();
        public CustomAction<Mob> onSharedPreDeathMob = new CustomAction<Mob>();
        public CustomAction<Mob> onSharedPostDeathMob = new CustomAction<Mob>();

        public Action<Unit> onAddUnit;
        public Action<Unit> onRemoveUnit;
        public Action onClearedUnit;

        public Action<Drone> onAddDrone;
        public Action<Drone> onRemoveDrone;
        public Action onClearedDrone;

        private ulong userID;
        public ulong UserID => userID;

        private PlayerType playerType;
        public PlayerType PlayerType => playerType;

        private TechnologyBag technologyBag;
        public TechnologyBag TechnologyBag => technologyBag;
        
        private RelicSetBag relicSetBag;
        public RelicSetBag RelicSetBag => relicSetBag;
        
        private RelicBag relicBag;
        public RelicBag RelicBag => relicBag;

        [SettingValue]
        private int gold;
        public int Gold 
        { 
            get => gold;
            set 
            {
                float changedValue = value - gold;

                gold = value; 
                
                if(changedValue > 0)
                {
                    onGoldUp?.Invoke(changedValue);
                }

                if (changedValue < 0)
                {
                    onGoldDown?.Invoke(changedValue);
                }

                onGoldChanged?.Invoke(value);
            } 
        }

        public Action<float> onGoldChanged;
        public Action<float> onGoldUp;
        public Action<float> onGoldDown;

        [SettingValue]
        protected int unitHpUpgradeCostMultiplier;
        [SettingValue]
        protected int unitDamageUpgradeCostMultiplier;
        [SettingValue]
        protected int unitHealUpgradeCostMultiplier;
        [SettingValue]
        protected int unitDefenseUpgradeCostMultiplier;
        
        [SettingValue]
        protected int unitPickUpCost;
        public int UnitPickUpCost => unitPickUpCost;
        
        [SettingValue]
        protected int unitUnlockCostMultiplier;
        public int NextUnitUnLockCost => Mobs.Count * unitUnlockCostMultiplier;
        public bool IsEnoughNextUnitUnLockCost => Gold >= NextUnitUnLockCost;

        [SettingValue]
        protected int droneCreateCost;
        public int DroneCreateCost => droneCreateCost;
        
        public int MaxUnitCount { get; set; } = 5;
        public int MaxBuildingCount { get; set; } = 3;
        public int CreatedDroneCount { get; set; } = 0;
        public int MaxDroneCount { get; set; } = 0;
        public int MaxRelicCount { get; set; } = 6;

        private RaceType raceType;
        public RaceType RaceType { get => raceType; set => raceType = value; }

        public void Init(ulong userID, PlayerType playerType)
        {
            this.userID = userID;
            this.playerType = playerType;
            
            this.SetValue();
            
            ObjectPoolManager.Instance.New(nameof(ProjectL.TechnologyBag), transform, obj =>
            {
                technologyBag = obj.GetComponent<TechnologyBag>();
                technologyBag.Init(this);
            });
            
            ObjectPoolManager.Instance.New(nameof(ProjectL.RelicSetBag), transform, obj =>
            {
                relicSetBag = obj.GetComponent<RelicSetBag>();
                relicSetBag.Init(this);
                
                ObjectPoolManager.Instance.New(nameof(ProjectL.RelicBag), transform, obj =>
                {
                    relicBag = obj.GetComponent<RelicBag>();
                    relicBag.Init(this);
                });
            });
        }

        public void Clear()
        {
            SkillCooldownReduceRate = 0;
            ClearUnit();
            ClearDrone();
            ClearChangeStat();
            ClearStatPercentage();
            ClearUpgradeLevel();
        }
        
        public void CreateHeroUnit(Action<Mob> complete = null)
        {
            Gold -= NextUnitUnLockCost;

            var ownerType = playerType == PlayerType.Ally ? OwnerType.My : OwnerType.Enemy;
            var grade = GradeUtil.GetRandomGrade();
           
            var mobs = MobManager.Instance.GetRandomMobNames((RoundManager.Instance.Chapter, ownerType, grade, MobType.Hero), 1);

            Debug.Log($"Player.CreateHeroUnit(), Mob : {(mobs != null ? mobs?[0].type : string.Empty)}, grade : {grade}, Gold : {Gold}");

            //mobs.ForEach(mob => MobFactory.Instance.CreateMob(mob)); 테스트 코드로 인한 주석
            mobs.ForEach(_ => MobFactory.Instance.CreateMob((GradeType.Ancient, MobType.Hero, "AncientJeni"), this, complete));
        }

        public void AddUnit(Unit unit)
        {
            if (units.Contains(unit))
            {
                return;
            }
            
            units.Add(unit);
            units.Sort((item1, item2) =>
            {
                int result = item1.GradeType.CompareTo(item2.GradeType);

                if (result != 0)
                {
                    return result * -1;
                }

                return item1.GetHashCode().CompareTo(item1.GetHashCode());
            });
            
            onAddUnit?.Invoke(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            if (units.Contains(unit) == false)
            {
                return;
            }
            
            units.Remove(unit);
            onRemoveUnit?.Invoke(unit);
        }

        public void ClearUnit()
        {
            units.ToList().ForEach(unit => unit.Death());
            units.ToList().ForEach(unit => unit.Clear());
            units.Clear();
            onClearedUnit?.Invoke();
        }

        public void FullHealAllUnits() => units.ForEach(unit => unit.FullHealHp());
        public void FullHealAllMobs() => Mobs.ForEach(mob => mob.FullHealHp());
        public void FullHealAllBuildings() => Buildings.ForEach(building => building.FullHealHp());

        public void AddDrone(Drone drone)
        {
            var repairDrone = drone as RepairDrone;
            if (repairDrone != null)
            {
                repairDrones.Add(repairDrone);
            }
            
            drones.Add(drone);
            onAddDrone?.Invoke(drone);
        }

        public void RemoveDrone(Drone drone)
        {
            var repairDrone = drone as RepairDrone;
            if (repairDrone != null)
            {
                repairDrones.Remove(repairDrone);
            }
            
            drones.Remove(drone);
            onRemoveDrone?.Invoke(drone);
        }

        public void ClearDrone()
        {
            drones.ToList().ForEach(drone => drone.Death());
            repairDrones.Clear();
            drones.Clear();
            CreatedDroneCount = 0;
            onClearedDrone?.Invoke();
        }

        #region ChangeStat
        
        public float GetChangeStat(UnitType unitType, StatType upgradeType, AttackType attackType)
        {
            if(statLevels.ContainsKey((unitType, upgradeType, attackType)) == false)
            {
                statLevels.Add((unitType, upgradeType, attackType), 0);
            }

            return statLevels[(unitType, upgradeType, attackType)];
        }

        public void AllUpgradeStat(UnitType unitType, StatType statType, float value)
        {
            var attackTypes = Enum.GetValues(typeof(AttackType));

            foreach(AttackType attackType in attackTypes)
            {
                UpgradeStat(unitType, statType, attackType, value);
            }
        }
        public void UpgradeStat(UnitType unitType, StatType statType, AttackType attackType, float value) 
            => _UpgradeStat(unitType, statType, attackType, value);

        private void _UpgradeStat(UnitType unitType, StatType statType, AttackType attackType, float value)
        {
            if(statLevels.ContainsKey((unitType, statType, attackType)) == false)
            {
                statLevels.Add((unitType, statType, attackType), 0);
            }

            statLevels[(unitType, statType, attackType)] += value;

            _UpgradeStat(unitType, statType, attackType);
        }

        private void _UpgradeStat(UnitType unitType, StatType statType, AttackType attackType)
        {
            var changeUnits = units.FindAll(unit => unit.UnitType == unitType && unit.AttackType == attackType);

            foreach(var unit in changeUnits)
            {
                unit.ChangeStat(statType, statLevels[(unitType, statType, attackType)]);
            }
        }

        public void ClearChangeStat() => statLevels.Clear();
        
        #endregion

        #region StatPercentage
        
        public float GetStatPercentage(UnitType unitType, StatType upgradeType, AttackType attackType)
        {
            if(statPercentageLevels.ContainsKey((unitType, upgradeType, attackType)) == false)
            {
                statPercentageLevels.Add((unitType, upgradeType, attackType), 0);
            }

            return statPercentageLevels[(unitType, upgradeType, attackType)];
        }

        public void AllUpgradeStatPercentage(UnitType unitType, StatType statType, float value)
        {
            var attackTypes = Enum.GetValues(typeof(AttackType));

            foreach(AttackType attackType in attackTypes)
            {
                UpgradeStatPercentage(unitType, statType, attackType, value);
            }
        }
        public void UpgradeStatPercentage(UnitType unitType, StatType statType, AttackType attackType, float value) 
            => _UpgradeStatPercentage(unitType, statType, attackType, value);

        private void _UpgradeStatPercentage(UnitType unitType, StatType statType, AttackType attackType, float value)
        {
            if(statPercentageLevels.ContainsKey((unitType, statType, attackType)) == false)
            {
                statPercentageLevels.Add((unitType, statType, attackType), 0);
            }

            statPercentageLevels[(unitType, statType, attackType)] += value;

            _UpgradeUnitStatPercentage(unitType, statType, attackType);
        }

        private void _UpgradeUnitStatPercentage(UnitType unitType, StatType statType, AttackType attackType)
        {
            var upgradeUnits = units.FindAll(unit => unit.UnitType == unitType && unit.AttackType == attackType);

            foreach(var unit in upgradeUnits)
            {
                unit.ChangeStatPercentage(statType, statPercentageLevels[(unitType, statType, attackType)]);
            }
        }

        public void ClearStatPercentage() => statPercentageLevels.Clear();
        
        #endregion

        #region UpgradeGold

        public int GetHpNextUpgradeCost(UnitType unitType, AttackType attackType)
        {
            if (statUpgradeLevelsByGold.ContainsKey((unitType, StatType.Hp, attackType)) == false)
            {
                statUpgradeLevelsByGold.Add((unitType, StatType.Hp, attackType), 0);
            }

            return (statUpgradeLevelsByGold[(unitType, StatType.Hp, attackType)] + 1) * unitHpUpgradeCostMultiplier;
        }
        
        public int GetDamageNextUpgradeCost(UnitType unitType, AttackType attackType)
        {
            if (statUpgradeLevelsByGold.ContainsKey((unitType, StatType.Damage, attackType)) == false)
            {
                statUpgradeLevelsByGold.Add((unitType, StatType.Damage, attackType), 0);
            }

            return (statUpgradeLevelsByGold[(unitType, StatType.Damage, attackType)] + 1) * unitDamageUpgradeCostMultiplier;
        }
        
        public int GetHealNextUpgradeCost(UnitType unitType, AttackType attackType)
        {
            if (statUpgradeLevelsByGold.ContainsKey((unitType, StatType.Heal, attackType)) == false)
            {
                statUpgradeLevelsByGold.Add((unitType, StatType.Heal, attackType), 0);
            }

            return (statUpgradeLevelsByGold[(unitType, StatType.Heal, attackType)] + 1) * unitHealUpgradeCostMultiplier;
        }
        
        public int GetDefenseNextUpgradeCost(UnitType unitType, AttackType attackType)
        {
            if (statUpgradeLevelsByGold.ContainsKey((unitType, StatType.Defense, attackType)) == false)
            {
                statUpgradeLevelsByGold.Add((unitType, StatType.Defense, attackType), 0);
            }

            return (statUpgradeLevelsByGold[(unitType, StatType.Defense, attackType)] + 1) * unitDefenseUpgradeCostMultiplier;
        }
        
        public int GetUpgradeLevelByGold(UnitType unitType, StatType upgradeType, AttackType attackType)
        {
            if (statUpgradeLevelsByGold.ContainsKey((unitType, upgradeType, attackType)) == false)
            {
                statUpgradeLevelsByGold.Add((unitType, upgradeType, attackType), 0);
            }

            return statUpgradeLevelsByGold[(unitType, upgradeType, attackType)];
        }
        
        public void AllUpgradeByGold(UnitType unitType, StatType statType, int upgradeCount)
        {
            var attackTypes = Enum.GetValues(typeof(AttackType));

            foreach (AttackType attackType in attackTypes)
            {
                UpgradeByGold(unitType, statType, attackType, upgradeCount);
            }
        }
        
        public void UpgradeByGold(UnitType unitType, StatType statType, AttackType attackType, int upgradeCount)
        {
            statUpgradeLevelsByGold[(unitType, statType, attackType)] += upgradeCount;
            UpgradeLevel(unitType, statType, attackType, upgradeCount);
        }

        #endregion

        #region UpgradeByLogic

        public int GetUpgradeLevel(UnitType unitType, StatType upgradeType, AttackType attackType)
        {
            if (statUpgradeLevels.ContainsKey((unitType, upgradeType, attackType)) == false)
            {
                statUpgradeLevels.Add((unitType, upgradeType, attackType), 0);
            }

            return statUpgradeLevels[(unitType, upgradeType, attackType)];
        }

        public void AllUpgrade(UnitType unitType, StatType statType, int upgradeCount)
        {
            var attackTypes = Enum.GetValues(typeof(AttackType));

            foreach (AttackType attackType in attackTypes)
            {
                Upgrade(unitType, statType, attackType, upgradeCount);
            }
        }

        public void Upgrade(UnitType unitType, StatType statType, AttackType attackType, int upgradeCount) =>
            UpgradeLevel(unitType, statType, attackType, upgradeCount);

        private void UpgradeLevel(UnitType unitType, StatType statType, AttackType attackType, int upgradeCount)
        {
            if (statUpgradeLevels.ContainsKey((unitType, statType, attackType)) == false)
            {
                statUpgradeLevels.Add((unitType, statType, attackType), 0);
            }

            statUpgradeLevels[(unitType, statType, attackType)] += upgradeCount;

            UpgradeUnitsUpgradeValue(unitType, statType, attackType);
        }

        private void UpgradeUnitsUpgradeValue(UnitType unitType, StatType statType, AttackType attackType)
        {
            var upgradeUnits = units.FindAll(unit => unit.UnitType == unitType && unit.AttackType == attackType);

            foreach (var unit in upgradeUnits)
            {
                unit.ChangeUpgradeValue(statType, statUpgradeLevels[(unitType, statType, attackType)]);
            }
        }

        #endregion

        public void ClearUpgradeLevel()
        {
            statUpgradeLevelsByGold.Clear();
            statUpgradeLevels.Clear();
        }
        
        public void ChangeDroneState()
        {
            if (isDroneModeChangeDevelopmentCompleted == false)
            {
                Debug.Log($"Player.ChangeDroneState(), isDroneModeChangeDevelopmentCompleted is false");
                return;
            }
            
            Debug.Log($"Player.ChangeDroneState(), prevState : {RepairDroneState.ToString()}");

            int length = Enum.GetValues(typeof(RepairDroneState)).Length;
            var nextState = (RepairDroneState)(((int)RepairDroneState + 1) % length);

            RepairDroneState = nextState;
        }
    }
}