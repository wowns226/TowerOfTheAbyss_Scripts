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
using UnityEngine.EventSystems;

namespace ProjectL
{
    public class SkillInfoPopup : DataContainer, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        private Skill skill;

        [DataObservable]
        private Sprite Icon => skill?.Icon;
        
        [DataObservable]
        private string DisplayName => skill?.DisplayName;
        
        [DataObservable]
        private string Description => skill?.Description;

        private static DlgItemInfo dlgItemInfo;
        
        public void Init(Skill skill)
        {
            this.skill = skill;
            this.NotifyObserver();
            
            if (dlgItemInfo != null)
            {
                dlgItemInfo.CloseDialog();
                dlgItemInfo = null;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (dlgItemInfo != null)
            {
                dlgItemInfo.CloseDialog();
                dlgItemInfo = null;
            }

            DialogManager.Instance.OpenDialog<DlgItemInfo>("DlgItemInfo", dlg =>
            {
                dlg.Icon = Icon;
                dlg.Name = DisplayName;
                dlg.FullDescription = Description;

                dlgItemInfo = dlg;

                dlgItemInfo.SetPosition(eventData);
            });
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (dlgItemInfo == null)
            {
                return;
            }
            
            dlgItemInfo.SetPosition(eventData);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (dlgItemInfo != null)
            {
                dlgItemInfo.CloseDialog();
                dlgItemInfo = null;
            }
        }

    }
}