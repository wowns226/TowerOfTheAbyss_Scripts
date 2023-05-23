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
    [MobType(ChapterType.PassageOfTime, GradeType.Unique, MobType.Hero)]
    public class UniqueNovember : PlayerMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)NovemberAnimType.Die)
            {
                return;
            }
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Die);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }
            
            base.IdleAnim();
            
            if (CurrentAnim == (int)NovemberAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)NovemberAnimType.Idle 
                || CurrentAnim == (int)NovemberAnimType.Idle_E)
            {
                return;
            }

            int index = Random.Range(0, 2);

            switch (index)
            {
                case 0:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Idle);
                    break;
                default:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Idle_E);
                    break; 
            }
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }
            
            base.AttackAnim();

            if (CurrentAnim == (int)NovemberAnimType.Atk3
                || CurrentAnim == (int)NovemberAnimType.atk_4
                || CurrentAnim == (int)NovemberAnimType.atk_5
                || CurrentAnim == (int)NovemberAnimType.Atk6
                || CurrentAnim == (int)NovemberAnimType.Atk7
                || CurrentAnim == (int)NovemberAnimType.Atk8
                || CurrentAnim == (int)NovemberAnimType.Atk9
                || CurrentAnim == (int)NovemberAnimType.mad_combo
                || CurrentAnim == (int)NovemberAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 8);
            
            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(NovemberAnimType.Atk3);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(NovemberAnimType.atk_4);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(NovemberAnimType.atk_5);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(NovemberAnimType.Atk6);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(NovemberAnimType.Atk7);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(NovemberAnimType.Atk8);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(NovemberAnimType.Atk9);
                    break;
                default:
                    StartAnimationWithReturnIdle(NovemberAnimType.mad_combo);
                    break; 
            }
        }

        protected override void HitAnim()
        {
            if (IsDeath)
            {
                return;
            }
            
            base.HitAnim();

            if (CurrentAnim == (int)NovemberAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(NovemberAnimType.Damage);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Run_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Run_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Walk_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Walk_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Walk);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(NovemberAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)NovemberAnimType.Idle);
        }

    }
}
