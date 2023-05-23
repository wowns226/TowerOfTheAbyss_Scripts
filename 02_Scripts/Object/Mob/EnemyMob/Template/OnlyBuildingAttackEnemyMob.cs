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

namespace ProjectL
{
    public abstract class OnlyBuildingAttackEnemyMob : EnemyMob
    {
        protected override List<Unit> BattleUnits => AttackType == AttackType.Heal ? BattlePoint.GetAllyMobs() : BattlePoint.GetBuildings();
        protected override void ChangeTargetToAttacker(Point attackerPoint)
        {
            if (ReferenceEquals(attackerPoint, null))
            {
                Debug.Log($"OnlyBuildingAttackEnemyMob.ChangeTargetToAttacker(), attackerPoint is null, Unit : {name}");
                return;
            }
            
            var destinationPoint = DestinationPoint;
            if (destinationPoint is null || BasePoint != attackerPoint && BasePoint.distanceDic[attackerPoint] < BasePoint.distanceDic[destinationPoint])
            {
                if(attackerPoint.GetBuildings().Count > 0)
                {
                    Debug.Log($"OnlyBuildingAttackEnemyMob.ChangeTargetToAttacker(), attackerPoint is changed, Unit : {name}, beforePoint : {TargetPoint.name}, changePoint : {attackerPoint.name}");
                    SetDestinationPoint(attackerPoint);
                }
            }
        }

        protected override Point SearchEnemyPoint()
        {
            Point enemyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return enemyPoint;

            enemyPoint = D.SelfBoard.FindBuilding(basePoint, Range, IsMelee, this);
            if (enemyPoint != null)
                return enemyPoint;

            return enemyPoint;
        }
    }
}
