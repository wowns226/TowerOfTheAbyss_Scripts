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
    [MobType(ChapterType.ElfCastle, GradeType.Epic, MobType.Hero)]
    public class EpicMay : PlayerMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Idle_A);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)MayAnimType.DieA 
                || CurrentAnim == (int)MayAnimType.DieB)
            {
                return;
            }
            
            int index = Random.Range(0, 2);

            if (index == 0)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.DieA);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.DieB);
            }
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)MayAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)MayAnimType.Idle_A 
                || CurrentAnim == (int)MayAnimType.Idle_B 
                || CurrentAnim == (int)MayAnimType.Idle_C)
            {
                return;
            }

            int index = Random.Range(0, 3);

            if (index == 0)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Idle_A);
            }
            else if(index == 1)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Idle_B);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Idle_C);
            }
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)MayAnimType.atk_5
                || CurrentAnim == (int)MayAnimType.atk_4
                || CurrentAnim == (int)MayAnimType.Atk3
                || CurrentAnim == (int)MayAnimType.Atk8
                || CurrentAnim == (int)MayAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 4);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(MayAnimType.atk_5);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(MayAnimType.atk_4);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(MayAnimType.Atk3);
                    break;
                default:
                    StartAnimationWithReturnIdle(MayAnimType.Atk8);
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

            if (CurrentAnim == (int)MayAnimType.Damage)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(MayAnimType.Damage);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Run_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Run_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Walk_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Walk_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Walk);
            }
        }
        
        private void StartAnimationWithReturnIdle(MayAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)MayAnimType.Idle_A);
        }

    }
}
