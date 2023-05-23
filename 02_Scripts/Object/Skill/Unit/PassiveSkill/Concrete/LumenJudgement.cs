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
    public class LumenJudgement : PassiveSkill
    {
        private Coroutine snipingModeCoroutine;
        private bool isActiveBuff = false;
        
        protected override void Active()
        {
            Debug.Log($"LumenJudgement.Active(), Skill : {this.name}");
            
            if (snipingModeCoroutine != null)
            {
                StopCoroutine(snipingModeCoroutine);
                snipingModeCoroutine = null;
            }

            snipingModeCoroutine = StartCoroutine(StartSnipingSkill());
        }

        protected override void InActive()
        {
            Debug.Log($"LumenJudgement.InActive(), Skill : {this.name}");
            
            if (snipingModeCoroutine != null)
            {
                StopCoroutine(snipingModeCoroutine);
                snipingModeCoroutine = null;
            }

            if (isActiveBuff)
            {
                DeActiveBuff();
            }
        }
        
        IEnumerator StartSnipingSkill()
        {
            while (true)
            {
                _StartSkillEffect(Unit.transform.position);

                ActiveBuff();
                yield return new WaitForSeconds(10.0f);

                DeActiveBuff();
                yield return new WaitForSeconds(Cooldown);
            }
        }

        private void ActiveBuff()
        {
            if (Unit == null)
            {
                return;
            }
            
            isActiveBuff = true;
            Unit.UpgradeStatPercentage(StatType.Speed, -50f);
            Unit.UpgradeStatPercentage(StatType.Damage, 20f);
            Unit.UpgradeStatPercentage(StatType.CriticalProbability, 20f);
            Unit.UpgradeStatPercentage(StatType.Range, 50f);
            Unit.UpgradeStatPercentage(StatType.CriticalDamagePercentage, 50f);
        }

        private void DeActiveBuff()
        {
            if (Unit == null)
            {
                return;
            }
            
            isActiveBuff = false;
            Unit.UpgradeStatPercentage(StatType.Speed, 50f);
            Unit.UpgradeStatPercentage(StatType.Damage, -20f);
            Unit.UpgradeStatPercentage(StatType.CriticalProbability, -20f);
            Unit.UpgradeStatPercentage(StatType.Range, -50f);
            Unit.UpgradeStatPercentage(StatType.CriticalDamagePercentage, -50f);
        }
    }
}
