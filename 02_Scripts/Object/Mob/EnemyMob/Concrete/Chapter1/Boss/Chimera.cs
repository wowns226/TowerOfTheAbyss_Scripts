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
    public enum ChimeraAnimType
    {
        Idle = 1,
        Walk = 5,
        Run = 9,
        Death = 13,
        GetHit1 = 17,
        SnakeBiteAttack = 21,
        Claws2HitComboAttackForward = 22,
        ClawsAttackForwardL = 23,
        ClawsAttackForwardR = 24,
        ClawsAttackL = 25,
        ClawsAttackR = 26,
        RamAttack = 27,
        RamBiteComboAttack = 28,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Boss)]
    public class Chimera : EnemyMob
    {
        //100이하의 데미지는 절대값으로 감소
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)ChimeraAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)ChimeraAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)ChimeraAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)ChimeraAnimType.SnakeBiteAttack
                || CurrentAnim == (int)ChimeraAnimType.Claws2HitComboAttackForward
                || CurrentAnim == (int)ChimeraAnimType.ClawsAttackForwardL
                || CurrentAnim == (int)ChimeraAnimType.ClawsAttackForwardR
                || CurrentAnim == (int)ChimeraAnimType.ClawsAttackL
                || CurrentAnim == (int)ChimeraAnimType.ClawsAttackR
                || CurrentAnim == (int)ChimeraAnimType.RamAttack
                || CurrentAnim == (int)ChimeraAnimType.RamBiteComboAttack
                || CurrentAnim == (int)ChimeraAnimType.GetHit1)
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
                    StartAnimationWithReturnIdle(ChimeraAnimType.SnakeBiteAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(ChimeraAnimType.Claws2HitComboAttackForward);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(ChimeraAnimType.ClawsAttackForwardL);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(ChimeraAnimType.ClawsAttackForwardR);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(ChimeraAnimType.ClawsAttackL);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(ChimeraAnimType.ClawsAttackR);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(ChimeraAnimType.RamAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(ChimeraAnimType.RamBiteComboAttack);
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

            if (CurrentAnim == (int)ChimeraAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(ChimeraAnimType.GetHit1);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Walk);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Walk);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Run);
            }
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Walk);
        }
        
        
        private void StartAnimationWithReturnIdle(ChimeraAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)ChimeraAnimType.Idle);
        }

    }
}
