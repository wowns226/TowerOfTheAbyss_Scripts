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
using System.Reflection;
using UnityEngine;

namespace ProjectL
{
    public class SkillManager : MonoSingleton<SkillManager>
    {
        public bool isShowSkillEffect = true;
        
        private List<Type> skillTypes = new List<Type>();
        public List<Type> SkillTypes
        {
            get
            {
                if (skillTypes.Count == 0)
                    skillTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(Skill)) && type.IsAbstract == false).ToList();

                return skillTypes;
            }
        }

        private Dictionary<(SkillAttackType, SkillGradeType), List<(int, string)>> skills = new Dictionary<(SkillAttackType, SkillGradeType), List<(int, string)>>();
        public Dictionary<(SkillAttackType, SkillGradeType), List<(int, string)>> Skills
        {
            get
            {
                if (skills.Count != 0)
                {
                    return skills;
                }

                SetSkills(SkillTypes);

                return skills;
            }
        }

        private void SetSkills(List<Type> skillTypes)
        {
            foreach (var type in skillTypes)
            {
                var attribute = type.GetCustomAttribute(typeof(SkillTypeAttribute)) as SkillTypeAttribute;

                if (attribute == null)
                {
                    Debug.Log($"SkillManager.SetSkills attribute is null, Type : {type.Name}");
                    continue;
                }

                var skillAttackType = attribute.SkillAttackType;
                var gradeType = attribute.GradeType;

                if (skills.ContainsKey((skillAttackType, gradeType)) == false)
                {
                    skills.Add((skillAttackType, gradeType), new List<(int, string)>());
                }

                skills[(skillAttackType, gradeType)].Add(((int)gradeType, type.Name));
            }
        }

    }
}
