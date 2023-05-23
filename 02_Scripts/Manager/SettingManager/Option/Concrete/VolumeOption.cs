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
using UnityEngine;

namespace ProjectL
{
    [Serializable]
    public class VolumeData : ISaveAndLoadToJson<VolumeData>, IEquatable<VolumeData>, ICloneable
    {
        public VolumeData()
        {
            masterVolume = 50f;
            musicVolume = 50f;
            sfxVolume = 50f;
            muteMasterVolume = false;
        }
        
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public bool muteMasterVolume;

        public bool Equals(VolumeData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return masterVolume.Equals(other.masterVolume)
                   && musicVolume.Equals(other.musicVolume)
                   && sfxVolume.Equals(other.sfxVolume)
                   && muteMasterVolume == other.muteMasterVolume;
        }

        public object Clone()
        {
            return new VolumeData
                   {
                       masterVolume = this.masterVolume,
                       musicVolume = this.musicVolume,
                       sfxVolume = this.sfxVolume,
                       muteMasterVolume = this.muteMasterVolume
                   };
        }
    }

    public class VolumeOption : Option<VolumeData>
    {
        public VolumeOption(bool isApplyImmediately) : base(isApplyImmediately) { }
        
        protected override void _Reset()
        {
        }

        protected override void _Apply()
        {
            base._Apply();
            SetMasterMute(CurrentData.muteMasterVolume);
            SetMasterVolume(CurrentData.masterVolume);
            SetBGMVolume(CurrentData.musicVolume);
            SetSFXVolume(CurrentData.sfxVolume);
        }
        
        public void SetMasterMute(bool isOn)
        {
            Debug.Log($"Set Mute : {isOn}");

            SoundManager.Instance.AllBGMAudioList.ForEach(x => x.mute = isOn);
            SoundManager.Instance.SFXAudioSource.mute = isOn;
        }

        public void SetMasterVolume(float value)
        {
            var volume = value * 0.2f - 20;
            if (volume < -19f)
            {
                volume = -40f;
            }
            
            Debug.Log($"Master Set Volume : {volume}");

            SoundManager.Instance.MasterMixer.SetFloat("Master", volume);
        }

        public void SetBGMVolume(float value)
        {
            var volume = value * 0.2f - 20;
            if (volume < -19f)
            {
                volume = -40f;
            }

            Debug.Log($"BGM Set Volume : {volume}");

            SoundManager.Instance.MasterMixer.SetFloat("BGM", volume);
        }

        public void SetSFXVolume(float value)
        {
            var volume = value * 0.2f - 20;
            if (volume < -19f)
            {
                volume = -40f;
            }
            
            Debug.Log($"SFX Set Volume : {volume}");

            SoundManager.Instance.MasterMixer.SetFloat("SFX", volume);
        }
    }
}