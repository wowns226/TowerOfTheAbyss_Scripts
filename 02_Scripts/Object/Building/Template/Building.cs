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
using System.Reflection;
using UnityEngine;

namespace ProjectL
{
    public abstract class Building : Unit
    {
        private BuildingType buildingType;
        public BuildingType BuildingType => buildingType;
        protected override List<Unit> BattleUnits => AttackType == AttackType.Heal ? BattlePoint.GetAllyMobs() : BattlePoint.GetEnemyMobs();

        protected override void Awake()
        {
            base.Awake();

            unitType = UnitType.Building;
            
            var buildingTypeAttribute = GetType().GetCustomAttribute(typeof(BuildingTypeAttribute)) as BuildingTypeAttribute;
            if (buildingTypeAttribute != null)
            {
                buildingType = buildingTypeAttribute.BuildingType;
                GradeType = buildingTypeAttribute.GradeType;
            }
        }

        protected override void Start()
        {
            ownerType = OwnerType.My;
            
            base.Start();
        }
        
        public override void InitState()
        {
            state = new Battle<Unit>(new Idle<Unit>());
        }
        
        public override void Spawn(Point spawnPoint)
        {
            Debug.Log($"{this.gameObject.name} override Spawn In Point {spawnPoint}");

            base.Spawn(spawnPoint);

            BasePoint.SetBuilding(this);
        }

        public override void Death()
        {
            BasePoint.UnSetBuilding(this);

            D.SelfPlayer.RemoveUnit(this);

            base.Death();
        }

        protected override void _PostDeath()
        {
            Clear();
        }

        protected override Func<Point, Point, List<Point>> GetPathsFunc() => D.SelfBoard.GetShortestPath;
        
        protected override Point SearchAllyPoint()
        {
            Point allyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return allyPoint;

            allyPoint = D.SelfBoard.FindAllyPoint(basePoint, Range, IsMelee, this);

            if (allyPoint != null)
                return allyPoint;

            return allyPoint;
        }

        protected override Point SearchNotFullHpAllyUnitPoint()
        {
            Point allyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return allyPoint;

            allyPoint = D.SelfBoard.FindNotFullHpAllyUnitPoint(basePoint, Range, IsMelee, this);

            if (allyPoint != null)
                return allyPoint;

            return allyPoint;
        }
        
        protected override Point SearchEnemyPoint()
        {
            Point enemyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return enemyPoint;

            enemyPoint = D.SelfBoard.FindEnemyPoint(basePoint, Range, IsMelee, this);

            if (enemyPoint != null)
                return enemyPoint;

            return enemyPoint;
        }

        public override void MoveToNearestBattleUnit() { }

    }
}
