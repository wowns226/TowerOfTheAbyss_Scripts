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
    public enum DragonideAnimType
    {
        IdleWeapon = 1,
        WalkWeapon = 5,
        StrafeLWeapon = 7,
        StrafeRWeapon = 8,
        RunWeapon = 9,
        DeathWeapon = 13,
        GetHitWeapon = 17,
        Hit2ComboSpecialWeapon = 21,
        Hit2ComboWeapon = 22,
        Hit3ComboWeapon = 23,
        Hit4ComboWeapon = 24,
        SpinningTailAttack = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Named)]
    public class Dragonide : OnlyBuildingAttackEnemyMob
    {
        //타워만 공격
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.IdleWeapon);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)DragonideAnimType.DeathWeapon)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.DeathWeapon);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)DragonideAnimType.GetHitWeapon)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)DragonideAnimType.IdleWeapon)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.IdleWeapon);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)DragonideAnimType.Hit2ComboSpecialWeapon
                || CurrentAnim == (int)DragonideAnimType.Hit2ComboWeapon
                || CurrentAnim == (int)DragonideAnimType.Hit3ComboWeapon
                || CurrentAnim == (int)DragonideAnimType.Hit4ComboWeapon
                || CurrentAnim == (int)DragonideAnimType.SpinningTailAttack
                || CurrentAnim == (int)DragonideAnimType.GetHitWeapon)
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
                    StartAnimationWithReturnIdle(DragonideAnimType.Hit2ComboSpecialWeapon);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(DragonideAnimType.Hit2ComboWeapon);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(DragonideAnimType.Hit3ComboWeapon);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(DragonideAnimType.Hit4ComboWeapon);
                    break;
                default:
                    StartAnimationWithReturnIdle(DragonideAnimType.SpinningTailAttack);
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

            if (CurrentAnim == (int)DragonideAnimType.GetHitWeapon)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(DragonideAnimType.GetHitWeapon);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.StrafeLWeapon);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.StrafeRWeapon);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.WalkWeapon);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.RunWeapon);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.StrafeLWeapon);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.StrafeRWeapon);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.WalkWeapon);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.WalkWeapon);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(DragonideAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)DragonideAnimType.IdleWeapon);
        }

    }
}
