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
    public class SetupRoundInfoUI : InGameUIBase
    {
        [DataObservable]
        public string RoundNumber => $"Round {D.SelfRound?.Stage ?? 1}";

        [DataObservable]
        public bool IsRoundSet => D.SelfRound?.IsSetRound ?? false;

        protected override void Active()
        {
            base.Active();

            D.SelfEnemyPlayer.onAddUnit += OnAddUnit;
        }

        protected override void InActive()
        {
            base.InActive();
            
            D.SelfEnemyPlayer.onAddUnit -= OnAddUnit;
        }

        private void OnAddUnit(Unit unit)
        {
            this.NotifyObserver();
        }
        
        public void OnClickedStartBtn()
        {
            if(IsRoundSet == false)
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog =>
                {
                    dialog.Text = Localization.GetLocalizedString("SetupRoundInfoUI/RoundSetYet/Content");
                });
                return;
            }

            PlayRoundLogic.Instance.NextStatus();
        }
    }
}
