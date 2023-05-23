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
    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Named)]
    public class CyclopsTanker : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)CyclopsAnimType.idleLookAround);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)CyclopsAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)CyclopsAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)CyclopsAnimType.getHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)CyclopsAnimType.idleLookAround)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)CyclopsAnimType.idleLookAround);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)CyclopsAnimType.fists2CrushAttack
                || CurrentAnim == (int)CyclopsAnimType.fists2SmashAttack90Left
                || CurrentAnim == (int)CyclopsAnimType.fists2SmashAttack90Right
                || CurrentAnim == (int)CyclopsAnimType.fists2SmashAttackForward
                || CurrentAnim == (int)CyclopsAnimType.Hit2ComboAttack
                || CurrentAnim == (int)CyclopsAnimType.Hit3ComboAttack
                || CurrentAnim == (int)CyclopsAnimType.leftHandAttack
                || CurrentAnim == (int)CyclopsAnimType.rightHandAttack
                || CurrentAnim == (int)CyclopsAnimType.getHit1)
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
                    StartAnimationWithReturnIdle(CyclopsAnimType.fists2CrushAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(CyclopsAnimType.fists2SmashAttack90Left);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(CyclopsAnimType.fists2SmashAttack90Right);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(CyclopsAnimType.fists2SmashAttackForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(CyclopsAnimType.Hit2ComboAttack);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(CyclopsAnimType.Hit3ComboAttack);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(CyclopsAnimType.leftHandAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(CyclopsAnimType.rightHandAttack);
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

            if (CurrentAnim == (int)CyclopsAnimType.getHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(CyclopsAnimType.getHit1);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)CyclopsAnimType.walk);
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)CyclopsAnimType.walk);
        }
        
        
        private void StartAnimationWithReturnIdle(CyclopsAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)CyclopsAnimType.idleLookAround);
        }

    }
}
