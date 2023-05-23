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
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ProjectL
{
    [CustomPropertyDrawer(typeof(DataItemAttribute))]
    public class DataItemDrawer : PropertyDrawer
    {
        DataItemAttribute itemAttribute { get { return attribute as DataItemAttribute; } }

        string[] dataNames;
        int dataIndex = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect popupRect = position;
            popupRect.yMax = (position.yMin + position.yMax) / 2f;

            DataContainer dataContainer = DataContainerDrawer.dataContainer;
            if (dataContainer == null)
                return;
            
            string dataName = property.stringValue.Replace(".", "/");

            List<string> dataNameList = new List<string>();

            dataNameList.Add("<None>");

            foreach (string name in dataContainer.observers.Keys)
            {
                dataNameList.Add(name.Replace(".", "/"));
            }
            dataNames = dataNameList.ToArray();

            dataIndex = dataNameList.IndexOf(dataName);
            dataIndex = dataIndex == -1 ? 0 : dataIndex;

            int newDataIndex = EditorGUI.Popup(popupRect, "DataItem" , dataIndex, dataNames);

            if (dataIndex != newDataIndex)
                property.stringValue = newDataIndex > 0 ? dataNames[newDataIndex].Replace("/", ".") : string.Empty;

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
