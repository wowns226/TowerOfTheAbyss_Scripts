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
    public enum SkillGradeType
    {
        Common = 1,
        Rare = 2,
        Unique = 5,
        Epic = 10,
        Legendary = 20,
        Acient = 40,
        Individual = 100,
        Extra = 0,
    }

    public enum SkillAttackType
    {
        Attack = 0,
        Buff = 1,
        Heal = 2,
        Special = 3,
        Passive = 4,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SkillTypeAttribute : Attribute
    {
        public SkillTypeAttribute(SkillAttackType skillAttackType, SkillGradeType gradeType)
        {
            this.SkillAttackType = skillAttackType;
            this.GradeType = gradeType;
        }

        public SkillAttackType SkillAttackType { get; }
        public SkillGradeType GradeType { get; }
    }
}
