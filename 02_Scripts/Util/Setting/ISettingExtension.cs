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
    public static class ISettingExtension
    {
        private static Dictionary<Type, IEnumerable<FieldInfo>> cacheFields = new Dictionary<Type, IEnumerable<FieldInfo>>();
        private static Dictionary<Type, IEnumerable<PropertyInfo>> cacheProperties = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        public static void SetValue(this ISetting settingClass)
        {
            var settingFields = GetSettingFields(settingClass);
            var settingProperties = GetSettingProperties(settingClass);

            if (settingFields == null && settingProperties == null)
                return;

            var settingValues = GetSettingValues(settingClass);

            if (settingValues == null)
                return;

            SetValue(settingClass, settingFields, settingProperties, settingValues);
        }

        private static IEnumerable<FieldInfo> GetSettingFields(ISetting settingClass)
        {
            var type = settingClass.GetType();

            if (cacheFields.ContainsKey(type) == false)
            {
                cacheFields.Add(type, type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(field => field.GetCustomAttribute(typeof(SettingValueAttribute), true) != null));
            }

            return cacheFields[type];
        }

        private static IEnumerable<PropertyInfo> GetSettingProperties(ISetting settingClass)
        {
            var type = settingClass.GetType();

            if (cacheProperties.ContainsKey(type) == false)
            {
                cacheProperties.Add(type, type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(property => property.SetMethod != null && property.GetCustomAttribute(typeof(SettingValueAttribute), true) != null));
            }

            return cacheProperties[type];
        }

        private static List<SettingManager.SettingMemberValueInfo> GetSettingValues(ISetting settingClass)
        {
            if (SettingManager.Instance is null)
                return null;

            var settingValues = SettingManager.Instance.GetClassSettingValues(settingClass.GetType().FullName);

            return settingValues;
        }

        private static void SetValue(ISetting settingClass, IEnumerable<FieldInfo> settingFields, IEnumerable<PropertyInfo> settingProperties, List<SettingManager.SettingMemberValueInfo> settingValues)
        {
            SetFields(settingClass, settingFields, settingValues);
            SetProperties(settingClass, settingProperties, settingValues);
        }

        private static void SetFields(ISetting settingClass, IEnumerable<FieldInfo> settingFields, List<SettingManager.SettingMemberValueInfo> settingValues)
        {
            foreach (var settingField in settingFields)
            {
                var settingData = settingValues.Find(data => data.name.Equals(settingField.Name));

                if (settingData == null)
                    continue;

                if (string.IsNullOrEmpty(settingData.value) && settingData.sprite != null && settingData.obj != null)
                    continue;

                object dataValue = null;

                if (settingData.sprite != null)
                {
                    dataValue = settingData.sprite;
                }
                else if (settingData.obj != null)
                {
                    dataValue = settingData.obj;
                }
                else if (settingField.FieldType.BaseType.Equals(typeof(Enum)))
                {
                    if (string.IsNullOrEmpty(settingData.value) == false)
                    {
                        dataValue = Enum.Parse(settingField.FieldType, settingData.value);
                    }
                }
                else if (string.IsNullOrEmpty(settingData.value) == false)
                {
                    dataValue = Convert.ChangeType(settingData.value, settingField.FieldType);
                }

                if (dataValue == null)
                {
                    Debug.LogWarning("ISettingExtension.SetFields dataValue is Null");
                    continue;
                }

                settingField.SetValue(settingClass, dataValue);

                Debug.Log($"SetFields, DataClass : {settingClass.GetType()}, Field : {settingField}, Value : {dataValue}");
            }
        }

        private static void SetProperties(ISetting settingClass, IEnumerable<PropertyInfo> settingProperties, List<SettingManager.SettingMemberValueInfo> settingValues)
        {
            foreach (var settingProperty in settingProperties)
            {
                var settingData = settingValues.Find(data => data.name.Equals(settingProperty.Name));

                if (settingData == null)
                    continue;

                if (string.IsNullOrEmpty(settingData.value) && settingData.sprite != null && settingData.obj != null)
                    continue;

                object dataValue = null;

                if (settingData.sprite != null)
                {
                    dataValue = settingData.sprite;
                }
                else if (settingData.obj != null)
                {
                    dataValue = settingData.obj;
                }
                else if (settingProperty.PropertyType.BaseType.Equals(typeof(Enum)))
                {
                    dataValue = Enum.Parse(settingProperty.PropertyType, settingData.value);
                }
                else if(string.IsNullOrEmpty(settingData.value) == false)
                {
                    dataValue = Convert.ChangeType(settingData.value, settingProperty.PropertyType);
                }

                if (dataValue == null)
                {
                    Debug.LogWarning("ISettingExtension.SetProperties dataValue is Null");
                    continue;
                }

                settingProperty.SetValue(settingClass, dataValue);

                Debug.Log($"SetProperties, DataClass : {settingClass.GetType()}, Property : {settingProperty}, Value : {dataValue}");
            }
        }

    }
}
