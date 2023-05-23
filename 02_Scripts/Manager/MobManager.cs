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
using Random = UnityEngine.Random;

namespace ProjectL
{
    public class MobManager : MonoSingleton<MobManager>
    {
        private List<Type> playerMobTypes = new List<Type>();
        public List<Type> PlayerMobTypes
        {
            get
            {
                if (playerMobTypes.Count == 0)
                    playerMobTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(PlayerMob)) && type.IsAbstract == false).ToList();

                return playerMobTypes;
            }
        }

        private List<Type> enemyMobTypes = new List<Type>();
        public List<Type> EnemyMobTypes
        {
            get
            {
                if (enemyMobTypes.Count == 0)
                    enemyMobTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(EnemyMob)) && type.IsAbstract == false).ToList();

                return enemyMobTypes;
            }
        }

        private Dictionary<(ChapterType, OwnerType, GradeType, MobType), List<(GradeType, MobType, string)>> mobTypeNames = new Dictionary<(ChapterType, OwnerType, GradeType, MobType), List<(GradeType, MobType, string)>>();
        public Dictionary<(ChapterType, OwnerType, GradeType, MobType), List<(GradeType, MobType, string)>> MobTypeNames
        {
            get
            {
                if (mobTypeNames.Count != 0)
                {
                    return mobTypeNames;
                }

                SetMobTypeNames(OwnerType.My, PlayerMobTypes);
                SetMobTypeNames(OwnerType.Enemy, EnemyMobTypes);

                return mobTypeNames;
            }
        }

        private void SetMobTypeNames(OwnerType ownerType, List<Type> mobTypes)
        {
            foreach (var type in mobTypes)
            {
                var attribute = type.GetCustomAttribute(typeof(MobTypeAttribute)) as MobTypeAttribute;

                if (attribute == null)
                {
                    Debug.Log($"MobManager.SetMobTypeNames attribute is null, Type : {type.Name}");
                    continue;
                }

                var chapterType = attribute.ChapterType;
                var gradeType = attribute.GradeType;
                var mobGradeType = attribute.MobGradeType;

                if (mobTypeNames.ContainsKey((chapterType, ownerType, gradeType, mobGradeType)) == false)
                {
                    mobTypeNames.Add((chapterType, ownerType, gradeType, mobGradeType), new List<(GradeType, MobType, string)>());
                }

                mobTypeNames[(chapterType, ownerType, gradeType, mobGradeType)].Add((gradeType, mobGradeType, type.Name));
            }
        }

        public List<(GradeType gradeType, MobType mobType, string type)> GetRandomMobNames((ChapterType chapter, OwnerType owner, GradeType grade, MobType mob) key, int count)
        { 
            key.chapter = key.chapter == ChapterType.UnderWorldCastle
                              ? ChapterType.All
                              : RoundManager.Instance.Chapter;
            
            var types = GetMatchedTypes(key);
            if (types.Count == 0)
            {
                return types;
            }

            var result = new List<(GradeType, MobType, string)>();
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, types.Count);
                result.Add(types[index]);
            }

            return result;
        }

        private List<(GradeType, MobType, string)> GetMatchedTypes((ChapterType chapter, OwnerType owner, GradeType grade, MobType mob) key)
        {
            if (key.chapter == ChapterType.All)
            {
                return GetAllMobTypes(key);
            }
            
            return GetFilterMobTypes(key);
        }
        
        private List<(GradeType, MobType, string)> GetAllMobTypes((ChapterType chapter, OwnerType owner, GradeType grade, MobType mob) key)
        {
            var result = new List<(GradeType, MobType, string)>();

            foreach (ChapterType chapter in Enum.GetValues(typeof(ChapterType)))
            {
                var tempKey = (chapter, key.owner, key.grade, key.mob);
                if (MobTypeNames.ContainsKey(tempKey))
                {
                    result.AddRange(MobTypeNames[tempKey]);
                }
            }
            
            return result;
        }
        
        private List<(GradeType, MobType, string)> GetFilterMobTypes((ChapterType chapter, OwnerType owner, GradeType grade, MobType mob) key)
        {
            var result = new List<(GradeType, MobType, string)>();

            var allKey = (ChapterType.All, key.owner, key.grade, key.mob);
            if (MobTypeNames.ContainsKey(allKey))
            {
                result.AddRange(MobTypeNames[allKey]);
            }

            if (MobTypeNames.ContainsKey(key))
            {
                result.AddRange(MobTypeNames[key]);
            }

            return result;
        }
    }
}
