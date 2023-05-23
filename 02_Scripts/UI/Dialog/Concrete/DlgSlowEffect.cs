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

namespace ProjectL
{
    public class DlgSlowEffect : DialogBase
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float fadeSpeed;

        [SerializeField]
        private UIElementSound openAudioSource;
        
        private Coroutine showCoroutine;

        public override void OpenDialog()
        {
            base.OpenDialog();

            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            SetAlpha(0);
            openAudioSource.PlayOneShot();
            showCoroutine = StartCoroutine(Fade(true));
        }


        public override void CloseDialog()
        {
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            SetAlpha(0);

            base.CloseDialog();
        }

        #region Fade In/Out Anim

        private IEnumerator Fade(bool isOn)
        {
            if (isOn)
            {
                while (canvasGroup.alpha <= 1f)
                {
                    FadeAlpha(isOn);
                    yield return null;
                }

                SetAlpha(1f);
            }
            else
            {
                while (canvasGroup.alpha >= 0)
                {
                    FadeAlpha(isOn);
                    yield return null;
                }

                SetAlpha(0);

                base.CloseDialog();
            }
        }

        private void FadeAlpha(bool isOn)
        {
            float alpha = canvasGroup.alpha + Time.deltaTime / fadeSpeed * ((isOn) ? 1 : -1);
            SetAlpha(alpha);
        }

        private void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }

        #endregion
    }
}
