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
    public class CooldownReduceWhenHitTripod : Tripod
    {
        public override string Description => string.Format(Localization.GetLocalizedString(description), reduceCooldownProbability, reduceCooldownRate);
        
        [SettingValue]
        private float reduceCooldownProbability;
        
        [SettingValue]
        private float reduceCooldownRate;
        
        public override void Activate()
        {
            D.SelfPlayer.onSharedHitMob.Add(CooldownReduce);
        }

        public override void DeActivate()
        {
            D.SelfPlayer.onSharedHitMob.Remove(CooldownReduce);
        }
        
        private void CooldownReduce(Mob mob)
        {
            bool isCooldown = Random.Range(0, 100f) <= reduceCooldownProbability;

            if (isCooldown)
            {
                mob.SkillGroup.GetAllSkills().ForEach(skill => skill.ReduceCooldownRate(reduceCooldownRate * 0.01f));
                mob.SkillGroup.ExtraSkill.ReduceCooldownRate(reduceCooldownRate * 0.01f);
            }
        }
    }
}