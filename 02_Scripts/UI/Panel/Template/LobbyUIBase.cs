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
    public class LobbyUIBase : DataContainer
    {
        [SerializeField]
        private LobbyLogic showStatus;

        [SerializeField]
        private CanvasGroup canvasGroup;

        protected virtual void Start()
        {
            if (PlayLobbyLogic.Instance)
                PlayLobbyLogic.Instance.AddStatusEvent(showStatus, Toggle);
        }

        protected virtual void OnDestroy()
        {
            if (PlayLobbyLogic.Instance)
                PlayLobbyLogic.Instance.RemoveStatusEvent(showStatus, Toggle);
        }

        protected void Toggle(bool isOn)
        {
            if (isOn)
                Active();
            else
                InActive();

            if (canvasGroup)
                ToggleCanvasGroup(isOn);
            else
                ToggleGameObject(isOn);
        }

        private void ToggleCanvasGroup(bool isOn)
        {
            if (isOn)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
            }
        }

        private void ToggleGameObject(bool isOn)
        {
            gameObject.SetActive(isOn);
        }

        protected virtual void Active() { }
        protected virtual void InActive() { }
    }
}
