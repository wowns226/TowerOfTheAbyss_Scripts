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
using UnityEngine.UI;

namespace ProjectL
{
    public class DlgToolTip : DialogBase
    {
        private static DlgToolTip instance;

        [SerializeField]
        private Image fadeImage;

        [SerializeField]
        private float fadeSpeed;

        [SerializeField]
        private float typingSpeed;

        [SerializeField]
        private float autoCloseTime;

        private Coroutine showCoroutine;
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
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }

        public override void OpenDialog()
        {
            if (instance != null)
            {
                instance.CloseDialog();
            }
            instance = this;

            base.OpenDialog();

            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            SetAlpha(0);
            showCoroutine = StartCoroutine(Fade(true));
        }

        public override void CloseDialog()
        {
            instance = null;
            SetAlpha(0);

            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            base.CloseDialog();
        }

        #region Fade In/Out Anim

        private IEnumerator Fade(bool isOn)
        {
            if (isOn)
            {
                while (fadeImage.color.a <= 1)
                {
                    SetImageColor(true);
                    yield return new WaitForSecondsRealtime(fadeSpeed);
                }

                SetAlpha(1);

                yield return new WaitForSecondsRealtime(autoCloseTime);
            }

            while (fadeImage.color.a >= 0)
            {
                SetImageColor(false);
                yield return null;
            }

            SetAlpha(0);

            CloseDialog();
        }

		private void SetImageColor(bool isOn)
        {
            float alpha = fadeImage.color.a + fadeSpeed * ((isOn) ? 1 : -1);
            SetAlpha(alpha);
        }

        private void SetAlpha(float alpha)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }

        #endregion
    }
}
