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
    public class HitTechnology : Technology
    {
        public override string FullDescription => string.Format(Localization.GetLocalizedString(fullDescription), healRateByMaxHp);
        
        [SettingValue]
        private float healRateByMaxHp;

        protected override void InitTripods()
        {
            firstTripods.Add(new HitUnitDamageUpTripod());
            firstTripods.Add(new HitUnitDefenseUpTripod());
        }

        protected override void Active()
        {
            D.SelfPlayer.onSharedHitMob.Add(HealHp);
        }

        protected override void DeActive()
        {
            D.SelfPlayer.onSharedHitMob.Remove(HealHp);
        }

        private void HealHp(Mob mob)
        {
            if(Random.Range(0,100f) < 10f)
            {
                mob.HealHp(mob.MaxHp * healRateByMaxHp * 0.01f);
            }
        }
    }
}
