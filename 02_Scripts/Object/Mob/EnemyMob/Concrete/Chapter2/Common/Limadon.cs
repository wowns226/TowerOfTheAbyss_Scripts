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
    public enum LimadonAnimType
    {
        Idle = 1,
        CrawlForward = 5,
        CrawlBackwards = 6,
        CrawlLeft = 7,
        CrawlRight = 8,
        Death = 13,
        GetHitFront = 17,
        LeftClawsAttackForward = 21,
        RightClawsAttackForward = 22,
        HitComboClawsAttack = 23,
        HitComboClawsAttackForward = 24,
        BiteAttack = 25,
        LeftClawsAttack = 26,
        RightClawsAttack = 27,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class Limadon : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)LimadonAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)LimadonAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)LimadonAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)LimadonAnimType.LeftClawsAttackForward
                || CurrentAnim == (int)LimadonAnimType.RightClawsAttackForward
                || CurrentAnim == (int)LimadonAnimType.HitComboClawsAttack
                || CurrentAnim == (int)LimadonAnimType.HitComboClawsAttackForward
                || CurrentAnim == (int)LimadonAnimType.BiteAttack
                || CurrentAnim == (int)LimadonAnimType.LeftClawsAttack
                || CurrentAnim == (int)LimadonAnimType.RightClawsAttack
                || CurrentAnim == (int)LimadonAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(LimadonAnimType.LeftClawsAttackForward);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(LimadonAnimType.RightClawsAttackForward);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(LimadonAnimType.HitComboClawsAttack);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(LimadonAnimType.HitComboClawsAttackForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(LimadonAnimType.BiteAttack);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(LimadonAnimType.LeftClawsAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(LimadonAnimType.RightClawsAttack);
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

            if (CurrentAnim == (int)LimadonAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(LimadonAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.CrawlForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(LimadonAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)LimadonAnimType.Idle);
        }

    }
}
