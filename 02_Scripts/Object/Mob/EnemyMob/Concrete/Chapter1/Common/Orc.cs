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
    public enum OrcAnimType
    {
        IdleNormal = 1,
        Walk = 5,
        WalkBackwards = 6,
        StrafeLeft = 7,
        StrafeRight = 8,
        Run = 9,
        Death = 13,
        GetHit = 17,
        Hit2ComboA = 21,
        Hit2ComboB = 22,
        Hit3ComboA = 23,
        Hit3ComboB = 24,
        Hit4Combo = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Common)]
    public class Orc : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.IdleNormal);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)OrcAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)OrcAnimType.GetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)OrcAnimType.IdleNormal)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.IdleNormal);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)OrcAnimType.Hit2ComboA
                || CurrentAnim == (int)OrcAnimType.Hit2ComboB
                || CurrentAnim == (int)OrcAnimType.Hit3ComboA
                || CurrentAnim == (int)OrcAnimType.Hit3ComboB
                || CurrentAnim == (int)OrcAnimType.Hit4Combo
                || CurrentAnim == (int)OrcAnimType.GetHit)
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
                    StartAnimationWithReturnIdle(OrcAnimType.Hit2ComboA);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(OrcAnimType.Hit2ComboB);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(OrcAnimType.Hit3ComboA);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(OrcAnimType.Hit3ComboB);
                    break;
                default:
                    StartAnimationWithReturnIdle(OrcAnimType.Hit4Combo);
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

            if (CurrentAnim == (int)OrcAnimType.GetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(OrcAnimType.GetHit);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.StrafeLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.StrafeRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.StrafeLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.StrafeRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.Walk);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(OrcAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)OrcAnimType.IdleNormal);
        }

    }
}
