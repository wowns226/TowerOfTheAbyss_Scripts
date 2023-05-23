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
    public abstract class EnemyMob : Mob
    {
        protected override List<Unit> BattleUnits => AttackType == AttackType.Heal ? BattlePoint?.GetEnemyMobs() : BattlePoint?.GetAllys();
                                                     
        protected override void Start()
        {
            ownerType = OwnerType.Enemy;
            
            base.Start();
        }
        
        public override void InitState()
        {
            state = new Battle<Unit>(new Move<Unit>(new CastleAttack<Unit>(new Idle<Unit>())));
        }

        public override void Spawn(Point spawnPoint)
        {
            base.Spawn(spawnPoint);

            BasePoint.SpawnEnemyMob(this);
        }

        public override void Death()
        {
            BasePoint?.UnSetEnemyMob(this);

            D.SelfPlayer.Gold += DropGold;
            D.SelfEnemyPlayer.RemoveUnit(this);

            base.Death();
        }

        protected override void ChangeTargetToAttacker(Point attackerPoint)
        {
            if (ReferenceEquals(attackerPoint, null))
            {
                Debug.Log($"EnemyMob.ChangeTargetToAttacker(), attackerPoint is null, Unit : {name}");
                return;
            }

            var destinationPoint = DestinationPoint;
            if (destinationPoint is null || BasePoint != attackerPoint && BasePoint.distanceDic[attackerPoint] < BasePoint.distanceDic[destinationPoint])
            {
                Debug.Log($"EnemyMob.ChangeTargetToAttacker(), attackerPoint is changed, Unit : {name}, beforePoint : {destinationPoint?.name}, changePoint : {attackerPoint.name}");
                SetDestinationPoint(attackerPoint);
            }
        }

        protected override Func<Point, Point, List<Point>> GetPathsFunc() => D.SelfBoard.GetRandomShortestPath;

        protected override Point SearchAllyPoint()
        {
            Point allyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return allyPoint;

            allyPoint = D.SelfBoard.FindEnemyPoint(basePoint, Range, IsMelee, this);

            if (allyPoint != null)
                return allyPoint;

            return allyPoint;
        }

        protected override Point SearchNotFullHpAllyUnitPoint()
        {
            Point allyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return allyPoint;

            allyPoint = D.SelfBoard.FindNotFullHpEnemyUnitPoint(basePoint, Range, IsMelee, this);

            if (allyPoint != null)
                return allyPoint;

            return allyPoint;
        }
        
        protected override Point SearchEnemyPoint()
        {
            Point enemyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return enemyPoint;

            enemyPoint = D.SelfBoard.FindAllyPoint(basePoint, Range, IsMelee, this);
            if (enemyPoint != null)
                return enemyPoint;

            enemyPoint = D.SelfBoard.FindBuilding(basePoint, Range, IsMelee, this);
            if (enemyPoint != null)
                return enemyPoint;

            return enemyPoint;
        }

    }
}
