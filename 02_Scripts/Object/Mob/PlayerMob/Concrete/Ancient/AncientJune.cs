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
    public enum JuneAnimType
    {
        Idle_A = 1,
        Idle_B = 2,
        Angpose = 3,
        Damage_wing = 4,
        DieA = 5,
        Walk = 6,
        Walk_L = 7,
        Walk_R = 8,
        Walk_back = 9,
        Run = 10,
        Run_L = 11,
        Run_R = 12,
        Jump = 13,
        DieB = 14,
        CuteA = 15,
        Bye = 16,
        Cry = 17,
        IdleE = 19,
        Rei = 20,
        Atk1 = 21,
        Atk2 = 22,
        Atk3 = 23,
        Atk4 = 24,
        IdleF = 25,
        CastspellB = 26,
        CastspellA = 27,
        CastspellC = 28,
        Thinking = 31,
        Atk_A1_wing = 32,
        Atk_A2_wing = 33,
        Atk_A3_wing = 34,
        Fly_idle = 35,
        Fly_narmalA = 36,
        Fly_narmalB = 37,
        Fly_speedup = 38,
        IdleG_wing = 39,
        PoseA_wing = 40,
        RunA_wing = 41,
        RunA_L_wing = 42,
        RunA_R_wing = 43,
    }
    
    [MobType(ChapterType.ElfCastle, GradeType.Ancient, MobType.Hero)]
    public class AncientJune : PlayerMob
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
