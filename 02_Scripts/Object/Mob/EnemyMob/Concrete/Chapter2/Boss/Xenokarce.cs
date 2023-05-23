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
    public enum XenokarceAnimType
    {
        Idle = 1,
        WalkForward = 5,
        WalkBackwards = 6,
        WalkLeft = 7,
        WalkRight = 8,
        Death = 13,
        GetHitFront = 17,
        JumpSmashAttack = 21,
        SmashAttackLeft = 22,
        SmashAttackLeftForward = 23,
        SmashAttackRight = 24,
        SmashAttackRightForward = 25,
        ClawsAttackLeft = 26,
        ClawsAttackLeftForward = 27,
        ClawsAttackRight = 28,
        ClawsAttackRightForward = 29,
        HitComboClawsAttack = 30,
        HitComboSmashAttack = 31,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Boss)]
    public class Xenokarce : EnemyMob
    {
        //주변 아군 체력 5초당 10퍼씩 채워주는 버프 자기제외 보스
        //체력 방어력 1.3배
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)XenokarceAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)XenokarceAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)XenokarceAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)XenokarceAnimType.JumpSmashAttack
                || CurrentAnim == (int)XenokarceAnimType.SmashAttackLeft
                || CurrentAnim == (int)XenokarceAnimType.SmashAttackLeftForward
                || CurrentAnim == (int)XenokarceAnimType.SmashAttackRight
                || CurrentAnim == (int)XenokarceAnimType.SmashAttackRightForward
                || CurrentAnim == (int)XenokarceAnimType.ClawsAttackLeft
                || CurrentAnim == (int)XenokarceAnimType.ClawsAttackLeftForward
                || CurrentAnim == (int)XenokarceAnimType.ClawsAttackRight
                || CurrentAnim == (int)XenokarceAnimType.ClawsAttackRightForward
                || CurrentAnim == (int)XenokarceAnimType.HitComboClawsAttack
                || CurrentAnim == (int)XenokarceAnimType.HitComboSmashAttack
                || CurrentAnim == (int)XenokarceAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 11);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(XenokarceAnimType.JumpSmashAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(XenokarceAnimType.SmashAttackLeft);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(XenokarceAnimType.SmashAttackLeftForward);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(XenokarceAnimType.SmashAttackRight);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(XenokarceAnimType.SmashAttackRightForward);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(XenokarceAnimType.ClawsAttackLeft);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(XenokarceAnimType.ClawsAttackLeftForward);
                    break;
                case 7:
                    StartAnimationWithReturnIdle(XenokarceAnimType.ClawsAttackRight);
                    break;
                case 8:
                    StartAnimationWithReturnIdle(XenokarceAnimType.ClawsAttackRightForward);
                    break;
                case 9:
                    StartAnimationWithReturnIdle(XenokarceAnimType.HitComboClawsAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(XenokarceAnimType.HitComboSmashAttack);
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

            if (CurrentAnim == (int)XenokarceAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(XenokarceAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.WalkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(XenokarceAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)XenokarceAnimType.Idle);
        }

    }
}
