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
    public enum ScolokarckAnimType
    {
        Idle = 1,
        WalkForward = 5,
        WalkBackwards = 6,
        WalkLeft = 7,
        WalkRight = 8,
        Run = 9,
        Death = 13,
        GetHitFront = 17,
        ClawsAttack = 21,
        ClawsAttackForward = 22,
        JumpClawsAttack = 23,
    }

    [MobType(ChapterType.PassageOfTime, GradeType.Common, MobType.Boss)]
    public class Scolokarck : EnemyMob
    {
        //적 유닛 수만큼 체력 방어력 높은 보스 유닛 수당 방,체 * 0.1배
        private Coroutine returnIdleCoroutine;

        private const string MOTION_KEY = "animation";
        private int CurrentAnim => unitAnimator.GetInteger(MOTION_KEY);

        public override void Spawn(Point spawnPoint)
        {
            base.Spawn(spawnPoint);
            
            int enemyCount = D.SelfEnemyPlayer.units.Count;
            UpgradeStat(enemyCount);
            
            D.SelfEnemyPlayer.onAddUnit += OnAddUnit;
            D.SelfEnemyPlayer.onRemoveUnit += OnRemoveUnit;
        }

        private void OnAddUnit(Unit unit) => UpgradeStat(1);
        private void OnRemoveUnit(Unit unit) => UpgradeStat(-1);

        private void UpgradeStat(int count)
        {
            Upgrade(StatType.Hp, count);
            Upgrade(StatType.Defense, count);
        }
        
        protected override void SpawnAnim()
        {
            base.SpawnAnim();

            unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.Idle);
        }

        public override void Death()
        {
            base.Death();
            D.SelfEnemyPlayer.onAddUnit -= OnAddUnit;
            D.SelfEnemyPlayer.onRemoveUnit -= OnRemoveUnit;
        }

        protected override void DeathAnim()
        {
            base.DeathAnim();

            if (CurrentAnim == (int)ScolokarckAnimType.Death)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.Death);
        }

        protected override void IdleAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.IdleAnim();
            
            if (CurrentAnim == (int)ScolokarckAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }
            
            if (CurrentAnim == (int)ScolokarckAnimType.Idle)
            {
                return;
            }

            unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.Idle);
        }

        protected override void AttackAnim()
        {
            if (IsDeath)
            {
                return;
            }

            base.AttackAnim();

            if (CurrentAnim == (int)ScolokarckAnimType.ClawsAttack
                || CurrentAnim == (int)ScolokarckAnimType.ClawsAttackForward
                || CurrentAnim == (int)ScolokarckAnimType.JumpClawsAttack
                || CurrentAnim == (int)ScolokarckAnimType.GetHitFront)
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
                    StartAnimationWithReturnIdle(ScolokarckAnimType.ClawsAttack);
                    break;
                case 1:
                    StartAnimationWithReturnIdle(ScolokarckAnimType.ClawsAttackForward);
                    break;
                default:
                    StartAnimationWithReturnIdle(ScolokarckAnimType.JumpClawsAttack);
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

            if (CurrentAnim == (int)ScolokarckAnimType.GetHitFront)
            {
                if(unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    return;
                }
            }

            StartAnimationWithReturnIdle(ScolokarckAnimType.GetHitFront);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.Run);
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
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkLeft);
            }
            else if (isSide && !isLeft)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkRight);
            }
            else if (isBack)
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkBackwards);
            }
            else
            {
                unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.WalkForward);
            }
        }
        
        
        private void StartAnimationWithReturnIdle(ScolokarckAnimType animType)
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
            
            unitAnimator?.SetInteger(MOTION_KEY, (int)ScolokarckAnimType.Idle);
        }

    }
}
