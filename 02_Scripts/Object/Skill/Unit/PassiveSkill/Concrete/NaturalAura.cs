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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectL
{
    [SkillType(SkillAttackType.Passive, SkillGradeType.Individual)]
    public class NaturalAura : PassiveSkill
    {
        private Coroutine naturalHealCoroutine;
        private List<Point> points;
        
        protected override void Active()
        {
            Debug.Log($"NaturalAura.Active(), Skill : {this.name}");
            
            if (naturalHealCoroutine != null)
            {
                StopCoroutine(naturalHealCoroutine);
                naturalHealCoroutine = null;
            }

            naturalHealCoroutine = StartCoroutine(StartNaturalHealing());
        }

        protected override void InActive()
        {
            Debug.Log($"NaturalAura.InActive(), Skill : {this.name}");
            
            if (naturalHealCoroutine != null)
            {
                StopCoroutine(naturalHealCoroutine);
                naturalHealCoroutine = null;
            }
        }
        
        IEnumerator StartNaturalHealing()
        {
            while (true)
            {
                _StartSkillEffect(Unit.transform.position);
                
                Unit.HealHpPercentage(SkillValue);
                
                yield return new WaitForSeconds(Cooldown);
            }
        }

    }
}