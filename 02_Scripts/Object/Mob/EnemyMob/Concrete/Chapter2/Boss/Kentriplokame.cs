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
    public enum KentriplokameAnimType
    {
        Idle = 1,
        CrawlForward = 5,
        CrawlBackwards = 6,
        CrawlLeft = 7,
        CrawlRight = 8,
        Death = 13,
        GetHitFront = 17,
        BiteAttack = 21,
        TentacleSmashLeft = 22,
        TentacleSmashLeftForward = 23,
        TentacleSmashRight = 24,
        TentacleSmashRightForward = 25,
        StingerAttack = 26,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Boss)]
    public class Kentriplokame : EnemyMob
    {
        //공격력 1배, 속도 5, 체력, 방어력 0.7배, 사거리 8
        //랜덤 5칸 이동 못하게
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)KentriplokameAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)KentriplokameAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)KentriplokameAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)KentriplokameAnimType.BiteAttack
                || CurrentAnim == (int)KentriplokameAnimType.TentacleSmashLeft
                || CurrentAnim == (int)KentriplokameAnimType.TentacleSmashLeftForward
                || CurrentAnim == (int)KentriplokameAnimType.TentacleSmashRight
                || CurrentAnim == (int)KentriplokameAnimType.TentacleSmashRightForward
                || CurrentAnim == (int)KentriplokameAnimType.StingerAttack
                || CurrentAnim == (int)KentriplokameAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 6);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(KentriplokameAnimType.BiteAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(KentriplokameAnimType.TentacleSmashLeft);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(KentriplokameAnimType.TentacleSmashLeftForward);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(KentriplokameAnimType.TentacleSmashRight);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(KentriplokameAnimType.TentacleSmashRightForward);
                    break;
                default:
                    StartAnimationWithReturnIdle(KentriplokameAnimType.StingerAttack);
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

            if (CurrentAnim == (int)KentriplokameAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(KentriplokameAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlBackwards);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.CrawlForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(KentriplokameAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)KentriplokameAnimType.Idle);
        }

    }
}
