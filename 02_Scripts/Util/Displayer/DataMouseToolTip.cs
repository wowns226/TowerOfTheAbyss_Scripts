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

using UnityEngine.EventSystems;

namespace ProjectL
{
    public class DataMouseToolTip : DataObserver, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        private string text = string.Empty;

        public override void UpdateData(object dataValue)
        {
            text = $"{dataValue}";

            if (dlgToolTipBox != null)
            {
                dlgToolTipBox.Text = text;
            }
        }
        
        private static DlgToolTipBox dlgToolTipBox;
        
        public void Init(Skill skill)
        {
            if (dlgToolTipBox != null)
            {
                dlgToolTipBox.CloseDialog();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (dlgToolTipBox != null)
            {
                dlgToolTipBox.CloseDialog();
            }
            
            DialogManager.Instance.OpenDialog<DlgToolTipBox>("DlgToolTipBox", dlg =>
            {
                dlgToolTipBox = dlg;
                dlgToolTipBox.Text = text;
                
                dlgToolTipBox.SetPosition(eventData);
            });
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (dlgToolTipBox == null)
            {
                return;
            }
            
            dlgToolTipBox.SetPosition(eventData);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (dlgToolTipBox != null)
            {
                dlgToolTipBox.CloseDialog();
                dlgToolTipBox = null;
            }
        }

    }
}