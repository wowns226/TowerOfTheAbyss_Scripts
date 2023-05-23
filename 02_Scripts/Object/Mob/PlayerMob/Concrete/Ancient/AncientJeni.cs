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
    public enum JeniAnimType
    {
        Idle_A = 1,
        Idle_B = 2,
        Angpose = 3,
        Damage = 4,
        DieA = 5,
        Walk = 6,
        Walk_L = 7,
        Walk_R = 8,
        Walk_back = 9,
        Run = 10,
        Run_L = 11,
        Run_R = 12,
        Jump = 13,
        DieB = 14,
        CuteA = 15,
        Bye = 16,
        Cry = 17,
        Idle_C = 18,
        IdleD = 19,
        Kneeling = 20,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Ancient, MobType.Hero)]
    public class AncientJeni : PlayerMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Idle_A);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)JeniAnimType.DieA 
                || CurrentAnim == (int)JeniAnimType.DieB)
            {
                return;
            }
            
            int index = Random.Range(0, 2);

            if (index == 0)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.DieA);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.DieB);
            }
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)JeniAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)JeniAnimType.Idle_A 
                || CurrentAnim == (int)JeniAnimType.Idle_B 
                || CurrentAnim == (int)JeniAnimType.Idle_C)
            {
                return;
            }

            int index = Random.Range(0, 3);

            if (index == 0)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Idle_A);
            }
            else if(index == 1)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Idle_B);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Idle_C);
            }
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)JeniAnimType.Kneeling
                || CurrentAnim == (int)JeniAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            StartAnimationWithReturnIdle(JeniAnimType.Kneeling);
            
        }

        protected override void HitAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.HitAnim();

            if (CurrentAnim == (int)JeniAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(JeniAnimType.Damage);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            if (isSide && isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Run_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Run_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Run);
            }
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            if (isSide && isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Walk_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Walk_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Walk);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(JeniAnimType animType)
        {
            unitAnimator?.SetInteger(MOTION_KEY, (int)animType);
            
            if (returnIdleCoroutine != null)
            {
                StopCoroutine(returnIdleCoroutine);
                returnIdleCoroutine = null;
            }

            returnIdleCoroutine = StartCoroutine(ReturnIdleWhenAnimationEnd(animType.ToString()));
        }
        
        IEnumerator ReturnIdleWhenAnimationEnd(string animationName)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(animationName))
                {
                    yield break;
                }
                
                if (unitAnimator?.GetCurrentAnimatorStateInfo(0).IsName(animationName) == true)
                {
                    if(unitAnimator?.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                    {
                        break;
                    }
                }
                
                yield return null; //애니메이션 실행까지 대기
            }
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)JeniAnimType.Idle_A);
        }

    }
}
