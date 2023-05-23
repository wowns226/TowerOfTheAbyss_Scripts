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
    public enum CrassorridAnimType
    {
        Idle = 1,
        WalkForward = 5,
        WalkBackwards = 6,
        WalkLeft = 7,
        WalkRight = 8,
        Run = 9,
        Death = 13,
        GetHitFront = 17,
        HitCombo2 = 21,
        HitCombo1 = 22,
        ClawsAttackRight1 = 23,
        ClawsAttackRight2 = 24,
        SmashAttack = 25,
        ClawsAttackLeft1 = 26,
        ClawsAttackLeft2 = 27,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Boss)]
    public class Crassorrid : EnemyMob, IEatingUnit
    {
        //유닛 소화
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        private Unit eatUnit;
        private bool isEating;
        public bool IsEating
        {
            get => isEating; 
            set
            {
                if (value)
                {
                    transform.localScale = Vector3.one * 1.2f;
                    Speed -= 1f;
                }
                else
                {
                    transform.localScale = Vector3.one;
                    Speed = BaseSpeed;
                }

                if (eatUnit is not null)
                {
                    eatUnit.IsEaten = value;
                    eatUnit.gameObject.SetActive(!value);
                }
                
                highlightEffect.highlighted = value;
                isEating = value;
            }
        }

        private Coroutine eatCoroutine;

        public void Eat(Unit unit, float digestionTime)
        {
            eatUnit = unit;
            IsEating = true;
            eatUnit.Eaten();
            
            if (eatCoroutine != null)
            {
                StopCoroutine(eatCoroutine);
                eatCoroutine = null;
            }
            
            eatCoroutine = StartCoroutine(_Eat(unit, digestionTime));
        }

        IEnumerator _Eat(Unit unit, float digestionTime)
        {
            Debug.Log($"Crassorrid.Eat(), Start EatUnit : {unit.name}, DigestionTime : {digestionTime}");
            yield return new WaitForSeconds(digestionTime);

            eatUnit.gameObject.SetActive(true);
            eatUnit.ChangeBasePoint(BasePoint);
            eatUnit.Kill();
            
            IsEating = false;
            eatUnit = null;
            
            Debug.Log($"Crassorrid.Eat(), End EatUnit : {unit.name}, DigestionTime : {digestionTime}");
        }

        private void StopEat()
        {
            Debug.Log($"Crassorrid.StopEat(), End EatUnit : {eatUnit.name}");
            
            eatUnit.ChangeBasePoint(BasePoint);
            
            if (eatCoroutine != null)
            {
                StopCoroutine(eatCoroutine);
                eatCoroutine = null;
            }
            
            IsEating = false;
        }
        
        public override void Stun(float time)
        {
            base.Stun(time);

            if (IsEating)
            {
                StopEat();
            }
        }

        public override void Spawn(Point spawnPoint)
        {
            base.Spawn(spawnPoint);
            
            IsEating = false;
        }

        public override void Death()
        {
            base.Death();

            if (IsEating)
            {
                StopEat();
            }
        }

        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.Idle);
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)CrassorridAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)CrassorridAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)CrassorridAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();
            
            if (CurrentAnim == (int)CrassorridAnimType.HitCombo2
                || CurrentAnim == (int)CrassorridAnimType.HitCombo1
                || CurrentAnim == (int)CrassorridAnimType.ClawsAttackRight1
                || CurrentAnim == (int)CrassorridAnimType.ClawsAttackRight2
                || CurrentAnim == (int)CrassorridAnimType.SmashAttack
                || CurrentAnim == (int)CrassorridAnimType.ClawsAttackLeft1
                || CurrentAnim == (int)CrassorridAnimType.ClawsAttackLeft2
                || CurrentAnim == (int)CrassorridAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(CrassorridAnimType.HitCombo2);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(CrassorridAnimType.HitCombo1);
                    break;
                case 2:
                    StartAnimationWithReturnIdle(CrassorridAnimType.ClawsAttackRight1);
                    break;
                case 3:
                    StartAnimationWithReturnIdle(CrassorridAnimType.ClawsAttackRight2);
                    break;
                case 4:
                    StartAnimationWithReturnIdle(CrassorridAnimType.SmashAttack);
                    break;
                case 5:
                    StartAnimationWithReturnIdle(CrassorridAnimType.ClawsAttackLeft1);
                    break;
                default:
                    StartAnimationWithReturnIdle(CrassorridAnimType.ClawsAttackLeft2);
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

            if (CurrentAnim == (int)CrassorridAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(CrassorridAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.WalkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(CrassorridAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)CrassorridAnimType.Idle);
        }

    }
}
