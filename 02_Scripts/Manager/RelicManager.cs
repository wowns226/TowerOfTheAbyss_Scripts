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

namespace ProjectL
{
    public class RelicManager : MonoSingleton<RelicManager>
    {
        private List<Type> relicTypes = new List<Type>();
        public List<Type> RelicTypes
        {
            get
            {
                if(relicTypes.Count == 0)
                    relicTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(Relic)) && type.IsAbstract == false).ToList();

                return relicTypes;
            }
        }

        private List<Type> relicSetTypes = new List<Type>();
        public List<Type> RelicSetTypes
        {
            get
            {
                if(relicTypes.Count == 0)
                    relicSetTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(RelicSet)) && type.IsAbstract == false).ToList();

                return relicSetTypes;
            }
        }
    }
}

