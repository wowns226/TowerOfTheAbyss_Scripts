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
    [Serializable]
    public class SkillRangeModel
    {
        public bool[] rangeData = new bool[SkillRangeData.SKILL_RANGE];
    }

    [Serializable]
    public class SkillRangeInfo
    {
        [HideInInspector]
        public SkillRangeModel[] rangeRow = new SkillRangeModel[SkillRangeData.SKILL_RANGE]; 
        public int topDepth;
        public int bottomDepth;
        public float duration;
        public float damagePercentage;
    }

    [CreateAssetMenu(fileName = "Skill Object", menuName = "Scriptable Object/Skill Object", order = int.MaxValue)]
    public class SkillRangeData : ScriptableObject
    {
        public const int SKILL_RANGE = 11;
        public List<SkillRangeInfo> rangeInfos = new List<SkillRangeInfo>();

        private (int x, int y) centerIndexOffset;
        public (int x, int y) CenterIndexOffset => centerIndexOffset;
        private int maxRange;
        public int MaxRange
        {
            get
            {
                if (maxRange == 0)
                {
                    CalcMaxRange();
                }

                return maxRange;
            }
        }

        private void CalcMaxRange()
        {
            var combineRangeInfo = CombineRangeInfo(rangeInfos);

            CalcMaxRange(combineRangeInfo);
        }

        private bool[,] CombineRangeInfo(List<SkillRangeInfo> rangeInfos)
        {
            var result = new bool[SKILL_RANGE, SKILL_RANGE];

            for (int i = 0; i < SKILL_RANGE; i++)
            {
                for (int j = 0; j < SKILL_RANGE; j++)
                {
                    result[i, j] = rangeInfos.Any(rangeInfo => rangeInfo.rangeRow[i].rangeData[j]);
                }
            }

            return result;
        }

        private void CalcMaxRange(bool[,] rangeInfo)
        {
            for (int i = 0; i < SKILL_RANGE; i++)
            {
                for (int j = 0; j < SKILL_RANGE; j++)
                {
                    if (rangeInfo[i,j])
                    {
                        CalcMaxRange(rangeInfo, i, j, -1, 0); // Left
                        CalcMaxRange(rangeInfo, i, j, 1, 0); // Right
                        CalcMaxRange(rangeInfo, i, j, 0, 1); // Up
                        CalcMaxRange(rangeInfo, i, j, 0, -1); // Down
                        CalcMaxRange(rangeInfo, i, j, -1, 1); // Left,Up
                        CalcMaxRange(rangeInfo, i, j, -1, -1); // Left,Down
                        CalcMaxRange(rangeInfo, i, j, 1, 1); // Right,Up
                        CalcMaxRange(rangeInfo, i, j, 1, -1); // Right,Down
                    }
                }
            }
        }

        private void CalcMaxRange(bool[,] rangeInfo, int i, int j, int x, int y)
        {
            int xIndex = i + (x + x * maxRange);
            int yIndex = j + (y + y * maxRange);

            CalcRange(rangeInfo, i, j, xIndex, yIndex, x, y);
        }

        private void CalcRange(bool[,] rangeInfo, int i, int j, int xIndex, int yIndex, int x, int y)
        {
            if (CheckOverIndex(xIndex, yIndex))
            {
                return;
            }

            if (rangeInfo[xIndex, yIndex])
            {
                int distance = 0;

                //대각선도 1로 계산
                if (x == 1)
                {
                    distance += xIndex - i;
                }
                else if(x == -1)
                {
                    distance += i - xIndex;
                }
                else if (y == 1)
                {
                    distance += yIndex - j;
                }
                else
                {
                    distance += j - yIndex;
                }

                centerIndexOffset = (((i + xIndex) / 2) - (SKILL_RANGE / 2), ((j + yIndex) / 2) - (SKILL_RANGE / 2));
                maxRange = distance;
            }

            CalcRange(rangeInfo, i, j, xIndex + x, yIndex + y, x, y);
        }

        private bool CheckOverIndex(int xIndex, int yIndex)
        {
            if (xIndex < 0 || xIndex >= SKILL_RANGE)
            {
                return true;
            }

            if (yIndex < 0 || yIndex >= SKILL_RANGE)
            {
                return true;
            }

            return false;
        }

    }
}
