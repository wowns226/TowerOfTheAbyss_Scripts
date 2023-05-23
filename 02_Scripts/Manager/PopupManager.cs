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
using DamageNumbersPro;
using UnityEngine;

namespace ProjectL
{
    public enum PopupType
    {
        Damage = 0,
        CriticalDamage = 1,
        Heal = 2,
        GoldUp = 3,
        GoldDown = 4,
        Timer = 5,
    }
    
    [Serializable]
    public class PopupData
    {
        public PopupType popupType;
        public DamageNumber damageNumber;
    }

    public class PopupManager : MonoSingleton<PopupManager>
    {
        [SerializeField]
        private List<PopupData> popupDatas = new List<PopupData>();

        public void OpenPopup(PopupType popupType, Vector3 position, float value)
        {
            var popup = popupDatas.Find(popup => popup.popupType == popupType);

            if (popup == null)
            {
                Debug.Log($"Popup is null, Type : {popupType.ToString()}, pos : {position.ToString()}, value : {value}");
                return;
            }

            popup.damageNumber.Spawn(position, value);
        }
        
        public void OpenPopup(PopupType popupType, RectTransform rectParent, float value, int layer)
        {
            var popup = popupDatas.Find(popup => popup.popupType == popupType);

            if (popup == null)
            {
                Debug.Log($"Popup is null, Type : {popupType.ToString()}, pos : {rectParent.anchoredPosition.ToString()}, value : {value}");
                return;
            }

            var damageNumber = popup.damageNumber.Spawn(Vector3.zero, value);
            damageNumber.SetAnchoredPosition(rectParent, new Vector2(rectParent.rect.width * -0.5f + 50f, 5f));
            damageNumber.gameObject.layer = layer;
        }
    }
}
