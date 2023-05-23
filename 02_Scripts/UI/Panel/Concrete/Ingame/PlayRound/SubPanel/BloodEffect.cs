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
    public class BloodEffect : InGameUIBase
    {
        [SerializeField]
        private CanvasGroup effectCanvasGroup;

        [SerializeField]
        private AudioSource lowHPAudio;

        protected override void Active()
        {
            base.Active();

            D.SelfPlayer.Castle.onChangedHp += OnChangedHp;
        }

        protected override void InActive()
        {
            base.InActive();

            if (D.SelfPlayer.Castle == null)
            {
                return;
            }
            
            D.SelfPlayer.Castle.onChangedHp -= OnChangedHp;
        }

        private void OnChangedHp(float value) 
        {
            if (D.SelfPlayer == null || D.SelfPlayer.Castle == null)
            {
                return;
            }
                
            float hpRate = value / D.SelfPlayer.Castle.MaxHp;

            if(hpRate > 0.3)
            {
                StopSound();
                return;
            }

            effectCanvasGroup.alpha = (0.3f - hpRate) * 2;
            PlaySound(hpRate);
        }

        private void PlaySound(float hpRate)
        {
            lowHPAudio.volume = 1 - hpRate * 2;
            
            if (lowHPAudio.isPlaying == false)
            {
                lowHPAudio.Play();
            }
        }

        private void StopSound()
        {
            if (lowHPAudio.isPlaying)
            {
                lowHPAudio.Stop();
            }
        }
    }
}
