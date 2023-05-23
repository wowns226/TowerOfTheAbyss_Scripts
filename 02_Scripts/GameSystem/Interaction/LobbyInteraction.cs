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
    public class LobbyInteraction : MonoBehaviour
    {
        private void Update()
        {
            if (PlayLobbyLogic.Instance.Status != LobbyLogic.MainLobby)
            {
                return;
            }

            if (DialogManager.Instance.TopDialog)
            {
                if (Input.GetKeyDown(GetKey(InputType.Exit)) && !DialogManager.Instance.TopDialog.IsBlockEscape)
                {
                    DialogManager.Instance.TopDialog.CloseDialog();
                }

                return;
            }

            if (Input.GetKeyDown(GetKey(InputType.Exit)))
            {
                DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
                {
                    dialog.Title = Localization.GetLocalizedString("EXIT_TO_GAME");
                    dialog.Content = Localization.GetLocalizedString("Do you want to exit?");
                    dialog.AddOKEvent(Application.Quit);
                });
            }

            if (Input.GetKeyDown(GetKey(InputType.Profile)))
            {
                DialogManager.Instance.OpenDialog<DlgProfile>("DlgProfile", dlg => dlg.OnClickProfile());
            }

            if (Input.GetKeyDown(GetKey(InputType.System)))
            {
                DialogManager.Instance.OpenDialog("DlgSettings");
            }
        }

        private KeyCode GetKey(InputType inputType) => SettingManager.Instance.KeyBindingOption.GetKey(inputType);
    }
}
