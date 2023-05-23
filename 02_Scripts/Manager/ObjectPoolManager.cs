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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectL
{
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        private static Dictionary<string, Queue<GameObject>> monoObjectPool = new Dictionary<string, Queue<GameObject>>();
        private static Dictionary<Type, Queue<object>> classObjectPool = new Dictionary<Type, Queue<object>>();

        private const string POST_FIXNAME = "(Clone)";

        public void New(string key, Transform parent = null, Action<GameObject> onComplete = null, bool activeImmediately = true)
        {
            StartCoroutine(NewAsync(key, parent, onComplete, activeImmediately));
        }

        public IEnumerator NewAsync(string key, Transform parent = null, Action<GameObject> onComplete = null, bool activeImmediately = true)
        {
            var objPoolKey = string.Concat(key, POST_FIXNAME);

            if (monoObjectPool.ContainsKey(objPoolKey) && monoObjectPool[objPoolKey].Count > 0)
            {
                var obj = monoObjectPool[objPoolKey].Dequeue();

                if (parent != null)
                    obj.transform.SetParent(parent);
                else
                    obj.transform.SetParent(transform);

                obj.SetActive(activeImmediately);
                
                onComplete?.Invoke(obj);
            }
            else
            {
                var process = Addressables.InstantiateAsync(key, Vector3.zero, Quaternion.identity, parent ??= transform);
                yield return process;

                process.Completed += handle => handle.Result?.SetActive(activeImmediately);
                process.Completed += handle => onComplete?.Invoke(handle.Result);
                Debug.Log($"ObjectPoolManager.New : {key}");
            }
        }

        public T New<T>() where T : class
        {
            if (classObjectPool.ContainsKey(typeof(T)) && classObjectPool[typeof(T)].Count > 0)
            {
                return classObjectPool[typeof(T)].Dequeue() as T;
            }

            return Activator.CreateInstance<T>();
        }

        public void Return(GameObject obj)
        {
            if(monoObjectPool.ContainsKey(obj.name) == false)
            {
                monoObjectPool.Add(obj.name, new Queue<GameObject>());
            }

            monoObjectPool[obj.name].Enqueue(obj);
            obj.transform.SetParent(transform);
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
        }

        public void Return(object obj)
        {
            if (classObjectPool.ContainsKey(obj.GetType()) == false)
            {
                classObjectPool.Add(obj.GetType(), new Queue<object>());
            }

            classObjectPool[obj.GetType()].Enqueue(obj);
        }

        public long GetTotalSize()
        {
            return GC.GetTotalMemory(forceFullCollection: false);
        }
    }
}
