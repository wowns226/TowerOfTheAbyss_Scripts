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
    public enum FulgurodonteAnimType
    {
        Idle = 1,
        WalkForward = 5,
        WalkBackwards = 6,
        WalkLeft = 7,
        WalkRight = 8,
        Death = 13,
        GetHitFront = 17,
        HitComboClawsAttack = 21,
        HitComboClawsAttackForward = 22,
        ClawsAttackLeft = 23,
        ClawsAttackLeftForward = 24,
        ClawsAttackRight = 25,
        ClawsAttackRightForward = 26,
        RamAttack = 27,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class Fulgurodonte : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)FulgurodonteAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)FulgurodonteAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)FulgurodonteAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)FulgurodonteAnimType.HitComboClawsAttack
                || CurrentAnim == (int)FulgurodonteAnimType.HitComboClawsAttackForward
                || CurrentAnim == (int)FulgurodonteAnimType.ClawsAttackLeft
                || CurrentAnim == (int)FulgurodonteAnimType.ClawsAttackLeftForward
                || CurrentAnim == (int)FulgurodonteAnimType.ClawsAttackRight
                || CurrentAnim == (int)FulgurodonteAnimType.ClawsAttackRightForward
                || CurrentAnim == (int)FulgurodonteAnimType.RamAttack
                || CurrentAnim == (int)FulgurodonteAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 7);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.HitComboClawsAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.HitComboClawsAttackForward);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.ClawsAttackLeft);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.ClawsAttackLeftForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.ClawsAttackRight);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.ClawsAttackRightForward);
                    break;
                default:
                    StartAnimationWithReturnIdle(FulgurodonteAnimType.RamAttack);
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

            if (CurrentAnim == (int)FulgurodonteAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(FulgurodonteAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.WalkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(FulgurodonteAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)FulgurodonteAnimType.Idle);
        }

    }
}
