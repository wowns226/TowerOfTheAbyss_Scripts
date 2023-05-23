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
    public enum RoundType
    {
        Story = 0, // Default
        Infinity = 1,
    }

    public class RoundManager : MonoSingleton<RoundManager>
    {   
        private RoundType roundType = RoundType.Story; 
        public RoundType RoundType { 
            get => roundType; 
            set
            {
                roundType = value;
                onRoundTypeChange.Invoke(value);
            }
        }

        [SerializeField]
        private readonly int maxStoryStage = 100;
        public int MaxStoryStage => maxStoryStage;
        
        [HideInInspector]
        public IngameMapScene ingameMapScene;
        public ChapterType Chapter => ingameMapScene.ToChapterType();

        public CustomAction<RoundType> onRoundTypeChange = new CustomAction<RoundType>();

        private void Start()
        {
            onRoundTypeChange.Add(OnRoundTypeChange);
        }

        private void OnRoundTypeChange(RoundType roundType)
        {
            if (roundType == RoundType.Story)
            {
                SoundManager.Instance.PlayBGM(BGMType.Lobby);
            }
            else
            {
                SoundManager.Instance.PlayBGM(BGMType.LobbyInfinity);
            }
        }
    }
}
