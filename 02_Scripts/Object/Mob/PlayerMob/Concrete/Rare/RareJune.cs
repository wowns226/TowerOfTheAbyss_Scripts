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
    [MobType(ChapterType.ElfCastle, GradeType.Rare, MobType.Hero)]
    public class RareJune : PlayerMob
    {
        private Coroutine returnIdleCoroutine;
        
        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Idle_A);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)JuneAnimType.DieA 
                || CurrentAnim == (int)JuneAnimType.DieB)
            {
                return;
            }
            
            int index = Random.Range(0, 2);

            if (index == 0)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.DieA);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.DieB);
            }
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }
            
            base.IdleAnim();
            
            if (CurrentAnim == (int)JuneAnimType.Damage_wing)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)JuneAnimType.Idle_A 
                || CurrentAnim == (int)JuneAnimType.Idle_B 
                || CurrentAnim == (int)JuneAnimType.IdleE
                || CurrentAnim == (int)JuneAnimType.IdleF
                || CurrentAnim == (int)JuneAnimType.IdleG_wing)
            {
                return;
            }

            int index = Random.Range(0, 5);

            switch (index)
            {
                case 0:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Idle_A);
                    break;
                case 1:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Idle_B);
                    break;
                case 2:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.IdleE);
                    break;
                case 3:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.IdleF);
                    break;
                default:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.IdleG_wing);
                    break; 
            }
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }
            
            base.AttackAnim();

            if (CurrentAnim == (int)JuneAnimType.Atk_A1_wing
                || CurrentAnim == (int)JuneAnimType.Atk_A2_wing
                || CurrentAnim == (int)JuneAnimType.Atk_A3_wing
                || CurrentAnim == (int)JuneAnimType.Atk1
                || CurrentAnim == (int)JuneAnimType.Atk2
                || CurrentAnim == (int)JuneAnimType.Atk3
                || CurrentAnim == (int)JuneAnimType.Atk4
                || CurrentAnim == (int)JuneAnimType.Damage_wing)
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
                    StartAnimationWithReturnIdle(JuneAnimType.Atk_A1_wing);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(JuneAnimType.Atk_A2_wing);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(JuneAnimType.Atk_A3_wing);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(JuneAnimType.Atk1);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(JuneAnimType.Atk2);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(JuneAnimType.Atk3);
                    break;
                default:
                    StartAnimationWithReturnIdle(JuneAnimType.Atk4);
                    break; 
            }
        }

        protected override void HitAnim()
        {
            if (IsDeath)
            {
                return;
            }
            
            base.HitAnim();

            if (CurrentAnim == (int)JuneAnimType.Damage_wing)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            StartAnimationWithReturnIdle(JuneAnimType.Damage_wing);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.RunA_L_wing);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.RunA_R_wing);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.RunA_wing);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Walk_L);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Walk_R);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Walk_back);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.Walk);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(JuneAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)JuneAnimType.IdleG_wing);
        }

    }
}
