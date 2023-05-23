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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public class Navigation
    {
        #region PointData Class

        private class PointData
        {
            private static List<Point> pointPathList = new List<Point>();

            public Point MainPoint
            {
                get;
                private set;
            }

            private PointData ParentPoint
            {
                get;
                set;
            }

            public float TotalWeight
            {
                get;
                private set;
            }
            
            public bool IsVisited
            {
                get; 
                set;
            }

            public void Init(Point mainPoint, PointData parentPoint, float totalWeight)
            {
                this.MainPoint = mainPoint;
                this.ParentPoint = parentPoint;
                this.TotalWeight = totalWeight;
                IsVisited = false;
            }

            public void ClearData()
            {
                ParentPoint = null;
                TotalWeight = float.MaxValue;
                IsVisited = false;
            }

            public void UpdateData(PointData parentPoint, float totalWeight)
            {
                this.ParentPoint = parentPoint;
                this.TotalWeight = totalWeight;
            }

            public List<Point> GetPath()
            {
                pointPathList.Clear();

                CalcPath();
                pointPathList.RemoveAt(0); //첫 시작 지점은 제거하고 리턴

                return pointPathList.ToList();
            }
            private void CalcPath()
            {
                CalcParentPath();
                pointPathList.Add(MainPoint);
            }

            private void CalcParentPath()
            {
                if (ParentPoint == null) return;

                ParentPoint.CalcParentPath();
                pointPathList.Add(ParentPoint.MainPoint);
            }
        }
        #endregion

        private const int CREATE_RANDOM_PATH_COUNT = 4;

        private Dictionary<(Point start, Point end), List<List<Point>>> pathDataDic = new Dictionary<(Point, Point), List<List<Point>>>();

        public Navigation()
        {
            List<Point> points = D.SelfBoard.points;

            if (points == null)
            {
                Debug.LogError("List<Point> In 'Navigation Constructor' Is Null");
            }

            using (PerfTimerRegion perf = new PerfTimerRegion("SaveAllPath"))
            {
                PairClass<Point, PointData> pointDatas = CreatePointDataList(points);

                SaveAllPath(pointDatas, true);

                for (int i = 0; i < CREATE_RANDOM_PATH_COUNT; i++)
                {
                    SaveAllPath(pointDatas, false);
                }
            }
        }

        public List<Point> GetPossiblePath(Point start) => (from item in pathDataDic.Keys
                                                                    where item.start.Equals(start)
                                                                    select item.end).ToList();

        public List<Point> LoadShortestPath(Point start, Point end)
        {
            Debug.Log("Start To Func : LoadShortestPath");

            var result = pathDataDic.ContainsKey((start, end)) ? pathDataDic[(start, end)] : Enumerable.Empty<List<Point>>().ToList();

            if (result.Count == 0) return Enumerable.Empty<Point>().ToList();

            return result[0];
        }

        public List<Point> LoadRandomPath(Point start, Point end)
        {
            Debug.Log("Start To Func : LoadShortestPath");

            var result = pathDataDic.ContainsKey((start, end)) ? pathDataDic[(start, end)] : Enumerable.Empty<List<Point>>().ToList();

            if (result.Count == 0) return Enumerable.Empty<Point>().ToList();

            int index = Random.Range(0, result.Count);

            return result[index];
        }

        public List<List<Point>> LoadPaths(Point start, Point end)
        {
            Debug.Log("Start To Func : LoadShortestPath");

            var result = pathDataDic.ContainsKey((start, end)) ? pathDataDic[(start, end)] : Enumerable.Empty<List<Point>>().ToList();

            return result;
        }

        private PairClass<Point, PointData> CreatePointDataList(List<Point> points)
        {
            Debug.Log("Start To Func : CreatePointDataList");

            PairClass<Point, PointData> pairClass = new PairClass<Point, PointData>();

            foreach (Point point in points)
            {
                PointData pointData = new PointData();
                pointData.Init(point, null, float.MaxValue);
                pairClass.Add(point, pointData);
            }

            return pairClass;
        }

        private void SaveAllPath(PairClass<Point, PointData> pointdatas, bool isMakeStable)
        {
            Debug.Log("Start To Func : SaveShortestAllPath");

            foreach (PointData pointData in pointdatas.ToList())
            {
                MakePath(pointdatas, pointData, isMakeStable);
            }
        }

        private void MakePath(PairClass<Point, PointData> pointdatas, PointData start, bool isMakeStable)
        {
            Dijkstra(pointdatas, start, isMakeStable);

            SavePathInDic(pointdatas, start);
        }

        private void SavePathInDic(PairClass<Point, PointData> pointdatas, PointData start)
        {
            foreach (var pointData in pointdatas)
            {
                List<Point> result = new List<Point>();
                PointData cur = pointData;

                result = cur.GetPath();

                if (result.Count == 0)
                {
                    continue;
                }
                
                // 딕셔너리에 데이터가 없다면
                if (!IsContainsKeyInDic(start.MainPoint, cur.MainPoint))
                {
                    pathDataDic.Add((start.MainPoint, cur.MainPoint), new List<List<Point>>());
                }

                // 딕셔너리에 같은 경로가 없다면
                if (!CheckPathInDic(start.MainPoint, cur.MainPoint, result))
                {
                    pathDataDic[(start.MainPoint, cur.MainPoint)].Add(result);
                }
            }
        }

        // Dictionary에 데이터가 있는지 여부 확인
        private bool IsContainsKeyInDic(Point start, Point end)
        {
            if (pathDataDic.Count == 0) return false;
            if (pathDataDic.ContainsKey((start, end)) == false) return false;

            return true;
        }

        private bool CheckPathInDic(Point start, Point end, List<Point> list)
        {
            List<List<Point>> pathDataList = pathDataDic[(start, end)];

            if (pathDataList.Count == 0) return false;

            foreach (var pathData in pathDataList)
            {
                if (pathData.Count != list.Count) continue;

                for (int i = 0; i < pathData.Count; i++)
                {
                    if (pathData[i] != list[i]) break;

                    if (i == pathData.Count - 1) return true;
                }
            }

            return false;
        }

        private void Dijkstra(PairClass<Point, PointData> pointDatas, PointData start, bool isMakeStable)
        {
            foreach (var pointData in pointDatas)
            {
                pointData.ClearData();
                
                if (pointData == start)
                {
                    pointData.UpdateData(null, 0);
                }
            }

            var datas = pointDatas.ToList();
            
            while (datas.Count > 0)
            {
                datas.Sort((x, y) => x.TotalWeight.CompareTo(y.TotalWeight));

                var curPointData = datas[0];
                curPointData.IsVisited = true;
                
                List<Node> nodes = curPointData.MainPoint.Nodes;
                
                foreach (Node node in nodes)
                {
                    Point connectPoint = node.ConnectedPoint;
                    PointData connectPointData = pointDatas[connectPoint];

                    if (connectPointData != null && connectPointData.IsVisited == false)
                    {
                        float distance = isMakeStable ? node.Weight : node.RandomWeight;
                        distance = curPointData.TotalWeight + distance;

                        if (distance < connectPointData.TotalWeight)
                        {
                            connectPointData.UpdateData(curPointData, distance);
                        }
                    }
                }

                datas.RemoveAt(0);
            }
        }
    }
}