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
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    [Flags]
    public enum RoundLogic
    {
        LoadRound = 1 << 0,
        SetupRound = 1 << 1,
        PlayRound = 1 << 2,
        ResultRound = 1 << 3,
        EndRound = 1 << 4,
    }

    public class PlayRoundLogic : LogicBase<RoundLogic>
    {
        #region LoadRound
        private void EnterLoadRound()
        {
            Debug.Log("EnterLoadRound");

            DialogManager.Instance.OpenDialog<DlgFadeInOut>("DlgFadeInOut", dlgFade =>
            {
                dlgFade.Fade(false);
            });

            PlayBGM();
            D.SelfPlayer.Gold = 3000; // 테스트용
        }

        private void PlayBGM()
        {
            switch (RoundManager.Instance.ingameMapScene)
            {
                case IngameMapScene.ElfCastle:
                    SoundManager.Instance.PlayBGM(BGMType.ElfCastle);
                    break;
                case IngameMapScene.PassageOfTime:
                    SoundManager.Instance.PlayBGM(BGMType.PassageOfTime);
                    break;
                case IngameMapScene.UnderWorldCastle:
                    SoundManager.Instance.PlayBGM(BGMType.UnderWorldCastle);
                    break;
            }
        }

        private IEnumerator ProcessLoadRound()
        {
            Debug.Log("ProcessLoadRound");

            while(D.SelfBoard == null)
            {
                yield return null;
            }

            CreateCastle();
            CreateFirstHero();

            while (D.SelfPlayer.Castle == null || SceneManager.Instance.SceneLoaded == false)
            {
                yield return null;
            }

            DialogManager.Instance.CloseAllDialogs();
        }

        private void LeaveLoadRound()
        {
            Debug.Log("LeaveLoadRound");
        }
        #endregion

        #region SetupRound
        private void EnterSetupRound()
        {
            Debug.Log("EnterSetupRound");

            new Round(D.SelfRound != null ? D.SelfRound.Stage + 1 : 1, RoundManager.Instance.RoundType, RoundManager.Instance.ingameMapScene);

            D.SelfPlayer.FullHealAllUnits();
            
            TimeManager.Instance.PauseTimeScale();
        }

        private IEnumerator ProcessSetupRound()
        {
            Debug.Log("ProcessSetupRound");

            while (true)
            {
                yield return null;
            }
        }

        private void LeaveSetupRound()
        {
            Debug.Log("LeaveSetupRound");
            
            TimeManager.Instance.ReturnTimeScale();
        }
        #endregion

        #region PlayRound
        private void EnterPlayRound()
        {
            Debug.Log("EnterPlayRound");
        }

        private IEnumerator ProcessPlayRound()
        {
            Debug.Log("ProcessPlayRound");

            while (D.SelfPlayerGroup.enemyPlayers.All(player => player.units.Count == 0) == false)
            {
                if (D.SelfPlayer.Castle == null)
                {
                    DialogManager.Instance.OpenDialog("DlgFailStage");
                }
                
                yield return null;
            }
        }

        private void LeavePlayRound()
        {
            Debug.Log("LeavePlayRound");
        }
        #endregion

        #region ResultRound
        private void EnterResultRound()
        {
            Debug.Log("EnterResultRound");

            D.SelfRound.SuccessStage();
            ClearEnemyUnits();
            ClearPointBuff();
            ClearPointState();
            
            TimeManager.Instance.PauseTimeScale();
        }

        private IEnumerator ProcessResultRound()
        {
            Debug.Log("ProcessResultRound");

            if (TestManager.Instance.IsDemoMode && D.SelfRound.Stage >= 50)
            {
                DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
                {
                    dialog.Title = Localization.GetLocalizedString("Demo/Success/Title");
                    dialog.Content = Localization.GetLocalizedString("Demo/Success/Content");
                    dialog.AddOKEvent(() => EnterStatus(RoundLogic.EndRound));
                    dialog.AddCancelEvent(() => EnterStatus(RoundLogic.EndRound));
                    dialog.IsShowCancelButton = false;
                });
            }

            if (D.SelfRound.RoundType == RoundType.Story && D.SelfRound.Stage == RoundManager.Instance.MaxStoryStage)
            {
                //TODO 스토리 연출 실행
                
                EnterStatus(RoundLogic.EndRound);
            }
            
            while (true)
            {
                yield return null;
            }
        }

        private void LeaveResultRound()
        {
            Debug.Log("LeaveResultRound");
            
            TimeManager.Instance.ReturnTimeScale();
        }
        #endregion

        #region EndRound
        private void EnterEndRound()
        {
            Debug.Log("EnterEndRound");

            if (D.SelfRound.RoundType == RoundType.Infinity)
            {
                D.SelfUser.SetRecord(D.SelfRound.IngameMapScene, D.SelfRound.SuccessedStage);
            }

            D.SelfPlayer.Clear();
            D.SelfEnemyPlayer.Clear();
            D.SelfTechnologyBag.ClearActivate();
            D.SelfRelicBag.ClearActivate();
            D.SelfRound = null;
        }

        private IEnumerator ProcessEndRound()
        {
            Debug.Log("ProcessEndRound");

            yield return null;
        }

        private void LeaveEndRound()
        {
            Debug.Log("LeaveEndRound");

            SceneManager.Instance.GotoLobbyScene();
        }
        #endregion

        private static void CreateCastle()
        {
            Debug.Log("PlayRoundLogic.CreateCastle()");

            BuildingFactory.Instance.CreateBuilding("Castle", D.SelfBoard.CastlePoint);
        }

        private static void CreateFirstHero()
        {
            Debug.Log("PlayRoundLogic.CreateFirstHero()");
            D.SelfPlayer.CreateHeroUnit();
        }

        private void ClearEnemyUnits() => D.SelfEnemyPlayer.ClearUnit();
        private void ClearPointBuff() => D.SelfBoard.ClearAllPointBuff();
        private void ClearPointState() => D.SelfBoard.ClearSetAllPointStateEffect();
    }
}
