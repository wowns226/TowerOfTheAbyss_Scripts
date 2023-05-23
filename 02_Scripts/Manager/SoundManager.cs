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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace ProjectL
{
    public enum BGMType
    {
        Lobby = 0,
        LobbyInfinity = 1,
        ElfCastle = 2,
        PassageOfTime = 3,
        UnderWorldCastle = 4,
        Ending = 5,
    }

    [Serializable]
    public class AudioData
    {
        public BGMType bgmType;
        public List<AudioSource> audioSources;

        public bool IsPlaying => audioSources.Any(x => x.isPlaying);

        public void Play() => audioSources.ForEach(x => x.Play());
        public void Stop() => audioSources.ForEach(x => x.Stop());
    }
    
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField]
        private List<AudioData> audioDatas;
        public List<AudioSource> AllBGMAudioList
        {
            get
            {
                var result = new List<AudioSource>();
                
                audioDatas.ForEach(x => result.AddRange(x.audioSources));

                return result;
            }
        }

        [SerializeField]
        private AudioSource sfxAudioSource;
        public AudioSource SFXAudioSource => sfxAudioSource;
        
        [SerializeField]
        private AudioMixer masterMixer;
        public AudioMixer MasterMixer => masterMixer;

        private List<AudioData> playlist = new List<AudioData>();
        private AudioData playingSource;
        
        public bool IsPlaying { get; private set; }
        public bool IsRandomPlay { get; set; } = true;
        
        public BGMType CurrentBGMType { get; private set; }
        
        public void PlayBGM(BGMType bgmType)
        {
            StopBGM(CurrentBGMType);
            CurrentBGMType = bgmType;
            
            var bgms = audioDatas.FindAll(x => x.bgmType == bgmType);
            playlist.AddRange(bgms);
            
            if (IsPlaying == false)
            {
                StartCoroutine(PlayBGM());
            }
        }
        
        IEnumerator PlayBGM()
        {
            Debug.Log($"SoundManager.PlayBGM(), playlist Count : {playlist.Count}");
            
            IsPlaying = true;

            if (playlist.Count == 0)
            {
                yield break;
            }
            
            int index = FindNextPlayingIndex(-1);
            playingSource = playlist[index];
            playingSource.Play();
            
            while (playlist.Count > 0)
            {
                yield return new WaitForSecondsRealtime(1.0f);
                
                if(playingSource.IsPlaying) 
                {
                    continue;
                }

                yield return new WaitForSecondsRealtime(5.0f); //바로 다음 노래가 나오면 어색해서 5초 대기
                
                index = FindNextPlayingIndex(index);

                playingSource = playlist[index];
                playingSource.Play();
            }

            IsPlaying = false;
        }

        private int FindNextPlayingIndex(int index)
        {
            if (IsRandomPlay)
            {
                index = UnityEngine.Random.Range(0, playlist.Count);
            }
            else
            {
                index++;

                if (index >= playlist.Count)
                {
                    index = 0;
                }
            }

            return index;
        }

        public void StopAllBGM()
        {
            playlist.ForEach(audioData => audioData.Stop());
            playlist.Clear();
        }
        
        public void StopBGM(BGMType bgmType)
        {
            Debug.Log($"{bgmType} Audio Stop");

            var bgms = playlist.FindAll(x => x.bgmType == bgmType);

            bgms.ForEach(audioData =>
                {
                    audioData.Stop();

                    playlist.Remove(audioData);
                }
            );
        }

        public void PlayOneShotSFX(AudioClip audioClip)
        {
            Debug.Log($"{audioClip?.name} Audio Start");
            sfxAudioSource?.PlayOneShot(audioClip);
        }
    }
}

