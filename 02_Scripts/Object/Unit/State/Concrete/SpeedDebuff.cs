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
    public class SpeedDebuff<T> : UnitState<T> where T : MonoBehaviour, IUnitState
    {
        public SpeedDebuff(UnitState<T> nextState) : base(nextState) { }

        private float durationTime = 1f;
        private float EndTime => Time.time + durationTime;

        private float speedDownPercentage = 20f;
        private float damageDownPercentage = 10f;
        private float defenseDownPercentage = 10f;

        protected override bool CheckState(T unit)
        {
            if (unit?.BasePoint?.GetEnemyMobs().Count == 0)
            {
                return false;
            }

            return true;
        }

        protected override void ExecuteState(T unit)
        {
            var targetUnits = unit.BasePoint.GetEnemyMobs();

            targetUnits.ForEach(unit => unit.AddBuffSkill(GetType().Name, StartDebuffSkill, EndDebuffSkill, EndTime));
        }

        private void StartDebuffSkill(Unit unit)
        {
            unit.UpgradeStatPercentage(StatType.Speed, speedDownPercentage * -1);
            unit.UpgradeStatPercentage(StatType.Damage, damageDownPercentage* -1);
            unit.UpgradeStatPercentage(StatType.Defense, defenseDownPercentage* -1);
        }

        private void EndDebuffSkill(Unit unit)
        {
            unit.UpgradeStatPercentage(StatType.Speed, speedDownPercentage);
            unit.UpgradeStatPercentage(StatType.Damage, damageDownPercentage);
            unit.UpgradeStatPercentage(StatType.Defense, defenseDownPercentage);
        }
    }
}
