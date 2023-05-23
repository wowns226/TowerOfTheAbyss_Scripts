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

namespace ProjectL
{
    public static class GradeUtil
    {
        public static Dictionary<GradeType, float> grades = new Dictionary<GradeType, float>
                                                             {
                                                                 {GradeType.Ancient, 2f},
                                                                 {GradeType.Legendary, 3f},
                                                                 {GradeType.Special, 0f},
                                                                 {GradeType.Epic, 5f},
                                                                 {GradeType.Unique, 10f},
                                                                 {GradeType.Rare, 30f},
                                                                 {GradeType.Common, 50f},
                                                             };

        public static GradeType GetRandomGrade()
        {
            float probability = UnityEngine.Random.Range(0, 100f);
            
            foreach (var grade in grades)
            {
                if (grade.Value >= probability)
                {
                    return grade.Key;
                }

                probability -= grade.Value;
            }

            return GradeType.Common;
        }
    }
}