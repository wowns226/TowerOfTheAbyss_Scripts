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
    public enum ManticoraAnimType
    {
        Idle = 1,
        Walk = 5,
        Run = 9,
        Death = 13,
        GetHit1 = 17,
        Hit2ComboClawsAttackForward = 21,
        Hit2ComboStingerAttackCombat = 22,
        Hit3ComboClawsBiteAttackForward = 23,
        Hit4ComboClawsBiteStingerAttackForward = 24,
        Bite = 25,
        ClawsLeftAttackCombat = 26,
        ClawsRightAttackCombat = 27,
        StingerAttackCombat = 28,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Named)]
    public class Manticora : EnemyMob
    {
        //원거리
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)ManticoraAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)ManticoraAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)ManticoraAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)ManticoraAnimType.Hit2ComboClawsAttackForward
                || CurrentAnim == (int)ManticoraAnimType.Hit2ComboStingerAttackCombat
                || CurrentAnim == (int)ManticoraAnimType.Hit3ComboClawsBiteAttackForward
                || CurrentAnim == (int)ManticoraAnimType.Hit4ComboClawsBiteStingerAttackForward
                || CurrentAnim == (int)ManticoraAnimType.Bite
                || CurrentAnim == (int)ManticoraAnimType.ClawsLeftAttackCombat
                || CurrentAnim == (int)ManticoraAnimType.ClawsRightAttackCombat
                || CurrentAnim == (int)ManticoraAnimType.StingerAttackCombat
                || CurrentAnim == (int)ManticoraAnimType.GetHit1)
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
                    StartAnimationWithReturnIdle(ManticoraAnimType.Hit2ComboClawsAttackForward);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(ManticoraAnimType.Hit2ComboStingerAttackCombat);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(ManticoraAnimType.Hit3ComboClawsBiteAttackForward);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(ManticoraAnimType.Hit4ComboClawsBiteStingerAttackForward);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(ManticoraAnimType.Bite);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(ManticoraAnimType.ClawsRightAttackCombat);
                    break;
                case 6:
                    StartAnimationWithReturnIdle(ManticoraAnimType.ClawsLeftAttackCombat);
                    break;
                default:
                    StartAnimationWithReturnIdle(ManticoraAnimType.StingerAttackCombat);
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

            if (CurrentAnim == (int)ManticoraAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(ManticoraAnimType.GetHit1);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Walk);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Walk);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Run);
            }
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Walk);
        }
        
        
        private void StartAnimationWithReturnIdle(ManticoraAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)ManticoraAnimType.Idle);
        }

    }
}
