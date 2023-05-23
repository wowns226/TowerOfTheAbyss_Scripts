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

using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.EventSystems;
using Screen = UnityEngine.Device.Screen;

namespace ProjectL
{
    public class DlgItemInfo : DialogBase
    { 
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private RectTransform area;
        [SerializeField]
        private RectTransform moveRect;
        [SerializeField]
        private UIElementSound openAudioSound;
        
        private Sprite icon;
        [DataObservable]
        public Sprite Icon
        {
            get => icon;
            set
            {
                icon = value;
                this.NotifyObserver("Icon");
            }
        }

        private string displayName;
        [DataObservable]
        public string Name
        {
            get => displayName;
            set
            {
                displayName = value;
                this.NotifyObserver("Name");
            }
        }

        private string shortDescription;
        [DataObservable]
        public string ShortDescription
        {
            get => shortDescription;
            set
            {
                shortDescription = value;
                this.NotifyObserver("ShortDescription");
            }
        }

        private string fullDescription;
        [DataObservable]
        public string FullDescription
        {
            get => fullDescription;
            set
            {
                fullDescription = value;
                this.NotifyObserver("FullDescription");
            }
        }

        private int price;
        [DataObservable]
        public int Price
        {
            get => price;
            set
            {
                price = value;
                this.NotifyObserver("Price");
                this.NotifyObserver("IsShowPrice");
            }
        }

        [DataObservable]
        private bool IsShowPrice => price > 0;

        private bool isLeft, isTop;
        [DataObservable]
        private bool IsLeftTop => isLeft && isTop;
        [DataObservable]
        private bool IsRightTop => !isLeft && isTop;
        [DataObservable]
        private bool IsLeftBottom => isLeft && !isTop;
        [DataObservable]
        private bool IsRightBottom => !isLeft && !isTop;

        public override void OpenDialog()
        {
            base.OpenDialog();
            
            canvasGroup.alpha = 0;
            
            openAudioSound.PlayOneShot();
        }
        
        public void SetPosition(PointerEventData eventData)
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            var rectSize = moveRect.sizeDelta;
            var movePosition = Vector2.zero;
            var position = eventData.position;
            
            isLeft = position.x + rectSize.x > screenSize.x;
            isTop = position.y + rectSize.y < screenSize.y;

            if (IsLeftTop)
            {
                movePosition = new Vector2(position.x - rectSize.x * 0.5f, position.y + rectSize.y * 0.5f);
            }
            else if (IsRightTop)
            {
                movePosition = new Vector2(position.x + rectSize.x * 0.5f, position.y + rectSize.y * 0.5f);
            }
            else if (IsLeftBottom)
            {
                movePosition = new Vector2(position.x - rectSize.x * 0.5f, position.y - rectSize.y * 0.5f);
            }
            else if (IsRightBottom)
            {
                movePosition = new Vector2(position.x + rectSize.x * 0.5f, position.y - rectSize.y * 0.5f);
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, movePosition, eventData.enterEventCamera,
                out var targetPosition);
            
            moveRect.anchoredPosition = targetPosition;
            this.NotifyObserver();
            
            canvasGroup.alpha = 1;
        }
    }
}