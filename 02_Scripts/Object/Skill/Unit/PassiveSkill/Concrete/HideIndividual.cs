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
using UnityEngine;

namespace ProjectL
{
    [SkillType(SkillAttackType.Passive, SkillGradeType.Individual)]
    public class HideIndividual : PassiveSkill
    {
        private Coroutine individualCoroutine;

        protected override void Active()
        {
            Debug.Log($"HideIndividual.Active(), Skill : {this.name}");
            
            if (individualCoroutine != null)
            {
                StopCoroutine(individualCoroutine);
                individualCoroutine = null;
            }

            individualCoroutine = StartCoroutine(StartIndividualSkill());
        }

        protected override void InActive()
        {
            Debug.Log($"HideIndividual.InActive(), Skill : {this.name}");

            if (individualCoroutine != null)
            {
                StopCoroutine(individualCoroutine);
                individualCoroutine = null;
            }
            
            if (Unit != null)
            {
                Unit.IsInvincible = false;    
            }
        }
        
        IEnumerator StartIndividualSkill()
        {
            while (true)
            {
                _StartSkillEffect(Unit.transform.position);
                
                Unit.IsInvincible = true;
                yield return new WaitForSeconds(1.0f);

                Unit.IsInvincible = false;
                yield return new WaitForSeconds(Cooldown);
            }
        }

    }
}