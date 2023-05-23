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

namespace ProjectL
{
    public enum GradeType
    {
        Common = 0,
        Rare = 1,
        Unique = 2,
        Epic = 3,
        Special = 4,
        Legendary = 5,
        Ancient = 6,
    }

    public enum MobType
    {
        Common = 0,
        Named = 1,
        Boss = 2,
        Hero = 3,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MobTypeAttribute : Attribute
    {
        public MobTypeAttribute(GradeType gradeType, MobType mobGradeType)
        {
            ChapterType = ChapterType.All;
            GradeType = gradeType;
            MobGradeType = mobGradeType;
        }
        
        public MobTypeAttribute(ChapterType chapterType, GradeType gradeType, MobType mobGradeType)
        {
            ChapterType = chapterType;
            GradeType = gradeType;
            MobGradeType = mobGradeType;
        }

        public ChapterType ChapterType { get; }
        public GradeType GradeType { get; }
        public MobType MobGradeType { get; }
    }
}
