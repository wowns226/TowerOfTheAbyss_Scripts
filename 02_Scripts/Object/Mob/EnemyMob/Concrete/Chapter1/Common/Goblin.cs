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
    public enum GoblinAnimType
    {
        IdleSwordShield = 1,
        WalkForwardSwordShield = 5,
        WalkBackwardsSwordShield = 6,
        StrafeLeftSwordShield = 7,
        StrafeRightSwordShield = 8,
        RunSwordShield = 9,
        DeathSwordShield = 13,
        GetHitSwordShield = 17,
        Hit2ComboSwordShield = 21,
        Hit3ComboSwordShield = 22,
        Attack1SwordShield = 23,
        Attack2SwordShield = 24,
        Attack3SwordShield = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Common)]
    public class Goblin : EnemyMob
    {
        //골드 잭팟
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.IdleSwordShield);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)GoblinAnimType.DeathSwordShield)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.DeathSwordShield);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)GoblinAnimType.GetHitSwordShield)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)GoblinAnimType.IdleSwordShield)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.IdleSwordShield);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)GoblinAnimType.Hit2ComboSwordShield
                || CurrentAnim == (int)GoblinAnimType.Hit3ComboSwordShield
                || CurrentAnim == (int)GoblinAnimType.Attack1SwordShield
                || CurrentAnim == (int)GoblinAnimType.Attack2SwordShield
                || CurrentAnim == (int)GoblinAnimType.Attack3SwordShield
                || CurrentAnim == (int)GoblinAnimType.GetHitSwordShield)
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
                    StartAnimationWithReturnIdle(GoblinAnimType.Hit3ComboSwordShield);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(GoblinAnimType.Hit2ComboSwordShield);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(GoblinAnimType.Attack1SwordShield);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(GoblinAnimType.Attack2SwordShield);
                    break;
                default:
                    StartAnimationWithReturnIdle(GoblinAnimType.Attack3SwordShield);
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

            if (CurrentAnim == (int)GoblinAnimType.GetHitSwordShield)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(GoblinAnimType.GetHitSwordShield);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.StrafeLeftSwordShield);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.StrafeRightSwordShield);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.WalkBackwardsSwordShield);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.RunSwordShield);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.StrafeLeftSwordShield);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.StrafeRightSwordShield);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.WalkBackwardsSwordShield);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.WalkForwardSwordShield);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(GoblinAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)GoblinAnimType.IdleSwordShield);
        }

    }
}
