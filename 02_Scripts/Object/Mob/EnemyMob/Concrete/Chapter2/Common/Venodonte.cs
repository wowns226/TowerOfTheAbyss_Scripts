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
    public enum VenodonteAnimType
    {
        Idle = 1,
        CrawlForward = 5,
        CrawlBackwards = 6,
        CrawlLeft = 7,
        CrawlRight = 8,
        Death = 13,
        GetHitFront = 17,
        AcidShotCombo = 21,
        AcidShot = 22,
        BiteForwardAttack = 23,
        JumpBiteAttack = 24,
        Taunt = 25,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class Venodonte : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)VenodonteAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)VenodonteAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)VenodonteAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)VenodonteAnimType.AcidShotCombo
                || CurrentAnim == (int)VenodonteAnimType.AcidShot
                || CurrentAnim == (int)VenodonteAnimType.BiteForwardAttack
                || CurrentAnim == (int)VenodonteAnimType.JumpBiteAttack
                || CurrentAnim == (int)VenodonteAnimType.Taunt
                || CurrentAnim == (int)VenodonteAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(VenodonteAnimType.AcidShotCombo);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(VenodonteAnimType.AcidShot);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(VenodonteAnimType.BiteForwardAttack);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(VenodonteAnimType.JumpBiteAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(VenodonteAnimType.Taunt);
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

            if (CurrentAnim == (int)VenodonteAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(VenodonteAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.CrawlForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(VenodonteAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)VenodonteAnimType.Idle);
        }

    }
}
