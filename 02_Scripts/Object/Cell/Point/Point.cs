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
using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace ProjectL
{
    /// <summary>
    /// 상위 State가 하위 State를 덮어쓰는 구조(ex. NotMove, Moved 동시 설정 시 Moved 로 판단이 됨)
    /// </summary>
    [Flags]
    public enum PointState
    {
        None = 1 << 0,
        Buff = 1 << 1,
        DeBuff = 1 << 2,
        NotMove = 1 << 3,
        Move = 1 << 4,
        Moved = 1 << 5,
    }

    #region 포인트 간 거리 계산 클래스

    [Serializable]
    public class PointDistanceData
    {
        public float distance;
        public Point point;
    }

    #endregion

    #region Node class
    [Serializable]
    public class Node
    {
        public Node(Point connectedPoint, float weight)
        {
            this.connectedPoint = connectedPoint;
            this.weight = weight;
        }

        [SerializeField]
        private Point connectedPoint;
        public Point ConnectedPoint => connectedPoint;

        [SerializeField]
        private float weight;
        public float Weight => weight;
        public float RandomWeight => weight * Random.Range(0.8f, 1.2f); 

        public PathCreator path;
    }
    #endregion

    public class Point : DataContainer, IPointerEnterHandler, IPointerUpHandler
    {

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Position, 1);
        }
