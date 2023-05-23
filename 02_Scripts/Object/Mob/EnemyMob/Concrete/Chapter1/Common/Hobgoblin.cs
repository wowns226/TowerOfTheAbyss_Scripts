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
    public enum HobgoblinAnimType
    {
        idle = 1,
        walkForward = 5,
        walkBackwards = 6,
        walkLeft = 7,
        walkRight = 8,
        run = 9,
        Death = 13,
        getHitFront = 17,
        Hit2Combo = 21,
        Hit3Combo = 22,
        attack1 = 23,
        attack2 = 24,
        attack3 = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Common)]
    public class Hobgoblin : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)HobgoblinAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)HobgoblinAnimType.getHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)HobgoblinAnimType.idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)HobgoblinAnimType.Hit2Combo
                || CurrentAnim == (int)HobgoblinAnimType.Hit3Combo
                || CurrentAnim == (int)HobgoblinAnimType.attack1
                || CurrentAnim == (int)HobgoblinAnimType.attack2
                || CurrentAnim == (int)HobgoblinAnimType.attack3
                || CurrentAnim == (int)HobgoblinAnimType.getHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 5);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(HobgoblinAnimType.Hit2Combo);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(HobgoblinAnimType.Hit3Combo);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(HobgoblinAnimType.attack1);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(HobgoblinAnimType.attack2);
                    break;
                default:
                    StartAnimationWithReturnIdle(HobgoblinAnimType.attack3);
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

            if (CurrentAnim == (int)HobgoblinAnimType.getHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(HobgoblinAnimType.getHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.walkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(HobgoblinAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)HobgoblinAnimType.idle);
        }

    }
}
