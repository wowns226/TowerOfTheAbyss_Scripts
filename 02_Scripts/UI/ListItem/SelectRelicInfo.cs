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
using UnityEngine.UI;

namespace ProjectL
{
    public class SelectRelicInfo : DataContainer
    {
        private Relic relic;
        public Relic Relic
        {
            get => relic;
            private set
            { 
                relic = value;
                relicSet = Relic.RelicSetList;
                changedRelic = null;
            }
        }

        private Relic changedRelic;
        public Relic ChangedRelic => changedRelic;
        
        private GradeType gradeType;
        public GradeType GradeType => gradeType;

        private List<RelicSet> relicSet = new List<RelicSet>();

        public CustomAction<SelectRelicInfo> onToggleAction = new CustomAction<SelectRelicInfo>();

        [SerializeField]
        private Toggle toggle;
        public Toggle Toggle => toggle;
        
        #region Observable
        
        [DataObservable]
        private bool ToggleIsOn => Toggle.isOn;
        
        [DataObservable]
        private Sprite RelicIcon => Relic?.Icon;
        [DataObservable]
        private string RelicName => Relic?.DisplayName;
        [DataObservable]
        private string RelicDescription => Relic?.GetDescription(GradeType);
        [DataObservable]
        private string RelicSellGold
        {
            get
            {
                if (Relic == null)
                {
                    return string.Empty;
                }

                return $"+{(Relic.IsActive ? Relic.GetSellGold() : 0)}";
            }
        } 
        
        [DataObservable]
        private bool IsActive => Relic?.IsActive ?? false;

        [DataObservable]
        private bool IsHoldRelicCommon => Relic?.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsHoldRelicRare => Relic?.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsHoldRelicUnique => Relic?.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsHoldRelicEpic => Relic?.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsHoldRelicSpecial => Relic?.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsHoldRelicLegendary => Relic?.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsHoldRelicAncient => Relic?.GradeType == GradeType.Ancient;
        
        [DataObservable]
        private bool IsCommon => GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => GradeType == GradeType.Ancient;

        [DataObservable]
        private bool FirstRelicSetExist => relicSet.Count > 0;
        [DataObservable]
        private string FirstRelicSetDisplayName
        {
            get
            {
                if (relicSet?.Count < 1)
                {
                    return string.Empty;
                }

                if (relicSet[0].IsActive)
                {
                    return $"<color=green><b>{FirstRelicSetName} ({FirstRelicSetCurrentActiveCount}/{FirstRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{FirstRelicSetName} ({FirstRelicSetCurrentActiveCount}/{FirstRelicSetMaxActiveCount})";
            }
        }
        private string FirstRelicSetName => relicSet.Count > 0 ? relicSet[0].DisplayName : string.Empty;
        private int FirstRelicSetCurrentActiveCount => relicSet.Count > 0 ? relicSet[0].CurrentActiveRelicCount : 0;
        private int FirstRelicSetMaxActiveCount => relicSet.Count > 0 ? relicSet[0].ActiveRelicCount : 0;
        [DataObservable]
        private string FirstRelicSetDetailDescription => relicSet.Count > 0 ? relicSet[0].DetailDescription : string.Empty;

        [DataObservable]
        private bool SecondRelicSetExist => relicSet.Count > 1;
        [DataObservable]
        private string SecondRelicSetDisplayName
        {
            get
            {
                if (relicSet?.Count < 2)
                {
                    return string.Empty;
                }

                if (relicSet[1].IsActive)
                {
                    return $"<color=green><b>{SecondRelicSetName} ({SecondRelicSetCurrentActiveCount}/{SecondRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{SecondRelicSetName} ({SecondRelicSetCurrentActiveCount}/{SecondRelicSetMaxActiveCount})";
            }
        }
        private string SecondRelicSetName => relicSet.Count > 1 ? relicSet[1].DisplayName : string.Empty;
        private int SecondRelicSetCurrentActiveCount => relicSet.Count > 1 ? relicSet[1].CurrentActiveRelicCount : 1;
        private int SecondRelicSetMaxActiveCount => relicSet.Count > 1 ? relicSet[1].ActiveRelicCount : 1;
        [DataObservable]
        private string SecondRelicSetDetailDescription => relicSet.Count > 1 ? relicSet[1].DetailDescription : string.Empty;

        [DataObservable]
        private bool ThirdRelicSetExist => relicSet.Count > 2;
        [DataObservable]
        private string ThirdRelicSetDisplayName
        {
            get
            {
                if (relicSet?.Count < 3)
                {
                    return string.Empty;
                }

                if (relicSet[2].IsActive)
                {
                    return $"<color=green><b>{ThirdRelicSetName} ({ThirdRelicSetCurrentActiveCount}/{ThirdRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{ThirdRelicSetName} ({ThirdRelicSetCurrentActiveCount}/{ThirdRelicSetMaxActiveCount})";
            }
        }
        private string ThirdRelicSetName => relicSet.Count > 2 ? relicSet[2].DisplayName : string.Empty;
        private int ThirdRelicSetCurrentActiveCount => relicSet.Count > 2 ? relicSet[2].CurrentActiveRelicCount : 0;
        private int ThirdRelicSetMaxActiveCount => relicSet.Count > 2 ? relicSet[2].ActiveRelicCount : 0;
        [DataObservable]
        private string ThirdRelicSetDetailDescription => relicSet.Count > 2 ? relicSet[2].DetailDescription : string.Empty;

        #endregion

        public void Init(Relic relic, GradeType gradeType)
        {
            Relic = relic;
            this.gradeType = gradeType;
            
            this.NotifyObserver();
        }

        public void OnToggleChange(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            
            var activeRelics = D.SelfRelicBag.FilterList;
            var activeSameRelic = activeRelics.Find(relic => relic.GetType() == Relic.GetType());
            int activeCount = activeRelics.Count;
            int maxCount = D.SelfPlayer.MaxRelicCount;

            if (activeSameRelic == null)
            {
                if (activeCount >= maxCount)
                {
                    DialogManager.Instance.OpenDialog<DlgRelicChange>("DlgRelicChange", (dlg) =>
                    {
                        dlg.NewRelic = Relic;
                        dlg.okAction.Add(ExecuteToggleEvent);
                        dlg.okAction.Add(OkSeletedRelic);
                        dlg.cancelAction.Add(CancelSelectedRelic);
                    });

                    return;
                }
            }

            ExecuteToggleEvent();
        }

        private void ExecuteToggleEvent(Relic relic = null)
        {
            Debug.Log($"SelectRelicInfo.ExecuteToggleEvent(), Relic : {Relic.name}");
            onToggleAction?.Invoke(this);
        }

        private void OkSeletedRelic(Relic relic)
        {
            changedRelic = relic;
            this.NotifyObserver();
        }

        private void CancelSelectedRelic()
        {
            Toggle.isOn = false;
            this.NotifyObserver();
            onToggleAction?.Invoke(null);
        }
    }
}