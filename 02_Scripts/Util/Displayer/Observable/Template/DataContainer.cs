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
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ProjectL
{
    public class DataContainer : MonoBehaviour, IObservable
    {
        public Dictionary<string, FieldInfo> fieldInfosCache;
        public Dictionary<string, PropertyInfo> propertyInfosCache;
        public Dictionary<string, List<IObserver>> observers = new Dictionary<string, List<IObserver>>();

        protected virtual void Awake()
        {
            AddObserverField();
        }

        public void AddObserverField()
        {
            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfosCache = fields.Where(field => field.GetCustomAttribute(typeof(DataObservable)) != null)
                                    .ToDictionary(x => x.Name, x => x);

            foreach (var observer in fieldInfosCache)
            {
                if (observers.ContainsKey(observer.Key) == false)
                    observers.Add(observer.Key, new List<IObserver>());
            }

            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfosCache = properties
                                .Where(property => property.GetCustomAttribute(typeof(DataObservable)) != null)
                                .ToDictionary(x => x.Name, x => x);

            foreach (var observer in propertyInfosCache)
            {
                if (observers.ContainsKey(observer.Key) == false)
                    observers.Add(observer.Key, new List<IObserver>());
            }
        }
        
        #if UNITY_EDITOR
        
        [EditorButton("TestNotifyObserver"), SerializeField]
        private bool testNotifyObserver;
        protected void TestNotifyObserver() => this.NotifyObserver();
        
        #endif
    }
}
