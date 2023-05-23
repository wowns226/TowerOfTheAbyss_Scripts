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
using UnityEngine;

namespace ProjectL
{
    public abstract class Bag<T> : MonoBehaviour where T : MonoBehaviour, IBagItem
    {
        protected Player player;
        
        protected abstract List<Type> Types { get; }

        protected PairClass<string, T> pairClass = new PairClass<string, T>();
        public List<T> FilterList => pairClass.List.FindAll(CheckCondition);
        public List<T> AllList => pairClass.List;

        public void Init(Player player)
        {
            this.player = player;
            
            Debug.Log($"{name}.Init()");

            foreach(var type in Types)
            {
                ObjectPoolManager.Instance.New(type.Name, transform, obj =>
                {
                    var target = obj.GetComponent<T>();
                    pairClass.Add(type.Name, target);
                    
                    target.Init(player);
                });
            }
        }
        
        public T Get(string key) => pairClass?[key];

        protected abstract bool CheckCondition(T t);
    }
}
