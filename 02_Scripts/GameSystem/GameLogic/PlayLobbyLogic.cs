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
    [Flags]
    public enum LobbyLogic
    {
        LoadScene = 1 << 0,
        LoginScene = 1 << 1,
        SetupLobby = 1 << 2,
        MainLobby = 1 << 3,
        StartGame = 1 << 4,
    }

    public class PlayLobbyLogic : LogicBase<LobbyLogic>
    {
        #region LoadScene
        private void EnterLoadScene()
        {
            Debug.Log("EnterLoadScene");
            
            if (D.SelfUser != null)
            {
                Debug.Log("EnterLoadScene(), already logged in");

                GotoSetupLobby();
            }
        }

        private void GotoSetupLobby()
        {
            Debug.Log("GotoSetupLobby");

            DialogManager.Instance.CloseAllDialogs();

            DialogManager.Instance.OpenDialog<DlgFadeInOut>("DlgFadeInOut", dlgFade =>
            {
                dlgFade.Fade(false);
            });

            EnterStatus(LobbyLogic.SetupLobby);
        }

        private IEnumerator ProcessLoadScene()
        {
            Debug.Log("ProcessLoadScene");

            var downloader = SetupAssetDataAsync();
            while (downloader.MoveNext())
            {
                yield return downloader.Current;
            }

            bool sfxMuteTemp = SoundManager.Instance.SFXAudioSource.mute;
            SoundManager.Instance.SFXAudioSource.mute = true;
            
            var dialogCacher = DialogManager.Instance.CacheAllDialog();
            while (dialogCacher.MoveNext())
            {
                yield return dialogCacher.Current;
            }

            StartCoroutine(ReleaseSFXSound(sfxMuteTemp));
        }

        IEnumerator ReleaseSFXSound(bool sfxMuteTemp)
        {
            Debug.Log("ReleaseSFXSound Pre");
            
            while (DialogManager.Instance.DialogCount > 0 || SoundManager.Instance.SFXAudioSource.isPlaying)
            {
                yield return null;
            }
            
            Debug.Log("ReleaseSFXSound Post");
            
            SoundManager.Instance.SFXAudioSource.mute = sfxMuteTemp;
        }

        private IEnumerator SetupAssetDataAsync()
        {
            Debug.Log("SetupAssetDataAsync");

            AssetDownloader.Instance.Download();

            while (AssetDownloader.Instance.IsDownloadComplete == false)
            {
                yield return null;
            }
            
            AssetDownloader.Instance.Load();

            while (AssetDownloader.Instance.IsLoadComplete == false)
            {
                yield return null;
            }
        }

        private void LeaveLoadScene()
        {
            Debug.Log("LeaveLoadScene");
        }
        #endregion

        #region LoginScene
        private void EnterLoginScene()
        {
            Debug.Log("EnterLoginScene");
        }

        private IEnumerator ProcessLoginScene()
        {
            Debug.Log("ProcessLoginScene");

            if(D.SelfUser == null)
            {
                var setupUser = SetupUserAsync();
                while (setupUser.MoveNext())
                {
                    yield return setupUser.Current;
                }
            }

            while (string.IsNullOrEmpty(D.SelfUser.NickName))
            {
                yield return null;
            }
        }

        private IEnumerator SetupUserAsync()
        {
            Debug.Log("SetupUserAsync");

            while (D.SelfPlayerGroup == null)
            {
                yield return null;
            }

            //스팀 id 받아올 수 있을 때 아래 추가
            //ulong id = Steam.GetID();
            //UserFactory.Instance.Create(ulong.Parse(id));
            //if(id == -1) ?
            UserFactory.Instance.CreateGuest();

            PlayerFactory.Instance.Create(PlayerType.Ally, 1);
            PlayerFactory.Instance.Create(PlayerType.Enemy, 10000);

            while (D.SelfUser.IsLoadDone == false)
            {
                yield return null;
            }
        }

        private void LeaveLoginScene()
        {
            Debug.Log("LeaveLoginScene");
        }
        #endregion

        #region SetupLobby
        private void EnterSetupLobby()
        {
            Debug.Log("EnterSetupLobby");

            RoundManager.Instance.RoundType = RoundManager.Instance.RoundType;
        }

        private IEnumerator ProcessSetupLobby()
        {
            Debug.Log("ProcessSetupLobby");

             yield return null;
        }

        private void LeaveSetupLobby()
        {
            Debug.Log("LeaveSetupLobby");
        }
        #endregion

        #region MainLobby
        private void EnterMainLobby()
        {
            Debug.Log("EnterMainLobby");
        }

        private IEnumerator ProcessMainLobby()
        {
            Debug.Log("ProcessMainLobby");

            while(true)
            {
                yield return null;
            }
        }

        private void LeaveMainLobby()
        {
            Debug.Log("LeaveMainLobby");
        }
        #endregion

        #region StartGame
        private void EnterStartGame()
        {
            Debug.Log("EnterStartGame");
            DialogManager.Instance.CloseAllDialogs();
        }

        private IEnumerator ProcessStartGame()
        {
            Debug.Log("ProcessStartGame");

            yield return null;
        }

        private void LeaveStartGame()
        {
            Debug.Log("LeaveStartGame");
            SceneManager.Instance.GotoIngameScene();
        }
        #endregion
    }
}
