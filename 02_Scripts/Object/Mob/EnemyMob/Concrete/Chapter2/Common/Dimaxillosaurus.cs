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
    public enum DimaxillosaurusAnimType
    {
        Idle = 1,
        WalkForward = 5,
        WalkBackwards = 6,
        WalkLeft = 7,
        WalkRight = 8,
        Run = 9,
        Death = 13,
        GetHitFront = 17,
        BiteAttack = 21,
        RightClawsAttack = 22,
        HitComboClawsAttack = 23,
        HitComboClawsAttackForward = 24,
        LeftClawsAttack = 25,
        LeftClawsAttackForward = 26,
        JumpClawsAttack = 27,
        RightClawsAttackForward = 28,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class Dimaxillosaurus : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)DimaxillosaurusAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)DimaxillosaurusAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)DimaxillosaurusAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)DimaxillosaurusAnimType.BiteAttack
                || CurrentAnim == (int)DimaxillosaurusAnimType.RightClawsAttack
                || CurrentAnim == (int)DimaxillosaurusAnimType.HitComboClawsAttack
                || CurrentAnim == (int)DimaxillosaurusAnimType.HitComboClawsAttackForward
                || CurrentAnim == (int)DimaxillosaurusAnimType.LeftClawsAttack
                || CurrentAnim == (int)DimaxillosaurusAnimType.LeftClawsAttackForward
                || CurrentAnim == (int)DimaxillosaurusAnimType.JumpClawsAttack
                || CurrentAnim == (int)DimaxillosaurusAnimType.RightClawsAttackForward
                || CurrentAnim == (int)DimaxillosaurusAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 9);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.BiteAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.RightClawsAttack);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.HitComboClawsAttack);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.HitComboClawsAttackForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.LeftClawsAttack);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.LeftClawsAttackForward);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.JumpClawsAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(DimaxillosaurusAnimType.RightClawsAttackForward);
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

            if (CurrentAnim == (int)DimaxillosaurusAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(DimaxillosaurusAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.WalkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(DimaxillosaurusAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)DimaxillosaurusAnimType.Idle);
        }

    }
}
