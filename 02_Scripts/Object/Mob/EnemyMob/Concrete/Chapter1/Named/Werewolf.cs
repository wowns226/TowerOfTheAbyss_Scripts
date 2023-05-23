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
    public enum WerewolfAnimType
    {
        idleBreathe = 1,
        walk = 5,
        run = 9,
        death = 13,
        getHit1 = 17,
        clawsAttack2HitCombo = 21,
        clawsAttackLeft = 22,
        clawsAttackRight = 23,
        jumpBiteAttack = 24,
        jumpClawsAttack = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Named)]
    public class Werewolf : EnemyMob
    {
        //투명
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.idleBreathe);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)WerewolfAnimType.death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)WerewolfAnimType.getHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)WerewolfAnimType.idleBreathe)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.idleBreathe);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)WerewolfAnimType.clawsAttack2HitCombo
                || CurrentAnim == (int)WerewolfAnimType.clawsAttackLeft
                || CurrentAnim == (int)WerewolfAnimType.clawsAttackRight
                || CurrentAnim == (int)WerewolfAnimType.jumpClawsAttack
                || CurrentAnim == (int)WerewolfAnimType.jumpBiteAttack
                || CurrentAnim == (int)WerewolfAnimType.getHit1)
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
                    StartAnimationWithReturnIdle(WerewolfAnimType.clawsAttackRight);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(WerewolfAnimType.clawsAttackLeft);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(WerewolfAnimType.clawsAttack2HitCombo);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(WerewolfAnimType.jumpClawsAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(WerewolfAnimType.jumpBiteAttack);
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

            if (CurrentAnim == (int)WerewolfAnimType.getHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(WerewolfAnimType.getHit1);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.walk);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.walk);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.run);
            }
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.walk);
        }
        
        
        private void StartAnimationWithReturnIdle(WerewolfAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)WerewolfAnimType.idleBreathe);
        }

    }
}
