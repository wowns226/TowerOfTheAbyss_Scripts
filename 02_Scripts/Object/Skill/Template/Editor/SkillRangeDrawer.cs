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

using UnityEditor;
using UnityEngine;

namespace ProjectL
{
    [CustomEditor(typeof(SkillRangeData))]
    public class SkillRangeDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            var skillRangeData = (SkillRangeData)serializedObject.targetObject;
            var rangeInfos = skillRangeData.rangeInfos;

            for (int index = 0; index < rangeInfos.Count; index++)
            {
                EditorGUILayout.LabelField($"List Index : {index}");

                var rangeRow = rangeInfos[index].rangeRow;

                if (rangeRow.Length == 0)
                {
                    rangeInfos[index].rangeRow = new SkillRangeModel[SkillRangeData.SKILL_RANGE];
                    return;
                }
                
                for (int i = 0; i < SkillRangeData.SKILL_RANGE; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    for (int j = 0; j < SkillRangeData.SKILL_RANGE; j++)
                    {
                        GUI.color = GetColor(rangeRow[i].rangeData[j]); 

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.MinHeight(30))) 
                        {
                            rangeRow[i].rangeData[j] = !rangeRow[i].rangeData[j];
                            EditorUtility.SetDirty(target);
                        }
                        GUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
            
            serializedObject.ApplyModifiedProperties();
        }

        public Color GetColor(bool isCheck)
        {
            if (isCheck)
                return Color.black;

            return Color.white;
        }
    }
}
