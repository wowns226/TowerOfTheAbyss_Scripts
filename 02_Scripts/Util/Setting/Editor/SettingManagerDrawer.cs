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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = Unity.VisualScripting.ColorUtility;

namespace ProjectL
{
    [CustomEditor(typeof(SettingManager))]
    public class SettingManagerDrawer : Editor
    {
        SettingManager settingManager;
        List<Type> settingTypes;
        Dictionary<string, List<string>> prefabs = new Dictionary<string, List<string>>();

        Dictionary<string, bool> foldOutDic = new Dictionary<string, bool>();
        
        private GUIStyle foldoutStyle;
        
        private void OnEnable()
        {
            foldoutStyle = new GUIStyle(EditorStyles.foldout)
                           {
                               normal =
                               {
                                   textColor = new Color(1f, 0.5f, 0f)
                               }
                               ,
                               onNormal =
                               {
                                   textColor = new Color(0.8f, 0.3f, 0f)
                               }
                           };

            settingManager = (SettingManager)target;
            settingTypes = settingManager.SettingTypes;

            if (settingManager.settingClasses == null)
            {
                return;
            }

            foreach (var settingClass in settingManager.settingClasses)
            {
                foldOutDic[settingClass.TypeToString] = false;
            }
            
            settingManager.settingClasses.ForEach(settingClass =>
            {
                settingClass.UpdateMemberValues();
            });
            
            settingManager.settingClasses.ForEach(settingClass =>
            {
                settingClass.memberValues.Sort((a, b) => a.name.CompareTo(b.name));
            });
            
            settingManager.settingClasses.Sort((item1, item2) => item1.CompareTo(item2));
        }

        public override void OnInspectorGUI()
        {
            if (settingManager == null || settingManager.settingClasses == null || settingManager.settingClasses.Count == 0)
            {
                base.OnInspectorGUI();
                return;
            }
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Update Data"))
            {
                settingManager.UpdateMemberValues();
            }
            
            EditorGUILayout.Space(20);
            
            int listCount = settingManager.settingClasses.Count;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Setting Classes");

            EditorGUI.BeginDisabledGroup(true);
            int size = EditorGUILayout.IntField(listCount);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("+", GUILayout.ExpandWidth(true)))
                size++;

            if (GUILayout.Button("-", GUILayout.ExpandWidth(true)))
                size--;

            if (size < listCount)
                settingManager.settingClasses.RemoveRange(size, listCount - size);
            else if (size > listCount)
                settingManager.settingClasses.AddRange(Enumerable.Repeat(new SettingManager.SettingClassInfo(), size - listCount));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indent + 1;

            Dictionary<string, List<SettingManager.SettingClassInfo>> settingClassDic = new Dictionary<string, List<SettingManager.SettingClassInfo>>();
            settingManager.settingClasses.ForEach(settingClass =>
            {
                var key = "<None>";

                if (settingClass.BaseType != null)
                    key = settingClass.BaseType.Name;

                if (settingClassDic.ContainsKey(key) == false)
                {
                    settingClassDic.Add(key, new List<SettingManager.SettingClassInfo>());
                }

                settingClassDic[key].Add(settingClass);
            });
            
            List<string> dataNameList = new List<string>();
            dataNameList.Add("<None>");
            dataNameList.AddRange(settingManager.SettingTypes.
                                                 Where(type => type.IsAbstract == false && foldOutDic.ContainsKey(type.FullName) == false).ToList().
                                                 ConvertAll(type => type.FullName).ToArray());

            var dataNameArray = dataNameList.ToArray();
            
