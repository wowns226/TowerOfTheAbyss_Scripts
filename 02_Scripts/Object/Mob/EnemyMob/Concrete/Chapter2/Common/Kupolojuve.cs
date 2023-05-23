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
    public enum KupolojuveAnimType
    {
        Idle = 1,
        walkForward = 5,
        walkBackwards = 6,
        walkLeft = 7,
        walkRight = 8,
        FlyForward = 9,
        FlyBackwards = 10,
        FlyLeft = 11,
        FlyRight = 12,
        Death = 13,
        Death2 = 14,
        FlyGetHit = 17,
        JumpClawsAttack = 21,
        DashSpikeAttack = 22,
        FlyClawsAttack = 23,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Common)]
    public class Kupolojuve : EnemyMob
    {
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)KupolojuveAnimType.Death
                || CurrentAnim == (int)KupolojuveAnimType.Death2)
            {
                return;
            }
            
            int index = Random.Range(0, 2);

            switch (index)
            {
                case 0:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.Death);
                    break;
                default:
                    unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.Death2);
                    break;
            }
            
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)KupolojuveAnimType.FlyGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)KupolojuveAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)KupolojuveAnimType.JumpClawsAttack
                || CurrentAnim == (int)KupolojuveAnimType.DashSpikeAttack
                || CurrentAnim == (int)KupolojuveAnimType.FlyClawsAttack
                || CurrentAnim == (int)KupolojuveAnimType.FlyGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            int index = Random.Range(0, 3);

            switch (index)
            {
                case 0:
                    StartAnimationWithReturnIdle(KupolojuveAnimType.JumpClawsAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(KupolojuveAnimType.DashSpikeAttack);
                    break;
                default:
                    StartAnimationWithReturnIdle(KupolojuveAnimType.FlyClawsAttack);
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

            if (CurrentAnim == (int)KupolojuveAnimType.FlyGetHit)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(KupolojuveAnimType.FlyGetHit);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.FlyLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.FlyRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.FlyBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.FlyForward);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.walkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.walkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.walkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.walkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(KupolojuveAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)KupolojuveAnimType.Idle);
        }

    }
}
