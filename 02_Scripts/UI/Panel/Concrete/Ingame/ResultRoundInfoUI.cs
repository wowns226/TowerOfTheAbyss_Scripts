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

using System.Collections.Generic;
using UnityEngine;

namespace ProjectL
{
    public class ResultRoundInfoUI : InGameUIBase
    {
        [SerializeField] 
        private int relicSlotCount;
        public int RelicSlotCount => relicSlotCount;
        
        [SerializeField]
        private List<SelectRelicInfo> relicItems;
        private SelectRelicInfo selectedRelicInfo;

        protected override void Start()
        {
            base.Start();
            
            relicSlotCount = 3;
            
            relicItems.ForEach(item => item.onToggleAction.Add(OnToggleChange));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            relicItems.ForEach(item => item.onToggleAction.Remove(OnToggleChange));
        }
        
        protected override void Active()
        {
            base.Active();

            var relics = D.SelfRelicBag.GetRandomRelics(RelicSlotCount);

            int index = 0;
            
            foreach (var relic in relics)
            {
                if(relic.IsGradeType == true)
                    relicItems[index].Init(relic, GradeUtil.GetRandomGrade());
                else
                    relicItems[index].Init(relic, GradeType.Special);
                
                index++;
            }
        }

        protected override void InActive()
        {
            base.InActive();

            if (selectedRelicInfo != null)
            {
                selectedRelicInfo.Toggle.isOn = false;
                selectedRelicInfo = null;
            }
        }
        
        public void OnClickedProfileBtn()
        {
            Debug.Log("ResultRoundInfoUI.OnClickedProfileBtn");
            DialogManager.Instance.OpenDialog<DlgProfile>("DlgProfile");
        }

        public void OnClickedContinueBtn()
        {
            Debug.Log("ResultRoundInfoUI.OnClickedContinueBtn()");

            if (selectedRelicInfo == null)
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog =>
                {
                    dialog.Text = $"{Localization.GetLocalizedString("RoundResult/NotSelectRelic")}";
                });

                return;
            }
            
            ActiveSelectedRelic();
            
            PlayRoundLogic.Instance.EnterStatus(RoundLogic.SetupRound);
        }

        public void OnClickedExitBtn()
        {
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("Common/Exit/ToLobby");
                dialog.Content = Localization.GetLocalizedString("Common/Exit/Content");
                dialog.AddOKEvent(() => PlayRoundLogic.Instance.EnterStatus(RoundLogic.EndRound));
            });
        }
        
        private void ActiveSelectedRelic()
        {
            var changedRelic = selectedRelicInfo.ChangedRelic;
            if (changedRelic != null)
            {
                D.SelfPlayer.Gold += changedRelic.GetSellGold();
                D.SelfRelicBag.DeActivate(changedRelic);
            }
            
            D.SelfRelicBag.Activate(selectedRelicInfo.Relic, selectedRelicInfo.GradeType);
        }

        private void OnToggleChange(SelectRelicInfo selectRelicInfo)
        {
            this.selectedRelicInfo = selectRelicInfo;
            relicItems.ForEach(item => item.NotifyObserver());
        }

    }
}