            foreach (var item in settingClassDic)
            {
                var dicKey = item.Key + "dic";

                if (foldOutDic.ContainsKey(dicKey) == false)
                {
                    foldOutDic.Add(dicKey, false);
                }

                if (item.Key.Equals("<None>") == false)
                    foldOutDic[dicKey] = EditorGUILayout.Foldout(foldOutDic[dicKey], item.Key);

                if (foldOutDic[dicKey] || item.Key.Equals("<None>"))
                {
                    foreach (var settingClass in item.Value)
                    {
                        EditorGUILayout.BeginHorizontal();

                        int dataIndex = -1;

                        var dataName = settingClass.TypeToString;

                        if (string.IsNullOrEmpty(dataName))
                            dataIndex = 0;

                        int newIndex = 0;

                        if (dataIndex == 0)
                        {
                            GUILayout.FlexibleSpace();
                            newIndex = EditorGUILayout.Popup(dataIndex, dataNameArray, GUILayout.Width((EditorGUIUtility.currentViewWidth - 26) / 2f), GUILayout.Height(20f));

                            if (newIndex != 0)
                            {
                                dataName = dataNameList[newIndex];
                                settingClass.TypeToString = dataName;
                            }
                        }
                        else
                        {
                            newIndex = dataIndex;
                            dataName = settingClass.TypeToString;
                        }

                        EditorGUILayout.EndHorizontal();

                        if (newIndex != 0)
                        {
                            EditorGUILayout.BeginHorizontal();

                            if (foldOutDic.ContainsKey(dataName) == false)
                            {
                                foldOutDic.Add(dataName, false);
                            }

                            EditorGUI.indentLevel++;

                            if (settingClass.isExistValue)
                            {
                                foldOutDic[dataName] = EditorGUILayout.Foldout(foldOutDic[dataName], dataName, foldoutStyle);
                            }
                            else
                            {
                                foldOutDic[dataName] = EditorGUILayout.Foldout(foldOutDic[dataName], dataName);
                            }
                            

                            if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                            {
                                foldOutDic.Remove(settingClass.TypeToString);
                                settingManager.settingClasses.Remove(settingClass);
                                continue;
                            }

                            EditorGUILayout.EndHorizontal();

                            if (foldOutDic[dataName])
                            {
                                var values = settingClass.memberValues;

                                for (int k = 0; k < values.Count; k++)
                                {
                                    EditorGUILayout.BeginHorizontal();

                                    EditorGUI.indentLevel++;

                                    if (values[k].Type != null && values[k].Type.IsSubclassOf(typeof(UnityEngine.Object)))
                                    {
                                        if (values[k].Type.Equals(typeof(UnityEngine.Sprite)))
                                        {
                                            values[k].sprite = (Sprite)EditorGUILayout.ObjectField(values[k].type + ", " + values[k].name, values[k].sprite, typeof(Sprite), false);
                                        }
                                        else
                                        {
                                            values[k].obj = (GameObject)EditorGUILayout.ObjectField(values[k].type + ", " + values[k].name, values[k].obj, typeof(GameObject), false);
                                        }
                                    }
                                    else if (values[k].Type != null && values[k].Type.BaseType == typeof(Enum))
                                    {
                                        EditorGUILayout.LabelField(values[k].type + ", " + values[k].name);

                                        Enum enumData = null;

                                        if (string.IsNullOrEmpty(values[k].value))
                                        {
                                            enumData = (Enum)Enum.Parse(values[k].Type, Enum.GetValues(values[k].Type).GetValue(0).ToString());
                                        }
                                        else
                                        {
                                            enumData = (Enum)Enum.Parse(values[k].Type, values[k].value);
                                        }

                                        if (values[k].Type.HasAttribute(typeof(FlagsAttribute)))
                                        {
                                            enumData = EditorGUILayout.EnumFlagsField(enumData);
                                        }
                                        else
                                        {
                                            enumData = EditorGUILayout.EnumPopup(enumData);
                                        }

                                        values[k].value = enumData.ToString();
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField(values[k].type + ", " + values[k].name);

                                        values[k].value = EditorGUILayout.TextField(values[k].value);
                                    }

                                    EditorGUI.indentLevel--;

                                    EditorGUILayout.EndHorizontal();
                                }

                                if(settingClass.Type != null)
                                {
                                    if (prefabs.ContainsKey(settingClass.Type.Name) == false)
                                    {
                                        List<string> list = Directory.GetFiles(Application.dataPath, $"{settingClass.Type.Name}.prefab", SearchOption.AllDirectories)
                                           .Where(file => file.Contains(@"\Plugins\") == false && file.Contains(@"\TextMesh Pro\") == false && file.Contains(@"\ExternalAssets\") == false).ToList();

                                        prefabs.Add(settingClass.Type.Name, list);
                                    }

                                    if (prefabs[settingClass.Type.Name].Count == 1)
                                    {
                                        if (GUILayout.Button($"Open : {settingClass.typeToString}", GUILayout.ExpandWidth(true)))
                                        {
                                            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(string.Concat(prefabs[settingClass.Type.Name][0].Skip(prefabs[settingClass.Type.Name][0].IndexOf("Assets"))), typeof(GameObject)));
                                        }
                                    }
                                }
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }

            EditorGUI.indentLevel = indent;

            Undo.RecordObject(settingManager, "Change settingManager");
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
