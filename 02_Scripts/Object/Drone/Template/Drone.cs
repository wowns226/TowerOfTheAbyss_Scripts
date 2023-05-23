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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public abstract class Drone : MonoBehaviour, ISetting, IUnitState
    {
        protected Player player;
        
        protected UnitState<Drone> state;
        
        [SettingValue]
        protected float speed;

        public Action<Vector3> onMoved;
        public Action<Quaternion> onRotated;

        public AttackType AttackType => AttackType.Ranged;
        
        public bool IsSplineMove => false;
        public Point BattlePoint => TargetPoint;

        protected Point basePoint;
        public Point BasePoint => basePoint;

        protected Point targetPoint;
        public Point TargetPoint
        {
            get => targetPoint;
            set => targetPoint = value;
        }

        public Vector3 TargetPos => TargetPoint.Position;

        public bool IsNearestUnitInAttackRange => true;
        public Unit NearestUnit => BattlePoint.GetEnemyMobs().OrderBy(unit => Vector3.Distance(transform.position, unit.transform.position) - unit.Scale.x * 0.4f).First();

        private void Start()
        {
            Init();
        }

        private void Clear()
        {
            Debug.Log($"Drone.Clear(), Name : {name}");

            StopAllCoroutines();

            basePoint = null;
            targetPoint = null;

            onMoved = null;
            onRotated = null;
        }

        private void Update()
        {
            if (state == null)
            {
                return;
            }

            state.Execute(this);
        }

        public virtual void Spawn(Point spawnPoint)
        {
            this.SetValue();
            
            basePoint = spawnPoint;
            gameObject.SetActive(true);
            Vector3 spawnPos = spawnPoint.Position;
            transform.position = new Vector3(spawnPos.x, 5f, spawnPos.z); // 드론은 y고정

            BasePoint.SetDrone(this);
        }

        protected virtual void Init()
        {
            D.SelfPlayer.AddDrone(this);
            player = D.SelfPlayer;
            InitState();
        }

        public virtual void InitState()
        {
            state = new Idle<Drone>();
        }

        public void SetTargetPoints(Queue<Point> targets)
        {
            TargetPoint = targets.Last();
        }

        public void SetDestinationPoint(Point point)
        {
            TargetPoint = point;
        }

        public virtual void UseIndividuality()
        {
        }

        public void Idle()
        {

        }

        public void Attack()
        {
            //아직 공격 드론은 미구현
        }

        public void MoveToTarget()
        {
            if (IsReachPoint())
                ReachPoint();
            
            var position = transform.position;
            position = Vector3.MoveTowards(position, TargetPos, speed * Time.deltaTime);
            transform.position = position;
            onMoved?.Invoke(position);
        }
        
        private bool IsReachPoint() => IsInnerTargetPos(transform.position);

        private void ReachPoint()
        {
            basePoint.UnSetDrone(this);
            targetPoint.SetDrone(this);

            SetBasePoint();
        }

        private void SetBasePoint()
        {
            basePoint = targetPoint;
        }
        
        private bool IsInnerTargetPos(Vector3 position)
        {
            float offset = Point.POINT_SCALE * 0.1f;

            float minX = TargetPos.x - offset;
            float maxX = TargetPos.x + offset;

            float minZ = TargetPos.z - offset;
            float maxZ = TargetPos.z + offset;

            if (IsInnerCoordinate(position.x, minX, maxX) && IsInnerCoordinate(position.z, minZ, maxZ))
                return true;

            return false;
        }

        private bool IsInnerCoordinate(float x, float minX, float maxX)
        {
            if (minX < x && x < maxX)
                return true;

            return false;
        }
        
        public void RotateXZ(Vector3 targetPos)
        {
            var targetPosXZ = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            Rotate(targetPosXZ);
        }
        
        public void Rotate(Vector3 targetPos)
        {
            var rotation = transform.rotation;
            rotation = Quaternion.Lerp(rotation, Quaternion.LookRotation(targetPos - transform.position), Time.deltaTime * 2);
            transform.rotation = rotation;
            onRotated?.Invoke(rotation);
        }

        public void RotateImmediately(Vector3 targetPos)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
            onRotated?.Invoke(transform.rotation);
        }

        public void Death()
        {
            D.SelfPlayer.RemoveDrone(this);
            BasePoint.UnSetDrone(this);
            Clear();

            ObjectPoolManager.Instance.Return(this);
        }

        public void MoveToNearestBattleUnit() 
        {
            Vector3.MoveTowards(transform.position, BattlePoint.Position, speed * Time.deltaTime);
        }
    }
}
