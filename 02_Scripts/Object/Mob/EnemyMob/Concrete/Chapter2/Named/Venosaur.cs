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
    public enum VenosaurAnimType
    {
        Idle = 1,
        WalkForward = 5,
        WalkBackwards = 6,
        WalkLeft = 7,
        WalkRight = 8,
        Run = 9,
        Death = 13,
        GetHitFront = 17,
        JumpClawsAttack = 21,
        HitComboClawsAttack = 22,
        ClawsAttackLeftForward = 23,
        ClawsAttackRightForward = 24,
        BiteAttack = 25,
        ClawsAttackLeft = 26,
        ClawsAttackRight = 27,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Named)]
    public class Venosaur : EnemyMob
    {
        //안보이는 유닛
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)VenosaurAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)VenosaurAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)VenosaurAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)VenosaurAnimType.JumpClawsAttack
                || CurrentAnim == (int)VenosaurAnimType.HitComboClawsAttack
                || CurrentAnim == (int)VenosaurAnimType.ClawsAttackLeftForward
                || CurrentAnim == (int)VenosaurAnimType.ClawsAttackRightForward
                || CurrentAnim == (int)VenosaurAnimType.BiteAttack
                || CurrentAnim == (int)VenosaurAnimType.ClawsAttackLeft
                || CurrentAnim == (int)VenosaurAnimType.ClawsAttackRight
                || CurrentAnim == (int)VenosaurAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(VenosaurAnimType.JumpClawsAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(VenosaurAnimType.HitComboClawsAttack);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(VenosaurAnimType.ClawsAttackLeftForward);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(VenosaurAnimType.ClawsAttackRightForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(VenosaurAnimType.BiteAttack);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(VenosaurAnimType.ClawsAttackLeft);
                    break;
                default:
                    StartAnimationWithReturnIdle(VenosaurAnimType.ClawsAttackRight);
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

            if (CurrentAnim == (int)VenosaurAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(VenosaurAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.WalkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(VenosaurAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)VenosaurAnimType.Idle);
        }

    }
}
