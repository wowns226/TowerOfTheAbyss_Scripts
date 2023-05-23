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
    public class BuildingManager : MonoSingleton<BuildingManager>
    {
        private List<Type> buildingTypes = new List<Type>();
        public List<Type> BuildingTypes
        {
            get
            {
                if (buildingTypes.Count == 0)
                    buildingTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(Building)) && type.IsAbstract == false).ToList();

                return buildingTypes;
            }
        }

        private Dictionary<(OwnerType, GradeType, BuildingType), List<string>> buildingTypeNames = new Dictionary<(OwnerType, GradeType, BuildingType), List<string>>();
        public Dictionary<(OwnerType, GradeType, BuildingType), List<string>> BuildingTypeNames
        {
            get
            {
                if (buildingTypeNames.Count != 0)
                {
                    return buildingTypeNames;
                }

                SetBuildingTypeNames(OwnerType.My, BuildingTypes);

                return buildingTypeNames;
            }
        }

        private void SetBuildingTypeNames(OwnerType ownerType, List<Type> buildingTypes)
        {
            foreach (var type in buildingTypes)
            {
                var attribute = type.GetCustomAttribute(typeof(BuildingTypeAttribute)) as BuildingTypeAttribute;

                if (attribute == null)
                {
                    Debug.Log($"BuildingManager.SetBuildingTypeNames attribute is null, Type : {type.Name}");
                    continue;
                }

                var gradeType = attribute.GradeType;
                var buildingType = attribute.BuildingType;

                if (buildingTypeNames.ContainsKey((ownerType, gradeType, buildingType)) == false)
                {
                    buildingTypeNames.Add((ownerType, gradeType, buildingType), new List<string>());
                }

                buildingTypeNames[(ownerType, gradeType, buildingType)].Add(type.Name);
            }
        }

        public List<string> GetAllRandomBuildingNames(OwnerType ownerType, GradeType gradeType, int count)
         => GetRandomBuildingNames((ownerType, gradeType, GetRandomBuildingType()), count);

        public BuildingType GetRandomBuildingType()
        {
            var enums = Enum.GetValues(typeof(BuildingType));
            int index = Random.Range(1, enums.Length); // 0 은 Common이라 제외

            return (BuildingType)enums.GetValue(index);
        }

        public List<string> GetRandomBuildingNames((OwnerType, GradeType, BuildingType buildingType) key, int count)
        {
            if (BuildingTypeNames.ContainsKey(key) == false || key.buildingType == BuildingType.Common)
            {
                Debug.Log("BuildManager.GetRandomBuildingNames key is invalid");
                return Enumerable.Empty<string>().ToList();
            }

            var types = BuildingTypeNames[key];

            List<string> result = new List<string>();

            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, types.Count);
                result.Add(types[index]);
            }

            return result;
        }
    }
}
