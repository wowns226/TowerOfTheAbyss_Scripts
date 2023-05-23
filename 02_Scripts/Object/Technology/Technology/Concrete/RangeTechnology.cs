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

namespace ProjectL
{
    public class RangeTechnology : Technology
    {
        public override string FullDescription => string.Format(Localization.GetLocalizedString(fullDescription), increaseRange);
        
        [SettingValue]
        private int increaseRange;

        protected override void InitTripods()
        {
            firstTripods.Add(new RangeMoreUpTripod());
            firstTripods.Add(new RangePercentageUpTripod());
        }

        protected override void Active()
        {
            IncreaseRange(increaseRange);
        }

        protected override void DeActive()
        {
            IncreaseRange(increaseRange * -1);
        }

        private void IncreaseRange(int value)
        {
            D.SelfPlayer.AllUpgrade(UnitType.Mob, StatType.Range, value);
        }
    }
}
