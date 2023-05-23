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
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectL
{
    [Serializable]
    public class PointBoundData
    {
        [SerializeField]
        private List<Point> points;
        private Bounds bounds;

        public Vector3 Center => bounds.center;
        public Vector3 Extents => bounds.extents;
        
        public void Init()
        {
            if (points.Count == 0)
            {
                Debug.Log($"PointBoundData.Init(), points.Count == 0");
                return;
            }
        
            bounds = new Bounds(points[0].Position, points[0].transform.forward * Point.POINT_SCALE);

            if (points.Count > 1)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    var forward = points[i].transform.forward * Point.POINT_SCALE * 0.5f;
                    var up = points[i].transform.right * Point.POINT_SCALE * -0.5f;
                    var min = points[i].Position - forward - up;
                    var max = points[i].Position + forward + up;
                    bounds.Encapsulate(min);
                    bounds.Encapsulate(max);
                }
            }

            bounds.Expand(Vector3.up * 2);
            
            Debug.Log($"PointBoundData.Init(), bound center : {bounds.center}, size : {bounds.size}, extents : {bounds.extents}");
        }
        
        public bool Contains(Vector3 pos) => bounds.Contains(pos);
    }
    
    public class Board : MonoBehaviour
    {
        [SerializeField]
        private List<PointBoundData> bounds;

        private Navigation navigation;
        // 맵에 쓰일 포인트 리스트
        public List<Point> points;
        private Dictionary<(int x, int z), List<Point>> pointsBoundCache = new Dictionary<(int x, int z), List<Point>>();
        private Dictionary<(Vector2 pos, int minDepth, int maxDepth), Point> findPointsCache = new Dictionary<(Vector2 pos, int minDepth, int maxDepth), Point>();

        private Point castlePoint;
        public Point CastlePoint => castlePoint ??= points.Find(point => point.IsCastleSpawn);

        private List<Point> allySpawns = new List<Point>();
        public List<Point> AllySpawns
        {
            get
            {
                if (allySpawns.Count == 0)
                    allySpawns = points.FindAll(point => point.IsAllySpawn);

                return allySpawns;
            }
        }

        private List<Point> enemySpawns = new List<Point>();
        public List<Point> EnemySpawns
        {
            get
            {
                if (enemySpawns.Count == 0)
                    enemySpawns = points.FindAll(point => point.IsEnemySpawn);

                return enemySpawns;
            }
        }

        private void Awake()
        {
            D.SelfBoard = this;
            navigation = new Navigation();
            bounds.ForEach(data => data.Init());
        }

        public bool ContainsInPoint(Vector3 pos) => bounds.Any(data => data.Contains(pos));

        public Vector3? ContainsInPointCenter(Vector3 pos) =>
            bounds.FirstOrDefault(data => data.Contains(pos))?.Center;
        
        public List<Point> GetPossiblePath(Point start) => navigation.GetPossiblePath(start);
        public List<Point> GetShortestPath(Point start, Point end) => navigation.LoadShortestPath(start, end);
        public List<Point> GetRandomShortestPath(Point start, Point end) => navigation.LoadRandomPath(start, end);
        public List<Point> GetRandomCastlePath(Point start) => navigation.LoadRandomPath(start, CastlePoint);
        public List<List<Point>> GetPaths(Point start, Point end) => navigation.LoadPaths(start, end);
        public List<List<Point>> GetCastlePaths(Point start) => navigation.LoadPaths(start, CastlePoint);

        public List<Point> GetRandomPoint(int count)
        {
            var result = new List<Point>();

            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, points.Count);
                result.Add(points[index]);
            }

            return result;
        }

        public Point FindPoint(Vector3 pos, int minDepth = -100, int maxDepth = 100) => FindPoint(new Vector2(pos.x, pos.z), minDepth, maxDepth);
        public Point FindPoint(Vector2 pos, int minDepth = -100, int maxDepth = 100) //이 부분 캐싱이 너무 많이되어서 메모리 문제가 생긴다면 없애는것도 고려
        {
            if (findPointsCache.ContainsKey((pos, minDepth, maxDepth)))
            {
                return findPointsCache[(pos, minDepth, maxDepth)];
            }

            (int x, int z) roundPos = ((int)(Math.Round(pos.x * 0.1f) * 10f), (int)(Math.Round(pos.y * 0.1f) * 10f));

            if (pointsBoundCache.ContainsKey(roundPos) == false)
            {
                float range = Point.POINT_SCALE * 2f + 1f;
                var pointsBound = points.FindAll(point =>
                {
                    int x = (int)point.Position.x;
                    int z = (int)point.Position.z;

                    return ((x - roundPos.x) * (x - roundPos.x) + (z - roundPos.z) * (z - roundPos.z)) < range * range;
                });

                pointsBoundCache.Add(roundPos, pointsBound);
            }

            var point = pointsBoundCache[roundPos].Find(point => point.IsInnerPoint(pos, minDepth, maxDepth));
            findPointsCache.Add((pos, minDepth, maxDepth), point);
            
            return findPointsCache[(pos, minDepth, maxDepth)]; 
        }

        public Point FindAllyPoint(Point basePoint, int range, bool filterDepth, Unit unit)
        {
            Point result = null;
            
            result = (from data in basePoint.DistanceDatas
                      where data.distance < range 
                            && data.point.GetAllyMobs().Count != 0
                            && (data.point.GetAllyMobs().Count == 1 && data.point.GetAllyMobs()[0].Equals(unit)) == false
                            && (filterDepth ? basePoint.Depth == data.point.Depth : true)
                      orderby data.distance
                      select data.point).FirstOrDefault();

            return result;
        }

        public Point FindNotFullHpAllyUnitPoint(Point basePoint, int range, bool filterDepth, Unit unit)
        {
            Point result = null;

            result = (from data in basePoint.DistanceDatas
                      where data.distance < range 
                            && data.point.GetNotFullHpAllyMobs().Count != 0
                            && (data.point.GetAllyMobs().Count == 1 && data.point.GetAllyMobs()[0].Equals(unit)) == false
                            && (filterDepth ? basePoint.Depth == data.point.Depth : true)
                      orderby data.distance
                      select data.point).FirstOrDefault();

            return result;
        }
        
        public Point FindEnemyPoint(Point basePoint, int range, bool filterDepth, Unit unit)
        {
            Point result = null;

            result = (from data in basePoint.DistanceDatas
                      where data.distance < range 
                            && data.point.GetEnemyMobs().Count != 0
                            && (data.point.GetEnemyMobs().Count == 1 && data.point.GetEnemyMobs()[0].Equals(unit)) == false
                            && (filterDepth ? basePoint.Depth == data.point.Depth : true)
                      orderby data.distance
                      select data.point).FirstOrDefault();

            return result;
        }

        public Point FindNotFullHpEnemyUnitPoint(Point basePoint, int range, bool filterDepth, Unit unit)
        {
            Point result = null;

            result = (from data in basePoint.DistanceDatas
                      where data.distance < range 
                            && data.point.GetNotFullHpEnemyMobs().Count != 0
                            && (data.point.GetEnemyMobs().Count == 1 && data.point.GetEnemyMobs()[0].Equals(unit)) == false
                            && (filterDepth ? basePoint.Depth == data.point.Depth : true)
                      orderby data.distance
                      select data.point).FirstOrDefault();

            return result;
        }
        
        public Point FindBuilding(Point basePoint, int range, bool filterDepth, Unit building)
        {
            Point result = null;

            result = (from data in basePoint.DistanceDatas
                      where data.distance < range 
                            && data.point.IsBuilding
                            && (data.point.GetBuildings().Count == 1 && data.point.GetBuildings()[0].Equals(building)) == false
                            && (filterDepth ? basePoint.Depth == data.point.Depth : true)
                      orderby data.distance
                      select data.point).FirstOrDefault();

            return result;
        }

        public Point FindNotFullHpBuildingUnitPoint(Point basePoint, int range, bool filterDepth, Unit building)
        {
            Point result = null;

            result = (from data in basePoint.DistanceDatas
                      where data.distance < range 
                            && data.point.GetNotFullHpAllyBuildings().Count != 0
                            && (data.point.GetBuildings().Count == 1 && data.point.GetBuildings()[0].Equals(building)) == false
                            && (filterDepth ? basePoint.Depth == data.point.Depth : true)
                      orderby data.distance
                      select data.point).FirstOrDefault();

            return result;
        }
        
        public void ClearAllPointBuff() => points.ForEach(point => point.ClearBuff());

        public void SetAllPointStateEffect(PointState state) => points.ForEach(point => point.SetState(state));
        public void UnSetAllPointStateEffect(PointState state) => points.ForEach(point => point.UnSetState(state));
        public void ClearSetAllPointStateEffect() => points.ForEach(point => point.ClearState());

        #region Editor
