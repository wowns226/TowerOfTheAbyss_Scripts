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
    public class RelicBag : Bag<Relic>
    {
        protected override List<Type> Types => RelicManager.Instance.RelicTypes;
        
        public void Activate(Relic relic, GradeType gradeType)
        {
            Debug.Log($"RelicBag.Activate(), Relic : {relic.name}, GradeType : {gradeType}");
            if (relic.IsActive)
            {
                int sellGold = relic.GetSellGold();
                player.Gold += sellGold;
                DeActivate(relic);
                
                Debug.Log($"RelicBag.Activate() SellRelic, Relic : {relic.name}, GradeType : {sellGold}");
            }
            
            relic.Activate(gradeType);
        }

        public void DeActivate(Relic relic)
        {
            relic.InActivate();
        }

        public void ClearActivate()
        {
            FilterList.ForEach(relic => relic.InActivate());
        }

        public void Sell(Relic relic)
        {
            Debug.Log($"RelicBag.Sell(), Relic : {relic.name}");
            
            if (relic.IsActive == false)
            {
                Debug.Log($"RelicBag.Sell() Failed IsActive == false, Relic : {relic.name}");
                return;
            }

            var gold = relic.GetSellGold();
            player.Gold += gold;
            DeActivate(relic);
        }
        
        protected override bool CheckCondition(Relic relic) => relic.IsActive;

        // 유물 개발이 모두 완료가 되면 Where 삭제
        public List<Relic> GetRandomRelics(int count) => pairClass.List
            .Where(relic=>relic.CompletedDev == true).OrderBy(_ => Guid.NewGuid()).Take(count).ToList();
    }
}
