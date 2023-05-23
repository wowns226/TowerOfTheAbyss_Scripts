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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectL
{
    public abstract class PlayerMob : Mob, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private Point previousPathTargetPoint;
        private DialogBase slowEffect;

        [SerializeField]
        private RangeEffect rangeEffect;

        protected override List<Unit> BattleUnits => AttackType == AttackType.Heal ? BattlePoint?.GetAllyMobs() : BattlePoint?.GetEnemyMobs();

        protected override void Start()
        {
            ownerType = OwnerType.My;
            
            base.Start();
        }

        public override void InitState()
        {
            state = new Move<Unit>(new Battle<Unit>(new Idle<Unit>()));
        }

        public override void Spawn(Point spawnPoint)
        {
            base.Spawn(spawnPoint);

            BasePoint.SpawnMyMob(this);

            ToggleRangeEffect(VisualManager.Instance.ShowUnitRange);
            VisualManager.Instance.onShowUnitRange.Add(ToggleRangeEffect);
        }

        public override void Death()
        {
            CancelPreviousDestination();
            
            IsInvincible = false;
            IsGuaranteedCritical = false;
            
            BasePoint?.UnSetMyMob(this);
            basePoint = null;
            
            targetPoint = null;
            TargetPoints.Clear();

            ClearBuffSkill();
            
            ToggleRangeEffect(false);
            VisualManager.Instance.onShowUnitRange.Remove(ToggleRangeEffect);

            StopAllCoroutines();
            
            base.Death();
        }

        protected override void _PostDeath()
        {
            Revive();
        }

        private void Revive()
        {
            Debug.Log($"PlayerMob, Revive(), Start , Unit : {this.name}");
            
            isSpawn = false;
            isDeath = false;
        }
        
        public override void Clear()
        {
            CancelPreviousDestination();
            ToggleRangeEffect(false);
            BasePoint?.UnSetMyMob(this);
            player.RemoveUnit(this);
            base.Clear();
        }

        protected override void PreSetTargetPoints(Queue<Point> targets)
        {
            base.PreSetTargetPoints(targets);

            CancelPreviousDestination();

            SetDestination(targets);
        }

        private void CancelPreviousDestination()
        {
            if (TargetPoints.Count > 0)
            {
                DestinationPoint.IsAllyDestinationPoint = false;
            }
            else if(TargetPoint != null)
            {
                TargetPoint.IsAllyDestinationPoint = false;
            }
        }

        private void SetDestination(Queue<Point> targets)
        {
            if (targets.Count > 0)
            {
                targets.Last().IsAllyDestinationPoint = true;
            }
        }

        protected override void ArrivalDestination()
        {
            base.ArrivalDestination();

            BasePoint.IsAllyDestinationPoint = false;
        }

        protected override Func<Point, Point, List<Point>> GetPathsFunc() => D.SelfBoard.GetShortestPath;
        
        protected override Point SearchAllyPoint()
        {
            Point allyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return allyPoint;

            allyPoint = D.SelfBoard.FindAllyPoint(basePoint, Range, IsMelee, this);

            if (allyPoint != null)
                return allyPoint;

            return allyPoint;
        }

        protected override Point SearchNotFullHpAllyUnitPoint()
        {
            Point allyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return allyPoint;

            allyPoint = D.SelfBoard.FindNotFullHpAllyUnitPoint(basePoint, Range, IsMelee, this);

            if (allyPoint != null)
                return allyPoint;

            return allyPoint;
        }
        
        protected override Point SearchEnemyPoint()
        {
            Point enemyPoint = null;

            if (D.SelfBoard == null || basePoint == null)
                return enemyPoint;

            enemyPoint = D.SelfBoard.FindEnemyPoint(basePoint, Range, IsMelee, this);

            if (enemyPoint != null)
                return enemyPoint;

            return enemyPoint;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"PlayerMob.OnPointerDown(), Unit : {name}");

            if (IsSplineMove)
            {
                Debug.Log($"PlayerMob.OnPointerDown(), return because SplieMove Unit : {name}");
                ReturnPointerValues();
                return;
            }

            D.SelfUnit = this;
            TimeManager.Instance.ChangeTimeScale(0.2f);
            highlightEffect.highlighted = true;
            DialogManager.Instance.OpenDialog("DlgSlowEffect", dialog => slowEffect = dialog);

            SetPointsMoveEffect();
        }

        private void SetPointsMoveEffect()
        {
            D.SelfBoard.SetAllPointStateEffect(PointState.NotMove);

            SetPathPointsMoveEffect();
        }

        private void SetPathPointsMoveEffect()
        {
            var pathPoints = D.SelfBoard.GetPossiblePath(BasePoint);

            foreach (var point in pathPoints)
            {
                if (point.IsAvailableMove)
                {
                    point.SetState(PointState.Move);
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"PlayerMob.OnDrag(), Unit : {name}");

            if (IsSplineMove)
            {
                Debug.Log($"PlayerMob.OnDrag(), return because SplieMove Unit : {name}");
                ReturnPointerValues();
                return;
            }

            if (D.SelfPoint == null)
            {
                //툴팁 만들어서 여기에 넣기
                return;
            }

            if (ReferenceEquals(D.SelfPoint, null) == false && D.SelfPoint.IsAvailableMove == false)
            {
                //툴팁 만들어서 여기에 넣기 return은 하지말기
            }

            var targetPoint = D.SelfPoint;

            if (previousPathTargetPoint != null && previousPathTargetPoint == targetPoint)
            {
                return;
            }

            previousPathTargetPoint = targetPoint;

            SetPathEffect(targetPoint);
        }

        private void SetPathEffect(Point targetPoint)
        {
            var paths = D.SelfBoard.GetShortestPath(BasePoint, targetPoint);
            var allPoints = D.SelfBoard.points;

            foreach (var point in allPoints)
            {
                if (paths.Any(pathPoint => pathPoint.Equals(point)))
                {
                    point.SetState(PointState.Moved);
                }
                else
                {
                    point.UnSetState(PointState.Moved);
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log($"PlayerMob.OnPointerUp(), Unit : {name}");

            if (IsSplineMove)
            {
                Debug.Log($"PlayerMob.OnPointerUp(), return because SplieMove Unit : {name}");
                ReturnPointerValues();
                return;
            }

            if (D.SelfPoint == null)
            {
                Debug.Log($"PlayerMob.OnPointerUp(), return because D.selfPoint == null Unit : {name}");
                ReturnPointerValues();
                return;
            }

            if (D.SelfPoint.IsAvailableMove == false)
            {
                Debug.Log($"PlayerMob.OnPointerUp(), return because Point is NotMove Unit : {name}");
                ReturnPointerValues();
                return;
            }

            var targetPoint = D.SelfPoint;

            if (targetPoint != null)
            {
                SetDestinationPoint(targetPoint);
            }

            ReturnPointerValues();
        }

        private void ReturnPointerValues()
        {
            TimeManager.Instance.ReturnTimeScale();
            highlightEffect.highlighted = false;
            previousPathTargetPoint = null;
            D.SelfPoint = null;
            D.SelfBoard.UnSetAllPointStateEffect(PointState.Move);
            D.SelfBoard.UnSetAllPointStateEffect(PointState.Moved);
            D.SelfBoard.UnSetAllPointStateEffect(PointState.NotMove);

            if (slowEffect)
            {
                slowEffect.CloseDialog();
            }
        }

        private void ToggleRangeEffect(bool isOn) => rangeEffect?.Toggle(this, isOn, Range);

    }
}
