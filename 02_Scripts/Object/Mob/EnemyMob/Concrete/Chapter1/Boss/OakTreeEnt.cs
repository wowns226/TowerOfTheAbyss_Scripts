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
    public enum OakTreeEntAnimType
    {
        IdleBreathe = 1,
        Walk = 5,
        Death = 13,
        GetHit1 = 17,
        ClawsAttackR = 21,
        StompAttack = 22,
        TurnLeft90ClawsAttack = 23,
        TurnLeft90StompAttack = 24,
        TurnRight90ClawsAttack = 25,
        TurnRight90StompAttack = 26,
        Attack3HitCombo = 27,
        ClawsAttack2HitCombo = 28,
        ClawsAttackL = 29,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Boss)]
    public class OakTreeEnt : EnemyMob
    {
        //20초에 한번 5초간 무적되면서 잃은 체력의 20프로 회복 스피드 0
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)OakTreeEntAnimType.IdleBreathe);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)OakTreeEntAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)OakTreeEntAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)OakTreeEntAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)OakTreeEntAnimType.IdleBreathe)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)OakTreeEntAnimType.IdleBreathe);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)OakTreeEntAnimType.ClawsAttackR
                || CurrentAnim == (int)OakTreeEntAnimType.StompAttack
                || CurrentAnim == (int)OakTreeEntAnimType.TurnLeft90ClawsAttack
                || CurrentAnim == (int)OakTreeEntAnimType.TurnLeft90StompAttack
                || CurrentAnim == (int)OakTreeEntAnimType.TurnRight90ClawsAttack
                || CurrentAnim == (int)OakTreeEntAnimType.TurnRight90StompAttack
                || CurrentAnim == (int)OakTreeEntAnimType.Attack3HitCombo
                || CurrentAnim == (int)OakTreeEntAnimType.ClawsAttack2HitCombo
                || CurrentAnim == (int)OakTreeEntAnimType.ClawsAttackL
                || CurrentAnim == (int)OakTreeEntAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 9);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.ClawsAttackR);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.StompAttack);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.TurnLeft90ClawsAttack);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.TurnLeft90StompAttack);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.TurnRight90ClawsAttack);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.TurnRight90StompAttack);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.Attack3HitCombo);
                    break;
                case 7:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.ClawsAttack2HitCombo);
                    break;
                default:
                    StartAnimationWithReturnIdle(OakTreeEntAnimType.ClawsAttackL);
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

            if (CurrentAnim == (int)OakTreeEntAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(OakTreeEntAnimType.GetHit1);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)OakTreeEntAnimType.Walk);
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)OakTreeEntAnimType.Walk);
        }
        
        
        private void StartAnimationWithReturnIdle(OakTreeEntAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)OakTreeEntAnimType.IdleBreathe);
        }

    }
}
