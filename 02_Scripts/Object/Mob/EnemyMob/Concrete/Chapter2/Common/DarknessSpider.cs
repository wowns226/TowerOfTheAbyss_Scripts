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
    public enum DarknessSpiderAnimType
    {
        IdleNormal = 1,
        CrawlNormal = 5,
        DeathNormal = 13,
        CrawlBiteThreat = 17,
        Bite = 21,
        JumpBiteNormal = 22,
        Bite3HitCombo = 23,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class DarknessSpider : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)DarknessSpiderAnimType.IdleNormal);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)DarknessSpiderAnimType.DeathNormal)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DarknessSpiderAnimType.DeathNormal);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)DarknessSpiderAnimType.CrawlBiteThreat)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)DarknessSpiderAnimType.IdleNormal)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DarknessSpiderAnimType.IdleNormal);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)DarknessSpiderAnimType.Bite
                || CurrentAnim == (int)DarknessSpiderAnimType.JumpBiteNormal
                || CurrentAnim == (int)DarknessSpiderAnimType.Bite3HitCombo
                || CurrentAnim == (int)DarknessSpiderAnimType.CrawlBiteThreat)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 3);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(DarknessSpiderAnimType.Bite);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(DarknessSpiderAnimType.JumpBiteNormal);
                    break;
                default:
                    StartAnimationWithReturnIdle(DarknessSpiderAnimType.Bite3HitCombo);
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

            if (CurrentAnim == (int)DarknessSpiderAnimType.CrawlBiteThreat)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(DarknessSpiderAnimType.CrawlBiteThreat);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)DarknessSpiderAnimType.CrawlNormal);
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)DarknessSpiderAnimType.CrawlNormal);
        }
        
        
        private void StartAnimationWithReturnIdle(DarknessSpiderAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)DarknessSpiderAnimType.IdleNormal);
        }

    }
}
