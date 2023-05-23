﻿// Copyright (C) <2023>  
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

namespace ProjectL
{
    public class MoneyReduceTimeTripod : Tripod
    {
        public override string Description => string.Format(Localization.GetLocalizedString(description), reducePercentageGoldCycleTime);
        
        //value -> 0 ~ 1
        [SettingValue]
        private float reducePercentageGoldCycleTime;
        
        public override void Activate()
        {
            ReducePercentageGoldCycleTime(reducePercentageGoldCycleTime);
        }

        public override void DeActivate()
        {
            ReducePercentageGoldCycleTime(reducePercentageGoldCycleTime * -1);
        }
        
        private void ReducePercentageGoldCycleTime(float value)
        {
            GoldController.Instance.ReducePercentageGoldCycleTime = value;
        }
    }
}
