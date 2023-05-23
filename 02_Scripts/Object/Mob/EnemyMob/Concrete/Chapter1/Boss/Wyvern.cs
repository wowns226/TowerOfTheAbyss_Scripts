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
    public enum WyvernAnimType
    {
        FlyStationary = 1,
        Fly = 5,
        DeathHitTheGround = 13,
        FlyStationaryGetHit = 17,
        FlyStationarySpitFireball = 21,
        FlyStationarySpreadFire = 22,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Boss)]
    public class Wyvern : OnlyBuildingAttackEnemyMob
    {
        //건물만 공격(성 + 방벽 등..), 근거리 피해 70% 원거리 150%, 기본 능력치 60%
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)WyvernAnimType.FlyStationary);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)WyvernAnimType.DeathHitTheGround)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)WyvernAnimType.DeathHitTheGround);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)WyvernAnimType.FlyStationaryGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)WyvernAnimType.FlyStationary)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)WyvernAnimType.FlyStationary);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)WyvernAnimType.FlyStationarySpitFireball
                || CurrentAnim == (int)WyvernAnimType.FlyStationarySpreadFire
                || CurrentAnim == (int)WyvernAnimType.FlyStationaryGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 2);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(WyvernAnimType.FlyStationarySpitFireball);
                    break;
                default:
                    StartAnimationWithReturnIdle(WyvernAnimType.FlyStationarySpreadFire);
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

            if (CurrentAnim == (int)WyvernAnimType.FlyStationaryGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(WyvernAnimType.FlyStationaryGetHit);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)WyvernAnimType.Fly);
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)WyvernAnimType.Fly);
        }
        
        
        private void StartAnimationWithReturnIdle(WyvernAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)WyvernAnimType.FlyStationary);
        }

    }
}
