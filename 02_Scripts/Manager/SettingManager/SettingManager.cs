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
    public class SettingManager : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Editor

        private List<Type> settingTypes;
        [HideInInspector]
        public List<Type> SettingTypes
        {
            get
            {
                if (settingTypes == null)
                    settingTypes = Assembly.GetExecutingAssembly().GetTypes()
                                           .Where(type => type.IsClass && type.GetInterface("ISetting") != null)
                                           .ToList();

                return settingTypes;
            }
        }
        
        public void UpdateMemberValues()
        {
            settingClasses.ForEach(x => x.UpdateMemberValues());
        }
        
        #endregion
#endif
        #region NestedClass
        [Serializable]
        public class SettingClassInfo : IComparable<SettingClassInfo>
        {
            public SettingClassInfo()
            {
                typeToString = string.Empty;
                memberValues = new List<SettingMemberValueInfo>();
            }

            public bool isExistValue;
            public string typeToString;
            public List<SettingMemberValueInfo> memberValues;

            public Type BaseType => Type == null ? null : Type.BaseType;
            public Type Type
            {
                get
                {
                    if (string.IsNullOrEmpty(TypeToString))
                    {
                        return null;
                    }

                    return Assembly.GetExecutingAssembly().GetType(TypeToString);
                }
            }

            public string TypeToString
            {
                get => typeToString;
                set
                {
                    if (typeToString.Equals(value))
                    {
                        return;
                    }
                    
                    typeToString = value;

                    UpdateMemberValues();
                }
            }

            public void UpdateMemberValues()
            {
                if (string.IsNullOrEmpty(typeToString))
                    return;

                var type = Assembly.GetExecutingAssembly().GetType(typeToString);

                if (type == null)
                    return;

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.GetCustomAttribute(typeof(SettingValueAttribute), true) != null);
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(property => property.SetMethod != null && property.GetCustomAttribute(typeof(SettingValueAttribute), true) != null);

                UpdateFieldMemberValues(fields);
                UpdatePropertyMemberValues(properties);

                RemoveNotExistMemberValues(fields, properties);

                isExistValue = memberValues.Any(x =>
                    x.Type != null && x.Type.BaseType != typeof(Enum) && string.IsNullOrEmpty(x.value) == false
                    || x.obj != null
                    || x.sprite != null);
            }

            private void UpdateFieldMemberValues(IEnumerable<FieldInfo> fields)
            {
                foreach (var field in fields)
                {
                    if (memberValues.Any(item => item.name.Equals(field.Name)))
                        continue;

                    SettingMemberValueInfo settingValueInfo = new SettingMemberValueInfo();
                    settingValueInfo.name = field.Name;
                    settingValueInfo.type = field.FieldType.FullName;
                    settingValueInfo.value = "";

                    memberValues.Add(settingValueInfo);
                }
            }

            private void UpdatePropertyMemberValues(IEnumerable<PropertyInfo> properties)
            {
                foreach (var property in properties)
                {
                    if (memberValues.Any(item => item.name.Equals(property.Name)))
                        continue;

                    SettingMemberValueInfo settingValueInfo = new SettingMemberValueInfo();
                    settingValueInfo.name = property.Name;
                    settingValueInfo.type = property.PropertyType.FullName;
                    settingValueInfo.value = "";

                    memberValues.Add(settingValueInfo);
                }
            }

            private void RemoveNotExistMemberValues(IEnumerable<FieldInfo> fields, IEnumerable<PropertyInfo> properties)
            {
                foreach (var item in memberValues.ToList())
                {
                    if (fields.Any(field => field.Name.Equals(item.name)))
                        continue;

                    if (properties.Any(property => property.Name.Equals(item.name)))
                        continue;

                    memberValues.Remove(item);
                }
            }
            
            public int CompareTo(SettingClassInfo other)
            {
                if (BaseType != null && other.BaseType != null)
                {
                    int baseTypeCompare = BaseType.FullName.CompareTo(other.BaseType.FullName);
                    if (baseTypeCompare != 0)
                        return baseTypeCompare;
                }

                int typeCompare = typeToString.CompareTo(other.typeToString);
                if (typeCompare != 0)
                    return typeCompare;

                return GetHashCode().CompareTo(other.GetHashCode());
            }
        }

        [Serializable]
        public class SettingMemberValueInfo
        {
            public SettingMemberValueInfo()
            {
                name = string.Empty;
                value = string.Empty;
                type = string.Empty;
                obj = null;
                sprite = null;
            }

            public string name;
            public string value;
            public string type;
            public GameObject obj;
            public Sprite sprite;

            public Type Type
            {
                get
                {
                    if (string.IsNullOrEmpty(this.type))
                    {
                        return null;
                    }

                    var type = Type.GetType(this.type);

                    if (type != null)
                        return type;

                    var currentAssembly = Assembly.GetExecutingAssembly();
                    var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
                    foreach (var assemblyName in referencedAssemblies)
                    {
                        var assembly = Assembly.Load(assemblyName);
                        if (assembly != null)
                        {
                            type = assembly.GetType(this.type);
                            if (type != null)
                                return type;
                        }
                    }

                    return null;
                }
            }
        }

        #endregion

        #region Singleton
        private static SettingManager instance;
        public static SettingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(SettingManager)) as SettingManager;
                }

                return instance;
            }
        }
        #endregion

        public List<SettingClassInfo> settingClasses;
        public Dictionary<string, List<SettingMemberValueInfo>> settingClassesCache = new Dictionary<string, List<SettingMemberValueInfo>>();

        public List<SettingMemberValueInfo> GetClassSettingValues(string type)
        {
            if (settingClassesCache.ContainsKey(type))
            {
                return settingClassesCache[type];
            }

            if (settingClasses == null || settingClasses.Count == 0)
            {
                return null;
            }

            var matchClass = settingClasses.Find(item => item.TypeToString.Equals(type));
            if (matchClass == null)
            {
                return null;
            }

            settingClassesCache[type] = matchClass.memberValues.ToList();
            return settingClassesCache[type];
        }

        private KeyBindingOption keyBindingOption;
        public KeyBindingOption KeyBindingOption => keyBindingOption;
        
        private LanguageOption languageOption;
        public LanguageOption LanguageOption => languageOption;
        
        private ControlOption controlOption;
        public ControlOption ControlOption => controlOption;
        
        private QualityOption qualityOption;
        public QualityOption QualityOption => qualityOption;
        
        private ResolutionOption resolutionOption;
        public ResolutionOption ResolutionOption => resolutionOption;
        
        private VolumeOption volumeOption;
        public VolumeOption VolumeOption => volumeOption;

        private List<IOption> options = new List<IOption>();
        public bool IsOptionChanged => options.Any(option => option.IsChanged);
        
        protected void Start()
        {
            keyBindingOption = new KeyBindingOption(false);
            options.Add(keyBindingOption);
            languageOption = new LanguageOption(true);
            options.Add(languageOption);
            controlOption = new ControlOption(true);
            options.Add(controlOption);
            qualityOption = new QualityOption(false);
            options.Add(qualityOption);
            resolutionOption = new ResolutionOption(false);
            options.Add(resolutionOption);
            volumeOption = new VolumeOption(true);
            options.Add(volumeOption);
        }

        public void ResetAllOption() => options.ForEach(option => option.Reset());
        public void RevertAllOption() => options.ForEach(option => option.Revert());
        public void SaveAllOption() => options.ForEach(option => option.Save());
    }
}