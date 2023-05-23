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
	[CustomPropertyDrawer(typeof(DataContainerAttribute))]
	public class DataContainerDrawer : PropertyDrawer
	{
		public static DataContainer dataContainer;
		DataContainerAttribute itemAttribute { get { return attribute as DataContainerAttribute; } }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect objectPos = position;
			objectPos.xMax = (position.xMin + position.xMax) / 1.5f;
			position.xMin = (position.xMin + position.xMax) / 1.5f;

			Component component = property.objectReferenceValue as Component;
			GameObject gameObject = component ? component.gameObject : null;
			Component[] components = null;

			GameObject newGameObject = EditorGUI.ObjectField(objectPos, "DataContainer", gameObject, typeof(GameObject), true) as GameObject;
			if (newGameObject != gameObject)
			{
				if (!newGameObject)
					property.objectReferenceValue = null;
				else
				{
					components = newGameObject.GetComponentsInChildren(typeof(DataContainer), true);

					foreach (Component iter in components)
					{
						property.objectReferenceValue = iter;
						if (property.objectReferenceValue == iter)
							break;
					}
				}
			}

			if (newGameObject)
			{
				if (components == null)
				{
					components = newGameObject.GetComponents(typeof(DataContainer));
				}

				List<string> componentNames = new List<string>();
				int index = 0;
				for (int i = 0; i < components.Length; ++i)
				{
					Component iter = components[i];
					componentNames.Add((i + 1).ToString() + "." + iter.GetType().ToString());
					if (iter == property.objectReferenceValue)
						index = i;
				}

				int newIndex = EditorGUI.Popup(position, index, componentNames.ToArray());
				if (newIndex != index)
					property.objectReferenceValue = newIndex >= 0 ? components[newIndex] : null;
			}

			dataContainer = property.objectReferenceValue as DataContainer;

			if (dataContainer && dataContainer.observers.Count == 0)
				dataContainer.AddObserverField();
		}
	}
}
