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
using System.Collections.Generic;
using Michsky.UI.Dark;
using UnityEngine;

namespace ProjectL
{
    public class DlgProfile : DialogBase
    {
        [SerializeField]
        private Transform recordParent;
        [SerializeField]
        private Transform achievementParent;
        [SerializeField]
        private MainPanelManager mainPanelManager;

        private List<GameObject> InfoItems = new List<GameObject>();

        [DataObservable]
        private string Nickname => D.SelfUser?.NickName;
        [DataObservable]
        private string Title => D.SelfUser?.Achievement;
        [DataObservable]
        private int Gold => D.SelfPlayer?.Gold ?? 0;

        [DataObservable]
        public bool IsIngame => PlayRoundLogic.Instance != null;
        
        public override void OpenDialog()
        {
            base.OpenDialog();

            CreateAllRecordItems();
            CreateAllAchievementItems();

            D.SelfPlayer.onGoldChanged += OnGoldChange;
            
            TimeManager.Instance.PauseTimeScale();

            this.NotifyObserver();
        }

        public override void CloseDialog()
        {
            base.CloseDialog();
            
            D.SelfPlayer.onGoldChanged -= OnGoldChange;

            TimeManager.Instance.ReturnTimeScale();
            
            InfoItems.ForEach(item => ObjectPoolManager.Instance.Return(item));
        }

        public void OnClickClose()
        {
            OnClickProfile();

            CloseDialog();
        }

        private void CreateAllRecordItems()
        {
            Debug.Log("DlgProfile.CreateAllRecordItems()");

            foreach (IngameMapScene chapter in Enum.GetValues(typeof(IngameMapScene)))
            {
                int chapterRecord = D.SelfUser.GetRecord(chapter);

                if (chapterRecord == 0)
                {
                    continue;
                }

                ObjectPoolManager.Instance.New("RecordItemInfo", recordParent, itemSlot =>
                {
                    itemSlot.transform.localPosition = Vector3.zero;
                    itemSlot.GetComponent<RecordItemInfo>().Init(chapter, chapterRecord);
                    InfoItems.Add(itemSlot);
                });
            }
        }

        private void CreateAllAchievementItems()
        {
            Debug.Log("DlgProfile.CreateAllAchievementItems()");

            /* 스팀 연결 되면 생성
            ObjectPoolManager.Instance.New("AchievementItemInfo", achievementParent, (itemSlot) =>
            {
                itemSlot.transform.localPosition = Vector3.zero;
                itemSlot.GetComponent<AchievementItemInfo>().Init();
                InfoItems.Add(itemSlot);
            });
            */
        }

        public void OnClickProfile() => mainPanelManager.OpenPanel("Profile");
        public void OnClickTechnology() => mainPanelManager.OpenPanel("Technology");
        public void OnClickUnit() => mainPanelManager.OpenPanel("Unit");
        public void OnClickBuilding() => mainPanelManager.OpenPanel("Building");
        public void OnClickRelic() => mainPanelManager.OpenPanel("Relic");

        private void OnGoldChange(float value)
        { 
            this.NotifyObserver("Gold");
        } 
    }
}
