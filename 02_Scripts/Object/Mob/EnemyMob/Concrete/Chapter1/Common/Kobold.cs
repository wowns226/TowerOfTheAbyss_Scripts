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
    public enum KoboldAnimType
    {
        idleCombat = 1,
        walkForwardCombat = 5,
        walkBackwardsCombat = 6,
        walkLeftCombat = 7,
        walkRightCombat = 8,
        runNormal = 9,
        Death = 13,
        getHitFront = 17,
        Hit2Combo1 = 21,
        Hit2Combo2 = 22,
        Hit3Combo = 23,
        Hit4Combo = 24,
        attack1 = 25,
        attack2 = 26,
        attack3 = 27,
        attack4 = 28,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Common)]
    public class Kobold : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.idleCombat);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)KoboldAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)KoboldAnimType.getHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)KoboldAnimType.idleCombat)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.idleCombat);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)KoboldAnimType.Hit2Combo1
                || CurrentAnim == (int)KoboldAnimType.Hit2Combo2
                || CurrentAnim == (int)KoboldAnimType.Hit3Combo
                || CurrentAnim == (int)KoboldAnimType.Hit4Combo
                || CurrentAnim == (int)KoboldAnimType.attack1
                || CurrentAnim == (int)KoboldAnimType.attack2
                || CurrentAnim == (int)KoboldAnimType.attack3
                || CurrentAnim == (int)KoboldAnimType.attack4
                || CurrentAnim == (int)KoboldAnimType.getHitFront)
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
                    StartAnimationWithReturnIdle(KoboldAnimType.Hit2Combo1);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(KoboldAnimType.Hit2Combo2);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(KoboldAnimType.Hit3Combo);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(KoboldAnimType.Hit4Combo);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(KoboldAnimType.attack1);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(KoboldAnimType.attack2);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(KoboldAnimType.attack3);
                    break;
                default:
                    StartAnimationWithReturnIdle(KoboldAnimType.attack4);
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

            if (CurrentAnim == (int)KoboldAnimType.getHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(KoboldAnimType.getHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkLeftCombat);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkRightCombat);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkBackwardsCombat);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.runNormal);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkLeftCombat);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkRightCombat);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkBackwardsCombat);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.walkForwardCombat);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(KoboldAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)KoboldAnimType.idleCombat);
        }

    }
}
