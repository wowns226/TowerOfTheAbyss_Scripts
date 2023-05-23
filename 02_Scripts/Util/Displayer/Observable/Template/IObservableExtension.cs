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

namespace ProjectL
{
    public static class IObservableExtension
    {
        public static void RegisterObserver(this DataContainer dataContainer, string dataName, IObserver observer)
        {
            if (dataContainer.observers.ContainsKey(dataName) == false)
            {
                dataContainer.AddObserverField();
            }

            dataContainer.observers[dataName].Add(observer);
            dataContainer.NotifyObserver(dataName);
        }

        public static void UnRegisterObserver(this DataContainer dataContainer, string dataName, IObserver observer)
        {
            if (dataContainer.observers.ContainsKey(dataName) == false)
                return;

            dataContainer.observers[dataName].Remove(observer);
        }

        public static void NotifyObserver(this DataContainer dataContainer)
        {
            if (dataContainer.observers == null)
                return;

            foreach (var dataName in dataContainer.observers.Keys)
                _NotifyObserver(dataContainer, dataName);
        }

        public static void NotifyObserver(this DataContainer dataContainer, string dataName)
        {
            if (dataContainer.observers.ContainsKey(dataName) == false)
                return;

            _NotifyObserver(dataContainer, dataName);
        }

        private static void _NotifyObserver(DataContainer dataContainer, string dataName)
        {
            object dataValue = null;

            if (dataContainer.fieldInfosCache.TryGetValue(dataName, out var fieldInfo))
            {
                if (fieldInfo != null)
                    dataValue = fieldInfo.GetValue(dataContainer);
            }
            else if (dataContainer.propertyInfosCache.TryGetValue(dataName, out var propertyInfo))
            {
                if (propertyInfo != null)
                    dataValue = propertyInfo.GetValue(dataContainer);
            }
            
            if (ReferenceEquals(dataValue, null) == false)
                dataContainer.observers[dataName].ForEach(observer => observer.UpdateData(dataValue));
        }
    }
}
