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
    public enum DemonLordAnimType
    {
        Idle = 1,
        Walk = 5,
        StrafeLeft = 7,
        StrafeRight = 8,
        Death = 13,
        GetHit1 = 17,
        Hit2Combo = 21,
        Hit3Combo = 22,
        WhipAttack = 23,
        SwordAttack1 = 24,
        SwordAttack2 = 25,
    }

    [MobType(ChapterType.ElfCastle, GradeType.Common, MobType.Boss)]
    public class DemonLord : EnemyMob
    {
        //10초에 한번씩 common 유닛 생성, 죽을 때 같이 죽음
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)DemonLordAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)DemonLordAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)DemonLordAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)DemonLordAnimType.Hit2Combo
                || CurrentAnim == (int)DemonLordAnimType.Hit3Combo
                || CurrentAnim == (int)DemonLordAnimType.WhipAttack
                || CurrentAnim == (int)DemonLordAnimType.SwordAttack1
                || CurrentAnim == (int)DemonLordAnimType.SwordAttack2
                || CurrentAnim == (int)DemonLordAnimType.GetHit1)
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
                    StartAnimationWithReturnIdle(DemonLordAnimType.Hit2Combo);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(DemonLordAnimType.Hit3Combo);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(DemonLordAnimType.SwordAttack1);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(DemonLordAnimType.SwordAttack2);
                    break;
                default:
                    StartAnimationWithReturnIdle(DemonLordAnimType.WhipAttack);
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

            if (CurrentAnim == (int)DemonLordAnimType.GetHit1)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(DemonLordAnimType.GetHit1);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.StrafeLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.StrafeRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Walk);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.StrafeLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.StrafeRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Walk);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Walk);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(DemonLordAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)DemonLordAnimType.Idle);
        }

    }
}
