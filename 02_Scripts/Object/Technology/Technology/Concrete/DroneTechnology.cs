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
    public class DroneTechnology : Technology
    {
        public override string FullDescription => string.Format(Localization.GetLocalizedString(fullDescription), increaseMaxCount);
        
        [SettingValue]
        private int increaseMaxCount;

        protected override void InitTripods()
        {
            firstTripods.Add(new DroneCountIncreaseTripod());
            secondTripods.Add(new DroneModeChangeTripod());
        }

        protected override void Active()
        {
            IncreaseDroneCount(increaseMaxCount);
        }

        protected override void DeActive()
        {
            IncreaseDroneCount(increaseMaxCount * -1);
        }

        private void IncreaseDroneCount(int value)
        {
            D.SelfPlayer.MaxDroneCount += value;
        }
    }
}
