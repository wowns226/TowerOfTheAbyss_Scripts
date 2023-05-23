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
    public enum MummyAnimType
    {
        Idle = 1,
        Walk = 5,
        StrafeLeft = 7,
        StrafeRight = 8,
        Death = 13,
        GetHitHeavy = 17,
        BandageWhipAttack1 = 21,
        BandageWhipAttack2 = 22,
        BandageWhipAttack3 = 23,
        BandageWhip2HitCombo = 24,
        BandageWhip3HitCombo = 25,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class Mummy : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)MummyAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)MummyAnimType.GetHitHeavy)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)MummyAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)MummyAnimType.BandageWhipAttack1
                || CurrentAnim == (int)MummyAnimType.BandageWhipAttack2
                || CurrentAnim == (int)MummyAnimType.BandageWhipAttack3
                || CurrentAnim == (int)MummyAnimType.BandageWhip2HitCombo
                || CurrentAnim == (int)MummyAnimType.BandageWhip3HitCombo
                || CurrentAnim == (int)MummyAnimType.GetHitHeavy)
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
                    StartAnimationWithReturnIdle(MummyAnimType.BandageWhipAttack1);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(MummyAnimType.BandageWhipAttack2);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(MummyAnimType.BandageWhipAttack3);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(MummyAnimType.BandageWhip2HitCombo);
                    break;
                default:
                    StartAnimationWithReturnIdle(MummyAnimType.BandageWhip3HitCombo);
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

            if (CurrentAnim == (int)MummyAnimType.GetHitHeavy)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(MummyAnimType.GetHitHeavy);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.StrafeLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.StrafeRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Walk);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.StrafeLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.StrafeRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Walk);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(MummyAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)MummyAnimType.Idle);
        }

    }
}
