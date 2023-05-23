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
    public class LobbyInfoUI : LobbyUIBase
    {
        [DataObservable]
        private string UserNickname => D.SelfUser != null ? D.SelfUser.NickName : string.Empty;

        [DataObservable]
        private string NotReadyDevelop => Localization.GetLocalizedString("DlgToolTip/Notice/NotYetDevelop");

        private void OnEnable()
        {
            this.NotifyObserver();
        }

        protected override void Start()
        {
            base.Start();

            User.onChangedNickName += SetTextUserNickName;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            User.onChangedNickName -= SetTextUserNickName;
        }

        public void SetTextUserNickName(User user)
        {
            if(user != D.SelfUser) return;

            Debug.Log("SetTextUserNickName");
            this.NotifyObserver("UserNickname");
        }

        #region Buttons Event

        public void OnClickedGameStartBtn()
        {
            Debug.Log("LobbyInfoUI.OnClickedGameStartBtn");
            DialogManager.Instance.OpenDialog<DlgStory>("DlgStory");
        }

        public void OnClickedProfileBtn()
        {
            Debug.Log("LobbyInfoUI.OnClickedProfileBtn"); 
            DialogManager.Instance.OpenDialog<DlgProfile>("DlgProfile");
        }

        public void OnClickedCustomUnitBtn()
        {
            Debug.Log("LobbyInfoUI.OnClickedCustomUnitBtn");
            DialogManager.Instance.OpenDialog<DlgCustomUnit>("DlgCustomUnit");
        }

        public void OnClickedSettingBtn()
        {
            Debug.Log("LobbyInfoUI.OnClickedSettingBtn");
            DialogManager.Instance.OpenDialog<DlgSettings>("DlgSettings");
        }

        public void OnClickedExitBtn()
        {
            Debug.Log("LobbyInfoUI.OnClickedExitBtn");

            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog => 
            {
                dialog.Title = Localization.GetLocalizedString("Common/Exit/ToGame");
                dialog.Content = Localization.GetLocalizedString("Common/Exit/Content");
                dialog.AddOKEvent(() =>Application.Quit());
            });
        }

        #endregion
    }
}
