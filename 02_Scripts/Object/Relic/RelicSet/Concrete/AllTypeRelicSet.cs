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
    public class AllTypeRelicSet : RelicSet
    {
        public override string DetailDescription =>
            string.Format(Localization.GetLocalizedString(detailDescription)
                , evationProbability
                , rangedTypeValue
                , healTypeValue);
        
        [SettingValue]
        private float rangedTypeValue;
        [SettingValue]
        private float evationProbability;
        [SettingValue]
        private float evationDuration;
        [SettingValue]
        private float healTypeValue;
        
        protected override void _Activate()
        {
            Player.UpgradeStatPercentage(UnitType.Mob, StatType.DrainProbability, AttackType.Ranged, rangedTypeValue);
            Player.onSharedHitMob.Add(Evasion); 
            Player.UpgradeStatPercentage(UnitType.Mob, StatType.Heal, AttackType.Heal, healTypeValue);
        }

        protected override void _InActivate()
        {
            Player.UpgradeStatPercentage(UnitType.Mob, StatType.DrainProbability, AttackType.Ranged, -rangedTypeValue);
            Player.onSharedHitMob.Add(Evasion); 
            Player.UpgradeStatPercentage(UnitType.Mob, StatType.Heal, AttackType.Heal, -healTypeValue);
        }   
        
        private void Evasion(Mob mob)
        {
            bool isEvation = Random.Range(0, 100f) <= evationProbability;

            if (isEvation && mob.AttackType == AttackType.Melee)
            {
                mob.AddBuffSkill(GetType().Name, ActiveEvasion, InActiveEvasion, Time.time + evationDuration);
            }
        }

        private void ActiveEvasion(Unit unit)
        {
            unit.IsInvincible = true;
        }

        private void InActiveEvasion(Unit unit)
        {
            unit.IsInvincible = false;
        }
    }
}