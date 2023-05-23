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
    public enum UndeadAnimType
    {
        idleNormal = 1,
        walkSlow = 5,
        walkWeapon = 9,
        death1 = 13,
        getHit1Normal = 17,
        Hit2ComboWeapon = 21,
        Hit3ComboWeapon = 22,
        attack1Weapon = 23,
        attack2Weapon = 24,
        attack3Weapon = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Common)]
    public class Undead : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.idleNormal);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)UndeadAnimType.death1)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.death1);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)UndeadAnimType.getHit1Normal)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)UndeadAnimType.idleNormal)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.idleNormal);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)UndeadAnimType.Hit2ComboWeapon
                || CurrentAnim == (int)UndeadAnimType.Hit3ComboWeapon
                || CurrentAnim == (int)UndeadAnimType.attack1Weapon
                || CurrentAnim == (int)UndeadAnimType.attack2Weapon
                || CurrentAnim == (int)UndeadAnimType.attack3Weapon
                || CurrentAnim == (int)UndeadAnimType.getHit1Normal)
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
                    StartAnimationWithReturnIdle(UndeadAnimType.Hit2ComboWeapon);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(UndeadAnimType.Hit3ComboWeapon);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(UndeadAnimType.attack1Weapon);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(UndeadAnimType.attack2Weapon);
                    break;
                default:
                    StartAnimationWithReturnIdle(UndeadAnimType.attack3Weapon);
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

            if (CurrentAnim == (int)UndeadAnimType.getHit1Normal)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(UndeadAnimType.getHit1Normal);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.walkSlow);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.walkSlow);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.walkWeapon);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.walkSlow);
            }
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.walkSlow);
        }
        
        
        private void StartAnimationWithReturnIdle(UndeadAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)UndeadAnimType.idleNormal);
        }

    }
}