#endif

        public const int POINT_SCALE = 5;

        private List<Unit> allyMobs = new List<Unit>();
        private List<Unit> enemyMobs = new List<Unit>();
        private List<Unit> buildings = new List<Unit>();
        private List<Drone> drones = new List<Drone>();

        private Dictionary<int, Action<Unit>> activeBuffs = new Dictionary<int, Action<Unit>>();
        private Dictionary<int, Action<Unit>> inActiveBuffs = new Dictionary<int, Action<Unit>>();

        [SerializeField]
        private List<PointDistanceData> distanceDatas = new List<PointDistanceData>();
        public List<PointDistanceData> DistanceDatas => distanceDatas;
        public Dictionary<Point, float> distanceDic = new Dictionary<Point, float>();

        [SerializeField]
        private List<Node> nodes = new List<Node>();
        public List<Node> Nodes => nodes;

        public Vector3 Position { get; private set; }

        public Vector3 RandomPos
        {
            get
            {
                float weight = POINT_SCALE * 0.3f;
                float x = Position.x + Random.Range(-weight, weight);
                float z = Position.z + Random.Range(-weight, weight);

                return new Vector3(x, Position.y, z);
            }
        }

        public bool IsBuilding => buildings.Count != 0;

        public bool IsCastle => buildings.Any(building => building is Castle);

        [SerializeField]
        private bool isCastleSpawn;
        public bool IsCastleSpawn => isCastleSpawn;

        [SerializeField]
        private bool isWall;
        public bool IsWall => isWall;

        [SerializeField]
        private int depth;
        public int Depth => depth;

        [SerializeField]
        private bool isAllySpawn;
        public bool IsAllySpawn => isAllySpawn;

        [SerializeField]
        private bool isEnemySpawn;
        public bool IsEnemySpawn => isEnemySpawn;

        private bool isBlockSpawn;
        public bool IsBlockSpawn { set => isBlockSpawn = value; get => isBlockSpawn; }

        private float spawnTime;
        [SerializeField]
        private float spawnDelay;
        public bool AvailableSpawn => Time.time - spawnTime > spawnDelay && IsNotExistAllyUnit && IsBlockSpawn == false;

        public bool IsNotExistAllyUnit => allyMobs.Count == 0;

        private bool isAllyDestinationPoint;
        public bool IsAllyDestinationPoint { set => isAllyDestinationPoint = value; get => isAllyDestinationPoint; }

        public bool IsBlocking { get; set; }
        public bool IsAvailableMove => IsAllyDestinationPoint == false && allyMobs.Count == 0 && IsWall == false && IsBlocking == false;

        private PointState state;

        private PointState State
        {
            get => state;
            set
            {
                state = value;
                this.NotifyObserver();
            }
        }

        [DataObservable]
        public bool IsMove => IsCurrentState(PointState.Move);
        [DataObservable]
        public bool IsMoved => IsCurrentState(PointState.Moved);
        [DataObservable]
        public bool IsNotMoved => IsCurrentState(PointState.NotMove);
        [DataObservable]
        public bool IsBuff => IsCurrentState(PointState.Buff);
        [DataObservable]
        public bool IsDeBuff => IsCurrentState(PointState.DeBuff);

        protected override void Awake()
        {
            base.Awake();

            Position = transform.position;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            State = PointState.None;
            SetDistanceDic();
        }

        private void SetDistanceDic()
        {
            distanceDic = distanceDatas.ToDictionary(distanceData => distanceData.point, distanceData => distanceData.distance);
        }

        public List<Unit> GetUnits() => GetAllyMobs().Concat(GetEnemyMobs()).Concat(GetBuildings()).ToList();
        public List<Unit> GetAllys() => GetAllyMobs().Concat(GetBuildings()).ToList();
        public List<Unit> GetAllyMobs() => allyMobs;
        public List<Unit> GetNotFullHpAllyMobs() => GetAllyMobs().FindAll(mob => mob.Hp < mob.MaxHp);
        public List<Unit> GetEnemyMobs() => enemyMobs;
        public List<Unit> GetNotFullHpEnemyMobs() => GetEnemyMobs().FindAll(mob => mob.Hp < mob.MaxHp);
        public List<Unit> GetBuildings() => buildings;
        public List<Unit> GetNotFullHpAllyBuildings() => GetBuildings().FindAll(mob => mob.Hp < mob.MaxHp);
        public List<Drone> GetDrones() => drones;

        public void SpawnMyMob(Unit mob)
        {
            Debug.Log($"Point.SpawnMyMob(), Unit : {mob.name}");
            spawnTime = Time.time;
            SetMyMob(mob);
        }
        public void SetMyMob(Unit mob)
        {
            Debug.Log($"Point.SetMyMob(), Unit : {mob.name}");
            
            if (allyMobs.Contains(mob))
            {
                Debug.Log($"Point.SetMyMob() return already unit, Unit : {mob.name}");
                return;
            }
            
            allyMobs.Add(mob);

            foreach (var buff in activeBuffs)
            {
                buff.Value?.Invoke(mob);
            }
        }
        public void UnSetMyMob(Unit mob)
        {
            Debug.Log($"Point.UnSetMyMob(), Unit : {mob.name}");
            
            if (allyMobs.Contains(mob) == false)
            {
                Debug.Log($"Point.UnSetMyMob() return is not exist unit, Unit : {mob.name}");
                return;
            }
            
            allyMobs.Remove(mob);

            foreach (var buff in inActiveBuffs)
            {
                buff.Value?.Invoke(mob);
            }
        }

        public void SpawnEnemyMob(Unit mob)
        {
            Debug.Log($"Point.SpawnEnemyMob(), Unit : {mob.name}");
            spawnTime = Time.time;
            SetEnemyMob(mob);
        }
        public void SetEnemyMob(Unit mob)
        {
            Debug.Log($"Point.SetEnemyMob(), Unit : {mob.name}");
            
            if (enemyMobs.Contains(mob))
            {
                Debug.Log($"Point.SetEnemyMob() return already unit, Unit : {mob.name}");
                return;
            }
            
            enemyMobs.Add(mob);

            foreach (var buff in activeBuffs)
            {
                buff.Value?.Invoke(mob);
            }
        }
        public void UnSetEnemyMob(Unit mob)
        {
            Debug.Log($"Point.UnSetEnemyMob(), Unit : {mob.name}");

            if (enemyMobs.Contains(mob) == false)
            {
                Debug.Log($"Point.UnSetEnemyMob() return is not exist unit, Unit : {mob.name}");
                return;
            }

            enemyMobs.Remove(mob);

            foreach (var buff in inActiveBuffs)
            {
                buff.Value?.Invoke(mob);
            }
        }

        public void SetBuilding(Unit building)
        {
            Debug.Log($"Point.SetBuilding(), Unit : {building.name}");
            
            if (buildings.Contains(building))
            {
                Debug.Log($"Point.SetBuilding() return already unit, building : {building.name}");
                return;
            }

            buildings.Add(building);

            foreach (var buff in activeBuffs)
            {
                buff.Value?.Invoke(building);
            }
        }
        public void UnSetBuilding(Unit building)
        {
            Debug.Log($"Point.UnSetBuilding(), Unit : {building.name}");
            
            if (buildings.Contains(building) == false)
            {
                Debug.Log($"Point.UnSetBuilding() return is not exist unit, building : {building.name}");
                return;
            }

            buildings.Remove(building);

            foreach (var buff in inActiveBuffs)
            {
                buff.Value?.Invoke(building);
            }
        }

        public void SetDrone(Drone drone) => drones.Add(drone);
        public void UnSetDrone(Drone drone) => drones.Remove(drone);

        public void AddActiveBuff(int eventKey, Action<Unit> activebuff, Action<Unit> inActivebuff)
        {
            if (activeBuffs.ContainsKey(eventKey))
            {
                Debug.Log($"Point.AddActiveBuff has same buff, Point : {name}, eventKey : {eventKey}");
                return;
            }

            GetUnits().ForEach(unit => activebuff(unit));
            activeBuffs.Add(eventKey, activebuff);
            inActiveBuffs.Add(eventKey, inActivebuff);
        }
        public void RemoveActiveBuff(int eventKey)
        {
            if (inActiveBuffs.ContainsKey(eventKey) == false)
            {
                Debug.Log($"Point.RemoveActiveBuff hasn't buff, Point : {name}, eventKey : {eventKey}");
                return;
            }

            GetUnits().ForEach(unit => inActiveBuffs[eventKey](unit));
            inActiveBuffs.Remove(eventKey);
            activeBuffs.Remove(eventKey);
        }
        public void ClearBuff() => GetUnits().ForEach(unit =>
        {
            foreach (var buff in inActiveBuffs)
            {
                buff.Value?.Invoke(unit);
            }
        });

        public PathCreator GetTargetPointPathCreator(Point target)
        {
            var targetNode = nodes.Find(node => node.ConnectedPoint == target);

            if(targetNode == null)
            {
                return null;
            }

            return targetNode.path;
        }

        public bool IsInnerPoint(Vector2 position, int minDepth, int maxDepth)
        {
            Vector3 pointPos = Position;

            float minX = pointPos.x - POINT_SCALE * 0.5f;
            float maxX = pointPos.x + POINT_SCALE * 0.5f;

            float minZ = pointPos.z - POINT_SCALE * 0.5f;
            float maxZ = pointPos.z + POINT_SCALE * 0.5f;

            if (IsInnerCoordinate(position.x, minX, maxX) && IsInnerCoordinate(position.y, minZ, maxZ) && Depth >= minDepth && Depth <= maxDepth)
                return true;

            return false;
        }

        private bool IsInnerCoordinate(float x, float minX, float maxX)
        {
            if (minX < x && x < maxX)
                return true;

            return false;
        }

        public bool IsCurrentState(PointState pointState) => State.HasFlag(pointState) && (int)State < (int)pointState << 1;
        public void SetState(PointState pointState)
        {
            if (State.HasFlag(pointState))
            {
                return;
            }

            State |= pointState;
        }
        public void UnSetState(PointState pointState)
        {
            if (State.HasFlag(pointState) == false)
            {
                return;
            }

            State ^= pointState;
        }
        public void ClearState()
        {
            State = PointState.None;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            D.SelfPoint = this;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            D.SelfPoint = null;
        }
    }
}
