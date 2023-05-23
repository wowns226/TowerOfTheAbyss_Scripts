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
    public enum MountainDragonAnimType
    {
        FlyStationary = 1,
        FlyNormal = 5,
        DeathHitTheGround = 13,
        FlyStationaryGetHit = 17,
        FlyStationarySpreadFire = 21,
        FlyStationarySpitFireBall = 22,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Boss)]
    public class MountainDragon : EnemyMob
    {
        //브레스, 장판 , 근접 50 , 원거리 130
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)MountainDragonAnimType.FlyStationary);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)MountainDragonAnimType.DeathHitTheGround)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)MountainDragonAnimType.DeathHitTheGround);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)MountainDragonAnimType.FlyStationaryGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)MountainDragonAnimType.FlyStationary)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)MountainDragonAnimType.FlyStationary);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)MountainDragonAnimType.FlyStationarySpreadFire
                || CurrentAnim == (int)MountainDragonAnimType.FlyStationarySpitFireBall
                || CurrentAnim == (int)MountainDragonAnimType.FlyStationaryGetHit)
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
                    StartAnimationWithReturnIdle(MountainDragonAnimType.FlyStationarySpreadFire);
                    break;
                default:
                    StartAnimationWithReturnIdle(MountainDragonAnimType.FlyStationarySpitFireBall);
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

            if (CurrentAnim == (int)MountainDragonAnimType.FlyStationaryGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(MountainDragonAnimType.FlyStationaryGetHit);
        }

        protected override void RunAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.RunAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)MountainDragonAnimType.FlyNormal);
        }

        protected override void WalkAnim(bool isLeft, bool isBack, bool isSide)
        {
            if (IsDeath)
            {
                return;
            }

            base.WalkAnim(isLeft, isBack, isSide);

            unitAnimator?.SetInteger(MOTION_KEY, (int)MountainDragonAnimType.FlyNormal);
        }
        
        
        private void StartAnimationWithReturnIdle(MountainDragonAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)MountainDragonAnimType.FlyStationary);
        }

    }
}
