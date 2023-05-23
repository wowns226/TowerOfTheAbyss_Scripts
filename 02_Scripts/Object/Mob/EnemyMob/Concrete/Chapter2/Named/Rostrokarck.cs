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
    public enum RostrokarckAnimType
    {
        Idle = 1,
        CrawlForward = 5,
        CrawlBackwards = 6,
        CrawlLeft = 7,
        CrawlRight = 8,
        Death = 13,
        GetHitFront = 17,
        HitComboAttackClawAttack = 21,
        ClawAttackRight = 22,
        ClawAttackRightForward = 23,
        HitComboAttackClawAttackForward = 24,
        ClawAttackLeft = 25,
        ClawAttackLeftForward = 26,
        DoubleClawsAttack = 27,
        DoubleClawsAttackForward = 28,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Named)]
    public class Rostrokarck : EnemyMob
    {
        //방어력 엄청높고 체력 엄청낮음
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)RostrokarckAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)RostrokarckAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)RostrokarckAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)RostrokarckAnimType.HitComboAttackClawAttack
                || CurrentAnim == (int)RostrokarckAnimType.ClawAttackRight
                || CurrentAnim == (int)RostrokarckAnimType.ClawAttackRightForward
                || CurrentAnim == (int)RostrokarckAnimType.HitComboAttackClawAttackForward
                || CurrentAnim == (int)RostrokarckAnimType.ClawAttackLeft
                || CurrentAnim == (int)RostrokarckAnimType.ClawAttackLeftForward
                || CurrentAnim == (int)RostrokarckAnimType.DoubleClawsAttack
                || CurrentAnim == (int)RostrokarckAnimType.DoubleClawsAttackForward
                || CurrentAnim == (int)RostrokarckAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(RostrokarckAnimType.HitComboAttackClawAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.ClawAttackRight);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.ClawAttackRightForward);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.HitComboAttackClawAttackForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.ClawAttackLeft);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.ClawAttackLeftForward);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.DoubleClawsAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(RostrokarckAnimType.DoubleClawsAttackForward);
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

            if (CurrentAnim == (int)RostrokarckAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(RostrokarckAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.CrawlForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(RostrokarckAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)RostrokarckAnimType.Idle);
        }

    }
}
