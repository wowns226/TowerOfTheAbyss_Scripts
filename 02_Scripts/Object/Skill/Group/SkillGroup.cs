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
using Random = UnityEngine.Random;

namespace ProjectL
{
    [Serializable]
    public class SkillGroup
    {
        [Serializable]
        private class SkillInfo
        {
            public Skill skill;
            public int weight;
            public int priority;
        }

        [SerializeField]
        private List<SkillInfo> skills = new List<SkillInfo>();

        private Dictionary<int, List<(int weight, Skill skill)>> skiilDic = new Dictionary<int, List<(int weight, Skill skill)>>();

        [SerializeField]
        private Skill extraSkill;
        public Skill ExtraSkill => extraSkill;

        [SerializeField]
        private Skill passiveSkill;
        public Skill PassiveSkill => passiveSkill;
        
        public float RandomValue => Random.Range(0, 100);

        public void Clear()
        {
            ExtraSkill?.Clear();
            PassiveSkill?.Clear();
            skills.ForEach(skillInfo => skillInfo.skill.Clear());
        }

        public void Init(Unit unit)
        {
            ExtraSkill?.Init(unit);
            PassiveSkill?.Init(unit);
            skills.ForEach(skillInfo => skillInfo.skill.Init(unit));
        }

        public List<Skill> GetRandomAllSkills() => skills.OrderBy(skillInfo => skillInfo.priority)
                                                         .ThenByDescending(skillInfo => skillInfo.weight / RandomValue)
                                                         .Select(skillInfo => skillInfo.skill).ToList();
        public List<Skill> GetAllSkills() => skills.ConvertAll(skillInfo => skillInfo.skill);
        public Dictionary<int, List<(int weight, Skill skill)>> GetAllSkillsDic()
        {
            if (skiilDic.Count == 0)
            {
                skiilDic = (from skillInfo in skills
                            orderby skillInfo.priority
                            group skillInfo by skillInfo.priority into skillGroup
                            select skillGroup)
                                        .ToDictionary(skillGroup => skillGroup.Key, skillGroup => skillGroup.ToList().ConvertAll(skillInfo => (skillInfo.weight, skillInfo.skill)));
            }

            return skiilDic;
        }
    }
}
