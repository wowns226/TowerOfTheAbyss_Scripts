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

using UnityEngine;

namespace ProjectL
{
    public class GoldPopup : MonoBehaviour
    {
        private RectTransform myRect;
        private int layer;
        
        private void Awake()
        {
            myRect = GetComponent<RectTransform>();
            layer = gameObject.layer;
        }

        private void OnEnable()
        {
            if (D.SelfPlayer)
            {
                D.SelfPlayer.onGoldUp += OnGoldUp;
                D.SelfPlayer.onGoldDown += OnGoldDown;
            }
        }

        private void OnDisable()
        {
            if (D.SelfPlayer)
            {
                D.SelfPlayer.onGoldUp -= OnGoldUp;
                D.SelfPlayer.onGoldDown -= OnGoldDown;
            }
        }

        private void OnGoldUp(float value) => PopupManager.Instance.OpenPopup(PopupType.GoldUp, myRect, value, layer);
        private void OnGoldDown(float value) => PopupManager.Instance.OpenPopup(PopupType.GoldDown, myRect, value * -1, layer);

    }
}
