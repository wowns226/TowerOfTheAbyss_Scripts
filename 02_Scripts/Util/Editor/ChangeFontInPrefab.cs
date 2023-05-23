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

using TMPro;
using UnityEditor;
using UnityEngine;

namespace ProjectL
{
    public class ChangeFontInPrefab : EditorWindow
    {
        private GameObject prefab;
        private TMP_FontAsset newFont;

        [MenuItem("Tools/Custom/Change Font In Prefab's TMP")]
        private static void OpenWindow()
        {
            GetWindow<ChangeFontInPrefab>();
        }

        private void OnGUI()
        {
            prefab = EditorGUILayout.ObjectField("Target Prefab", prefab, typeof(GameObject), true) as GameObject;
            newFont = EditorGUILayout.ObjectField("New Font", newFont, typeof(TMP_FontAsset), false) as TMP_FontAsset;

            if(GUILayout.Button("Change Font"))
            {
                Debug.Log("Button Clicked");

                if(prefab == null)
                {
                    Debug.Log("Prefab is Null");
                    return;
                }

                ChangeFontInChildren(prefab.transform);

                EditorUtility.SetDirty(prefab);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("Finish Changed");
            }
        }

        private void ChangeFontInChildren(Transform parent)
        {
            foreach(Transform child in parent)
            {
                TMP_Text textPro = child.GetComponent<TMP_Text>();

                if(textPro != null)
                {
                    textPro.font = newFont;
                }

                ChangeFontInChildren(child);
            }
        }
    }
}