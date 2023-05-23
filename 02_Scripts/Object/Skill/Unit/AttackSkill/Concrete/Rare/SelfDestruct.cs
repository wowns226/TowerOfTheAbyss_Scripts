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
    [SkillType(SkillAttackType.Attack, SkillGradeType.Rare)]
    public class SelfDestruct : AttackSkill
    {
        private Coroutine selfDestructTimerCoroutine;

        protected override void _Init()
        {
            base._Init();
            Unit.onSpawn.Add(Active);
            Unit.onDeath.Add(InActive);
        }

        public override void Clear()
        {
            base.Clear();

            if (Unit != null)
            {
                Unit.onSpawn.Remove(Active);
                Unit.onDeath.Remove(InActive);
            }
        }

        private void Active(Unit unit)
        {
            Debug.Log($"SelfDestruct.Active(), Skill : {this.name}");
            
            if (selfDestructTimerCoroutine != null)
            {
                StopCoroutine(selfDestructTimerCoroutine);
                selfDestructTimerCoroutine = null;
            }

            selfDestructTimerCoroutine = StartCoroutine(StartSelfDestructTimer());
        }

        private void InActive(Unit unit)
        {
            Debug.Log($"SelfDestruct.InActive(), Skill : {this.name}");
            
            if (selfDestructTimerCoroutine != null)
            {
                StopCoroutine(selfDestructTimerCoroutine);
                selfDestructTimerCoroutine = null;
            }
        }
        
        IEnumerator StartSelfDestructTimer()
        {
            UsedTime = Time.time;
            CanUseTime = Time.time + baseCooldown;
            int remainTime = (int)baseCooldown;
            Debug.Log($"SelfDestruct.StartSelfDestructTimer(), Start Skill : {this.name}");
            
            while (remainTime > 0)
            {
                Debug.Log($"SelfDestruct.StartSelfDestructTimer(), Start Skill : {this.name}, RemainTime : {remainTime}");
                PopupManager.Instance.OpenPopup(PopupType.Timer, transform.position, remainTime);
                remainTime--;
                
                yield return new WaitForSeconds(1.0f);
            }
            
            Debug.Log($"SelfDestruct.StartSelfDestructTimer(), Use Skill : {this.name}");
            Use(Unit.BasePoint);
        }

    }
}