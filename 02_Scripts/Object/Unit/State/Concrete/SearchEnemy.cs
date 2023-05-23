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

using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public class SearchEnemy<T> : UnitState<T> where T : MonoBehaviour, IUnitState
    {
        public SearchEnemy(UnitState<T> nextState) : base(nextState) { }

        protected override bool CheckState(T unit)
        {
            return D.SelfEnemyPlayer.SpawnMobs.Count != 0;
        }

        /// <summary>
        /// 성에서 가까운 적이 1순위
        /// </summary>
        /// <param name="unit"></param>
        protected override void ExecuteState(T unit)
        {
            var target = (from mob in D.SelfEnemyPlayer.SpawnMobs
                          let distance = (mob.transform.position - D.SelfPlayer.Castle.transform.position).sqrMagnitude
                          orderby distance
                          select mob).First();

            unit.TargetPoint = target.BasePoint;
        }
    }
}