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
    public enum SerpenopodAnimType
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
        BiteAttackForward = 22,
        DoubleClawsAttack = 23,
        DoubleClawsAttackForward = 24,
        SpitAttack1 = 25,
        SpitAttack2 = 26,
        LeftClawsAttack = 27,
        RightClawsAttack = 28,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Named)]
    public class Serpenopod : EnemyMob
    {
        //원거리
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)SerpenopodAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)SerpenopodAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)SerpenopodAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)SerpenopodAnimType.BiteAttack
                || CurrentAnim == (int)SerpenopodAnimType.BiteAttackForward
                || CurrentAnim == (int)SerpenopodAnimType.DoubleClawsAttack
                || CurrentAnim == (int)SerpenopodAnimType.DoubleClawsAttackForward
                || CurrentAnim == (int)SerpenopodAnimType.SpitAttack1
                || CurrentAnim == (int)SerpenopodAnimType.SpitAttack2
                || CurrentAnim == (int)SerpenopodAnimType.LeftClawsAttack
                || CurrentAnim == (int)SerpenopodAnimType.RightClawsAttack
                || CurrentAnim == (int)SerpenopodAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(SerpenopodAnimType.BiteAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.BiteAttackForward);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.DoubleClawsAttack);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.DoubleClawsAttackForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.SpitAttack1);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.SpitAttack2);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.LeftClawsAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(SerpenopodAnimType.RightClawsAttack);
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

            if (CurrentAnim == (int)SerpenopodAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(SerpenopodAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.Run);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(SerpenopodAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)SerpenopodAnimType.Idle);
        }

    }
}
