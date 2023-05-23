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
using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectL
{
    public enum LobbyUIScene
    {
        GameLobbyScene = 0,
    }

    public enum IngameUIScene
    {
        IngameScene = 0,
    }

    [Flags]
    public enum IngameMapScene
    {
        ElfCastle = 0,
        PassageOfTime = 1,
        UnderWorldCastle = 2,
    }
    
    [Flags]
    public enum ChapterType
    {
        All = 1 << 0,
        ElfCastle = 1 << 1,
        PassageOfTime = 1 << 2,
        UnderWorldCastle = 1 << 3,
    }
    
    public static class IngameMapSceneExtension
    {
        public static ChapterType ToChapterType(this IngameMapScene ingameMapScene)
        {
            return (ChapterType)Enum.Parse(typeof(ChapterType), ingameMapScene.ToString());
        }
    }
    
    public class SceneManager : MonoSingleton<SceneManager>
    {
        [SerializeField]
        private UIElementSound chapterStartSound;
        
        public IngameMapScene IngameMapScene { get; set; }

        private int currentLoadSceneCount;
        private int loadSceneCount;

        private bool sceneLoaded;
        public bool SceneLoaded => sceneLoaded;

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoadComplete;
        }

        public void GotoLobbyScene()
        {
            Debug.Log("SceneManager.GotoLobbyScene()");

            DialogManager.Instance.OpenDialog<DlgFadeInOut>("DlgFadeInOut", dlgFade =>
            {
                dlgFade.Fade(true, GotoLobbySceneAfterFade);
            });
        }

        public void GotoIngameScene()
        {
            Debug.Log($"SceneManager.GotoIngameScene(), Scene : {IngameMapScene.ToString()}");

            RoundManager.Instance.ingameMapScene = IngameMapScene;
            DialogManager.Instance.OpenDialog<DlgFadeInOut>("DlgFadeInOut", dlgFade =>
            {
                dlgFade.Fade(true, () => GotoIngameSceneAfterFade());
            });
            
            chapterStartSound.PlayOneShot();
        }

        private void GotoLobbySceneAfterFade()
        {
            SetLoadSceneCheckValues(1);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LobbyUIScene.GameLobbyScene.ToString(), LoadSceneMode.Single);
        }

        private void GotoIngameSceneAfterFade()
        {
            SetLoadSceneCheckValues(2);
            DialogManager.Instance.OpenDialog("DlgLoading");
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(IngameUIScene.IngameScene.ToString(), LoadSceneMode.Single);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(IngameMapScene.ToString(), LoadSceneMode.Additive);
        }

        private void SceneLoadComplete(Scene arg0, LoadSceneMode arg1)
        {
            currentLoadSceneCount++;
            if(currentLoadSceneCount == loadSceneCount)
            {
                sceneLoaded = true;
            }

            Debug.Log($"SceneManager.SceneLoadComplete(), Scene Count : {currentLoadSceneCount}");
        }

        private void SetLoadSceneCheckValues(int count)
        {
            sceneLoaded = false;
            currentLoadSceneCount = 0;
            loadSceneCount = count;
        }

    }
}
