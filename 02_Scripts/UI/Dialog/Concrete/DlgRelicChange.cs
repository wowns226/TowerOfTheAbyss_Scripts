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
    public class DlgRelicChange : DialogBase
    {
        private Relic newRelic;
        public Relic NewRelic
        {
            get => newRelic;
            set => newRelic = value;
        }
        
        private Relic changedRelic;
        public Relic ChangedRelic
        {
            get => changedRelic;
            set
            {
                changedRelic = value;
            }
        }

        private Relic selectedRelic;
        private Relic SelectedRelic
        {
            get => selectedRelic;
            set
            {
                selectedRelic = value;
                selectedRelicSet = selectedRelic.RelicSetList;
                this.NotifyObserver();
            }
        }
        private List<RelicSet> selectedRelicSet = new List<RelicSet>();
        
        private bool isDecideRelic;
        public CustomAction<Relic> okAction = new CustomAction<Relic>();
        public CustomAction cancelAction = new CustomAction();

        [SerializeField]
        private RelicChangeItemInfo newRelicItemInfo;
        [SerializeField]
        private List<RelicChangeItemInfo> relicChangeItemInfos;
        
        #region Observable

        [DataObservable]
        private string Gold => $"{D.SelfPlayer?.Gold}";
        
        [DataObservable]
        private Sprite NewRelicIcon => NewRelic?.Icon;
        [DataObservable]
        private string NewRelicGrade => $"{NewRelic?.GradeType}";
        [DataObservable]
        private Sprite ChangedRelicIcon => ChangedRelic?.Icon;
        [DataObservable]
        private string ChangedRelicGrade =>  $"{ChangedRelic?.GradeType}";
        [DataObservable]
        private string GetChangedGold => $"+{ChangedRelic?.GetSellGold()}";
        
        [DataObservable]
        private bool IsSelectChangedRelic => ChangedRelic != null;
        
        [DataObservable]
        private Sprite RelicIcon => SelectedRelic?.Icon;
        [DataObservable]
        private string RelicName => SelectedRelic?.DisplayName;
        [DataObservable]
        private string RelicDescription => SelectedRelic?.GetDescription(SelectedRelic.GradeType);
        [DataObservable]
        private string RelicMouseOverDescription => SelectedRelic?.MouseOverDescription;
       
        [DataObservable]
        private bool IsCommon => SelectedRelic && SelectedRelic.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => SelectedRelic && SelectedRelic.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => SelectedRelic && SelectedRelic.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => SelectedRelic && SelectedRelic.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => SelectedRelic && SelectedRelic.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => SelectedRelic && SelectedRelic.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => SelectedRelic && SelectedRelic.GradeType == GradeType.Ancient;

        [DataObservable]
        private bool FirstRelicSetExist => selectedRelicSet.Count > 0;
        [DataObservable]
        private string FirstRelicSetDisplayName
        {
            get
            {
                if (selectedRelicSet?.Count < 1)
                {
                    return string.Empty;
                }

                if (selectedRelicSet[0].IsActive)
                {
                    return $"<color=green><b>{FirstRelicSetName} ({FirstRelicSetCurrentActiveCount}/{FirstRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{FirstRelicSetName} ({FirstRelicSetCurrentActiveCount}/{FirstRelicSetMaxActiveCount})";
            }
        }
        private string FirstRelicSetName => selectedRelicSet.Count > 0 ? selectedRelicSet[0].DisplayName : string.Empty;
        private int FirstRelicSetCurrentActiveCount => selectedRelicSet.Count > 0 ? selectedRelicSet[0].CurrentActiveRelicCount : 0;
        private int FirstRelicSetMaxActiveCount => selectedRelicSet.Count > 0 ? selectedRelicSet[0].ActiveRelicCount : 0;
        [DataObservable]
        private string FirstRelicSetDetailDescription => selectedRelicSet.Count > 0 ? selectedRelicSet[0].DetailDescription : string.Empty;

        [DataObservable]
        private bool SecondRelicSetExist => selectedRelicSet.Count > 1;
        [DataObservable]
        private string SecondRelicSetDisplayName
        {
            get
            {
                if (selectedRelicSet?.Count < 2)
                {
                    return string.Empty;
                }

                if (selectedRelicSet[1].IsActive)
                {
                    return $"<color=green><b>{SecondRelicSetName} ({SecondRelicSetCurrentActiveCount}/{SecondRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{SecondRelicSetName} ({SecondRelicSetCurrentActiveCount}/{SecondRelicSetMaxActiveCount})";
            }
        }
        private string SecondRelicSetName => selectedRelicSet.Count > 1 ? selectedRelicSet[1].DisplayName : string.Empty;
        private int SecondRelicSetCurrentActiveCount => selectedRelicSet.Count > 1 ? selectedRelicSet[1].CurrentActiveRelicCount : 1;
        private int SecondRelicSetMaxActiveCount => selectedRelicSet.Count > 1 ? selectedRelicSet[1].ActiveRelicCount : 1;
        [DataObservable]
        private string SecondRelicSetDetailDescription => selectedRelicSet.Count > 1 ? selectedRelicSet[1].DetailDescription : string.Empty;

        [DataObservable]
        private bool ThirdRelicSetExist => selectedRelicSet.Count > 2;
        [DataObservable]
        private string ThirdRelicSetDisplayName
        {
            get
            {
                if (selectedRelicSet?.Count < 3)
                {
                    return string.Empty;
                }

                if (selectedRelicSet[2].IsActive)
                {
                    return $"<color=green><b>{ThirdRelicSetName} ({ThirdRelicSetCurrentActiveCount}/{ThirdRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{ThirdRelicSetName} ({ThirdRelicSetCurrentActiveCount}/{ThirdRelicSetMaxActiveCount})";
            }
        }
        private string ThirdRelicSetName => selectedRelicSet.Count > 2 ? selectedRelicSet[2].DisplayName : string.Empty;
        private int ThirdRelicSetCurrentActiveCount => selectedRelicSet.Count > 2 ? selectedRelicSet[2].CurrentActiveRelicCount : 0;
        private int ThirdRelicSetMaxActiveCount => selectedRelicSet.Count > 2 ? selectedRelicSet[2].ActiveRelicCount : 0;
        [DataObservable]
        private string ThirdRelicSetDetailDescription => selectedRelicSet.Count > 2 ? selectedRelicSet[2].DetailDescription : string.Empty;

        #endregion

        public override void CloseDialog()
        {
            base.CloseDialog();

            if (isDecideRelic == false)
            {
                cancelAction.Invoke();
            }

            UnregisterEvent();
            
            changedRelic = null;
            okAction.Clear();
            cancelAction.Clear();
        }

        public override void OpenDialog()
        {
            base.OpenDialog();

            isDecideRelic = false;
            
            RegisterEvent();
            InitRelicInfos();

            ChangedRelic = relicChangeItemInfos[0].Relic;

            SelectedRelic = NewRelic;
        }

        private void RegisterEvent()
        {
            D.SelfPlayer.onGoldChanged += OnGoldChange;
        }

        private void UnregisterEvent()
        {
            D.SelfPlayer.onGoldChanged -= OnGoldChange;
        }

        private void OnGoldChange(float value) => this.NotifyObserver("Gold");
        
        private void InitRelicInfos()
        {
            newRelicItemInfo.Init(newRelic);
            newRelicItemInfo.onToggleAction.Add(OnToggleChangeNewRelic);

            var relics = D.SelfRelicBag.FilterList;
            for (int i = 0; i < relics.Count; i++)
            {
                relicChangeItemInfos[i].Init(relics[i]);
                relicChangeItemInfos[i].onToggleAction.Add(OnToggleChangeChangeRelic);
            }
        }

        private void OnToggleChangeNewRelic(Relic relic) => SelectedRelic = relic;

        private void OnToggleChangeChangeRelic(Relic relic)
        {
            ChangedRelic = relic;
            SelectedRelic = relic;
        }

        public void ClickOK()
        {
            isDecideRelic = true;
            
            okAction.Invoke(ChangedRelic);
            CloseDialog();
        }
    }
}