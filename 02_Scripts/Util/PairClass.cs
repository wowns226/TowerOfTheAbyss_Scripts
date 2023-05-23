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
using System.Linq;

namespace ProjectL
{
    /// <summary>
    /// List, Dictionary를 동시에 구현한 클래스(int-Key는 사용 불가능)
    /// </summary>
    /// <typeparam name="TKey">key</typeparam>
    /// <typeparam name="TValue">value</typeparam>
    public class PairClass<TKey, TValue> : IEnumerable<TValue>
    {
        public PairClass()
        {
            if (typeof(TKey) == typeof(int))
                throw new InvalidCastException();
        }

        private List<TValue> list = new List<TValue>();
        public List<TValue> List => list;
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        public Dictionary<TKey, TValue> Dictionary => dictionary;

        public TValue this[TKey key] => dictionary[key];

        public TValue this[int index]
        {
            get
            {
                if (index >= list.Count || index < 0)
                    return default(TValue);

                return list[index];
            }
        }

        public int Count => list.Count;

        public void Add(TKey key, TValue value)
        {
            list.Add(value);

            dictionary.Add(key, value);
        }

        public void Insert(int index, TKey key, TValue value)
        {
            list.Insert(index, value);
            dictionary.Add(key, value);
        }

        public void Remove(TKey key)
        {
            list.Remove(dictionary[key]);

            dictionary.Remove(key);
        }

        public void RemoveAll(TValue value)
        {
            list.RemoveAll(item => item.Equals(value));

            var keys = dictionary.Keys.Where(key =>
            {
                if (dictionary[key].Equals(value))
                    return true;

                return false;
            });

            foreach (var key in keys)
                dictionary.Remove(key);
        }

        public void Clear()
        {
            dictionary.Clear();
            list.Clear();
        }

        public int IndexOf(TValue value) => list.IndexOf(value);
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        public bool ContainsValue(TValue value) => dictionary.ContainsValue(value);
        public IEnumerator<TValue> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
        public void Sort() => list.Sort();
        public void Sort(Comparison<TValue> comparison) => list.Sort(comparison);
        public void Sort(IComparer<TValue> comparer) => list.Sort(comparer);

    }
}
