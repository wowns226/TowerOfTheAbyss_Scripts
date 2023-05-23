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
using UnityEngine;

namespace ProjectL
{
    public class DlgWarning : DialogBase
    {
        [SerializeField]
        private float typingSpeed;

        private Coroutine typingCoroutine;

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
        }

    }
}
