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
    public enum HydraAnimType
    {
        IdleLookAround = 1,
        Walk = 5,
        Run = 9,
        Death = 13,
        GetHitFront1 = 17,
        Turn45Left3toxicSpitCombo = 21,
        Turn45LeftBite3HitCombo = 22,
        Turn45Right3toxicSpitCombo = 23,
        Turn45RightBite3HitCombo = 24,
        ToxicSpitCombo = 25,
        Bite3HitCombo = 26,
        Bite6HitCombo = 27,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Boss)]
    public class Hydra : EnemyMob
    {
        //스킬 속성 2개? 자연치유 엄청 높게, 1번 부활 부활때 스킬 바뀌게(스킬 그룹 2개)
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.IdleLookAround);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)HydraAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)HydraAnimType.GetHitFront1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)HydraAnimType.IdleLookAround)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.IdleLookAround);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)HydraAnimType.Turn45Left3toxicSpitCombo
                || CurrentAnim == (int)HydraAnimType.Turn45LeftBite3HitCombo
                || CurrentAnim == (int)HydraAnimType.Turn45Right3toxicSpitCombo
                || CurrentAnim == (int)HydraAnimType.Turn45RightBite3HitCombo
                || CurrentAnim == (int)HydraAnimType.ToxicSpitCombo
                || CurrentAnim == (int)HydraAnimType.Bite3HitCombo
                || CurrentAnim == (int)HydraAnimType.Bite6HitCombo
                || CurrentAnim == (int)HydraAnimType.GetHitFront1)
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
                    StartAnimationWithReturnIdle(HydraAnimType.Turn45Left3toxicSpitCombo);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(HydraAnimType.Turn45LeftBite3HitCombo);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(HydraAnimType.Turn45Right3toxicSpitCombo);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(HydraAnimType.Turn45RightBite3HitCombo);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(HydraAnimType.ToxicSpitCombo);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(HydraAnimType.Bite3HitCombo);
                    break;
                default:
                    StartAnimationWithReturnIdle(HydraAnimType.Bite6HitCombo);
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

            if (CurrentAnim == (int)HydraAnimType.GetHitFront1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(HydraAnimType.GetHitFront1);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.Walk);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.Walk);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.Run);
            }
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.Walk);
        }
        
        
        private void StartAnimationWithReturnIdle(HydraAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)HydraAnimType.IdleLookAround);
        }

    }
}
