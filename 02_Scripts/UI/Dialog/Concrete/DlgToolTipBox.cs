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

using System.Collections;
using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectL
{
    public class DlgToolTipBox : DialogBase
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private RectTransform area;
        [SerializeField]
        private RectTransform moveRect;
        [SerializeField]
        private float typingSpeed;
        [SerializeField]
        private UIElementSound openAudioSound;

        private Coroutine typingCoroutine;
        
        private bool isTop;
        [DataObservable]
        private bool IsTop => isTop;

        private string text;
        [DataObservable]
        public string Text
        {
            get => text;
            set
            {
                text = value;
                this.NotifyObserver("Text");
            }
        }

        public string TypingText
        {
            set
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                }

                typingCoroutine = StartCoroutine(Typing(value));
            }
        }

        private IEnumerator Typing(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Text += text[i];
                yield return new WaitForSeconds(typingSpeed);
            }

            typingCoroutine = null;
        }

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
            
            isTop = position.y + rectSize.y < screenSize.y;

            if (IsTop)
            {
                movePosition = new Vector2(position.x, position.y + rectSize.y * 0.5f);
            }
            else 
            {
                movePosition = new Vector2(position.x, position.y - rectSize.y * 0.5f);
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, movePosition, eventData.enterEventCamera,
                out var targetPosition);
            
            moveRect.anchoredPosition = targetPosition;
            this.NotifyObserver();
            
            canvasGroup.alpha = 1;
        }
    }
}
