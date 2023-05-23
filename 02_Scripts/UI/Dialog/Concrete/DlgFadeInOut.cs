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
using System.Collections;
using UnityEngine;

namespace ProjectL
{
    public class DlgFadeInOut : DialogBase
    {
        [SerializeField]
        private Animator fadeAnimator;

        private Coroutine animationCoroutine;

        public void Fade(bool isOn, Action finishCallback = null)
        {
            Debug.Log($"DlgFadeInOut, Fade : {isOn}");

            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            if (isOn)
            {
                fadeAnimator.Play("FadeIn");
            }
            else
            {
                fadeAnimator.Play("FadeOut");
            }

            if(animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }

            animationCoroutine = StartCoroutine(FinishAnimation(isOn, finishCallback));
        }

        private IEnumerator FinishAnimation(bool isOn, Action finishCallback = null)
        {
            while(fadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

            finishCallback?.Invoke();

            if(isOn == false)
            {
                CloseDialog();
            }
        }

    }
}