#if UNITY_EDITOR

        [EditorButton("TestInitBounds")]
        public bool testInitBounds;
        private void TestInitBounds() => bounds.ForEach(data => data.Init());
        
        [EditorButton("SetPointsToDistance")]
        public bool setPointsToDistance;
        private void SetPointsToDistance() => points.ForEach(point => { SetPointToDistance(point); });
        private void SetPointToDistance(Point point)
        {
            point.DistanceDatas.Clear();

            Vector3 basePos = point.transform.position;

            points.ForEach(item =>
            {
                var pointDistanceData = new PointDistanceData();
                pointDistanceData.point = item;
                pointDistanceData.distance = Vector3.Distance(item.transform.position, basePos);
                point.DistanceDatas.Add(pointDistanceData);
            });
        }

        [EditorButton("SetPointsToNode")]
        public bool setPointsToNode;
        private void SetPointsToNode() => points.ForEach(point => { SetPointToNode(point); });
        private void SetPointToNode(Point point)
        {
            point.Nodes.Clear();

            var connectedPoints = points.FindAll(target => IsReachedNode(point, target));

            AddNode(point, connectedPoints);
        }

        private bool IsReachedNode(Point point, Point target)
        {
            Vector3 basePos = point.Position;

            return (target.Position - basePos).sqrMagnitude < (Point.POINT_SCALE + Point.POINT_SCALE * 0.6) * (Point.POINT_SCALE + Point.POINT_SCALE * 0.6)
                        && (target.Position - basePos).sqrMagnitude != 0
                        && target.Depth == point.Depth
                        && target.IsWall == false;
        }

        private void AddNode(Point point, List<Point> connectedPoints)
        {
            foreach (var connectedPoint in connectedPoints)
            {
                Node node = new Node(connectedPoint, (connectedPoint.Position - point.Position).magnitude);

                point.Nodes.Add(node);
            }
        }

#endif
        #endregion
    }
}

