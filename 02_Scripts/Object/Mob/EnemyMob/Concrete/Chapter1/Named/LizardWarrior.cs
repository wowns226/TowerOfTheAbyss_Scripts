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
    public enum LizardWarriorAnimType
    {
        IdleSpear = 1,
        WalkSpear = 5,
        StrafeLeftSpear = 7,
        StrafeRightSpear = 8,
        RunSpear = 9,
        DeathSpear = 13,
        GetHit1Spear = 17,
        Hit2ComboSpear = 21,
        Hit3ComboSpear = 22,
        Attack1Spear = 23,
        Attack2Spear = 24,
        Attack3Spear = 25,
        TongueAttackSpear = 26,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Named)]
    public class LizardWarrior : EnemyMob
    {
        //잭팟
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.IdleSpear);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)LizardWarriorAnimType.DeathSpear)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.DeathSpear);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)LizardWarriorAnimType.GetHit1Spear)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)LizardWarriorAnimType.IdleSpear)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.IdleSpear);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)LizardWarriorAnimType.Hit2ComboSpear
                || CurrentAnim == (int)LizardWarriorAnimType.Hit3ComboSpear
                || CurrentAnim == (int)LizardWarriorAnimType.Attack1Spear
                || CurrentAnim == (int)LizardWarriorAnimType.Attack2Spear
                || CurrentAnim == (int)LizardWarriorAnimType.Attack3Spear
                || CurrentAnim == (int)LizardWarriorAnimType.TongueAttackSpear
                || CurrentAnim == (int)LizardWarriorAnimType.GetHit1Spear)
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
                    StartAnimationWithReturnIdle(LizardWarriorAnimType.Hit2ComboSpear);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(LizardWarriorAnimType.Hit3ComboSpear);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(LizardWarriorAnimType.Attack1Spear);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(LizardWarriorAnimType.Attack2Spear);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(LizardWarriorAnimType.Attack3Spear);
                    break;
                default:
                    StartAnimationWithReturnIdle(LizardWarriorAnimType.TongueAttackSpear);
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

            if (CurrentAnim == (int)LizardWarriorAnimType.GetHit1Spear)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(LizardWarriorAnimType.GetHit1Spear);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.StrafeLeftSpear);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.StrafeRightSpear);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.WalkSpear);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.RunSpear);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.StrafeLeftSpear);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.StrafeRightSpear);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.WalkSpear);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.WalkSpear);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(LizardWarriorAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)LizardWarriorAnimType.IdleSpear);
        }

    }
}
