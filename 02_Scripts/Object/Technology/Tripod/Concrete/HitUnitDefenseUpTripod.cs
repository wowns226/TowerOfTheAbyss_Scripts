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
    public class HitUnitDefenseUpTripod : Tripod
    {
        public override string Description => string.Format(Localization.GetLocalizedString(description), durationTime, defenseUpPercentage);
        
        [SettingValue]
        private float defenseUpPercentage;
        [SettingValue]
        private float durationTime;
        
        public override void Activate()
        {
            D.SelfPlayer.onSharedHitMob.Add(DefenseUp);
        }

        public override void DeActivate()
        {
            D.SelfPlayer.onSharedHitMob.Remove(DefenseUp);
        }

        private void DefenseUp(Mob mob)
        {
            mob.AddBuffSkill(GetType().Name, StartBuffSkill, EndBuffSkill, Time.time + durationTime);
        }

        private void StartBuffSkill(Unit unit)
        {
            unit.UpgradeStatPercentage(StatType.Defense, defenseUpPercentage);
        }
        
        private void EndBuffSkill(Unit unit)
        {
            unit.UpgradeStatPercentage(StatType.Defense, defenseUpPercentage * -1);
        }
    }
}
