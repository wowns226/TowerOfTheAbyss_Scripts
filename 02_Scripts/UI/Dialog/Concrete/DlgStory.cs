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

namespace ProjectL
{
    public class DlgStory : DialogBase
    {
        [DataObservable]
        private bool IsStoryMode => RoundManager.Instance.RoundType == RoundType.Story;
        [DataObservable]
        private string RoundTypeName => $"{RoundManager.Instance.RoundType}";

        public void OnChangeRoundType(float value)
        {
            if (value > 0.5)
            {
                RoundManager.Instance.RoundType = ProjectL.RoundType.Infinity;
            }
            else
            {
                RoundManager.Instance.RoundType = ProjectL.RoundType.Story;
            }
            
            this.NotifyObserver();
        }

        public override void OpenDialog()
        {
            base.OpenDialog();
            this.NotifyObserver();
        }

        public void OnClickClose() => CloseDialog();

        public void OnClickStartGame(int scene)
        {
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgStory/GameStart/Title");
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgStory/GameStart/Content"), Localization.GetLocalizedString(((IngameMapScene)scene).ToString()));

                dialog.AddOKEvent(() =>
                {
                    SceneManager.Instance.IngameMapScene = (IngameMapScene)scene;
                    PlayLobbyLogic.Instance.EnterStatus(LobbyLogic.StartGame);
                });

            });

        }
    }
}
