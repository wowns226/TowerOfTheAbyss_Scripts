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
    public class Battle<T> : UnitState<T> where T : MonoBehaviour, IUnitState
    {
        public Battle(UnitState<T> nextState) : base(nextState) { }

        protected override bool CheckState(T unit)
        {
            if (unit.BattlePoint == null)
                return false;

            if (unit.IsSplineMove)
                return false;

            if (unit.NearestUnit.BasePoint.Depth != unit.BasePoint.Depth)
            {
                if (unit.AttackType == AttackType.Melee)
                {
                    return false;
                }

                if (unit.AttackType == AttackType.Ranged && unit.IsNearestUnitInAttackRange == false)
                {
                    return false;
                }
            }
            
            return true;
        }

        protected override void ExecuteState(T unit)
        {
            if(unit.IsNearestUnitInAttackRange)
                unit.Attack();
            else
                unit.MoveToNearestBattleUnit();
        }
        protected override void Rotate(T unit) => unit.RotateXZ(unit.NearestUnit.transform.position);
    }
}
