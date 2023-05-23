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
    public enum GolemAnimType
    {
        idle = 1,
        walk = 5,
        death = 13,
        getHitHeavy = 17,
        Hit2Combo_A = 21,
        Hit2Combo_B = 22,
        Hit3Combo = 23,
        attack1 = 24,
        Attack2 = 25,
        attack3 = 26,
        attack4 = 27,
        attack5 = 28,
        turn90AttackLeft = 29,
        turn90AttackRight = 30,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Named)]
    public class GolemIce : EnemyMob
    {
        private Coroutine returnIdleCoroutine;
        
        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)GolemAnimType.idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)GolemAnimType.death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)GolemAnimType.death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)GolemAnimType.getHitHeavy)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)GolemAnimType.idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)GolemAnimType.idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)GolemAnimType.Hit2Combo_A
                || CurrentAnim == (int)GolemAnimType.Hit2Combo_B
                || CurrentAnim == (int)GolemAnimType.Hit3Combo
                || CurrentAnim == (int)GolemAnimType.attack1
                || CurrentAnim == (int)GolemAnimType.Attack2
                || CurrentAnim == (int)GolemAnimType.attack3
                || CurrentAnim == (int)GolemAnimType.attack4
                || CurrentAnim == (int)GolemAnimType.attack5
                || CurrentAnim == (int)GolemAnimType.turn90AttackLeft
                || CurrentAnim == (int)GolemAnimType.turn90AttackRight
                || CurrentAnim == (int)GolemAnimType.getHitHeavy)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 10);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(GolemAnimType.Hit2Combo_A);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(GolemAnimType.Hit2Combo_B);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(GolemAnimType.attack1);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(GolemAnimType.Attack2);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(GolemAnimType.attack3);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(GolemAnimType.attack4);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(GolemAnimType.attack5);
                    break;
                case 7:
                    StartAnimationWithReturnIdle(GolemAnimType.Hit3Combo);
                    break;
                case 8:
                    StartAnimationWithReturnIdle(GolemAnimType.turn90AttackLeft);
                    break;
                default:
                    StartAnimationWithReturnIdle(GolemAnimType.turn90AttackRight);
                    break;
            }
            
        }

        protected override void StunAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.StunAnim();

            if (CurrentAnim == (int)GolemAnimType.getHitHeavy)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(GolemAnimType.getHitHeavy);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)GolemAnimType.walk);
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)GolemAnimType.walk);
        }
        
        
        private void StartAnimationWithReturnIdle(GolemAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)GolemAnimType.idle);
        }

    }
}
