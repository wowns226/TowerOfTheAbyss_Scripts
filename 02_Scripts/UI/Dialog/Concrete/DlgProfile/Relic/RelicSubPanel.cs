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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class RelicSubPanel : DataContainer
    {
        private bool isActiveFilter;
        private List<RelicInfo> relics = new List<RelicInfo>();

        private Relic focusRelic;
        private Relic FocusRelic
        {
            get => focusRelic;
            set
            {
                focusRelic = value;
                focusRelicSet = focusRelic.RelicSetList;
                this.NotifyObserver();
            }
        }
        private List<RelicSet> focusRelicSet = new List<RelicSet>();

        [SerializeField]
        private Transform relicParent;
        [SerializeField]
        private DefaultSetToggleGroup toggleGroup;
        [SerializeField]
        private Scrollbar relicListScrollbar;
        
        [DataObservable]
        private string RelicName => $"{FocusRelic?.DisplayName}";
        [DataObservable]
        private Sprite RelicIcon => FocusRelic?.Icon;
        [DataObservable]
        private string RelicDescription => FocusRelic?.Description;
        [DataObservable]
        private string RelicSellGold => $"{FocusRelic?.GetSellGold()}";
        [DataObservable]
        private bool IsRelicActive => FocusRelic?.IsActive ?? false;
        [DataObservable]
        private string RelicMouseOverDescription => FocusRelic?.MouseOverDescription;

        [DataObservable]
        private bool IsCommon => FocusRelic && FocusRelic.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => FocusRelic && FocusRelic.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => FocusRelic && FocusRelic.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => FocusRelic && FocusRelic.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => FocusRelic && FocusRelic.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => FocusRelic && FocusRelic.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => FocusRelic && FocusRelic.GradeType == GradeType.Ancient;

        [DataObservable]
        private bool FirstRelicSetExist => focusRelicSet.Count > 0;
        [DataObservable]
        private string FirstRelicSetDisplayName
        {
            get
            {
                if (focusRelicSet?.Count < 1)
                {
                    return string.Empty;
                }

                if (focusRelicSet[0].IsActive)
                {
                    return $"<color=green><b>{FirstRelicSetName} ({FirstRelicSetCurrentActiveCount}/{FirstRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{FirstRelicSetName} ({FirstRelicSetCurrentActiveCount}/{FirstRelicSetMaxActiveCount})";
            }
        }
        private string FirstRelicSetName => focusRelicSet.Count > 0 ? focusRelicSet[0].DisplayName : string.Empty;
        private int FirstRelicSetCurrentActiveCount => focusRelicSet.Count > 0 ? focusRelicSet[0].CurrentActiveRelicCount : 0;
        private int FirstRelicSetMaxActiveCount => focusRelicSet.Count > 0 ? focusRelicSet[0].ActiveRelicCount : 0;
        [DataObservable]
        private string FirstRelicSetDetailDescription => focusRelicSet.Count > 0 ? focusRelicSet[0].DetailDescription : string.Empty;

        [DataObservable]
        private bool SecondRelicSetExist => focusRelicSet.Count > 1;
        [DataObservable]
        private string SecondRelicSetDisplayName
        {
            get
            {
                if (focusRelicSet?.Count < 2)
                {
                    return string.Empty;
                }

                if (focusRelicSet[1].IsActive)
                {
                    return $"<color=green><b>{SecondRelicSetName} ({SecondRelicSetCurrentActiveCount}/{SecondRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{SecondRelicSetName} ({SecondRelicSetCurrentActiveCount}/{SecondRelicSetMaxActiveCount})";
            }
        }
        private string SecondRelicSetName => focusRelicSet.Count > 1 ? focusRelicSet[1].DisplayName : string.Empty;
        private int SecondRelicSetCurrentActiveCount => focusRelicSet.Count > 1 ? focusRelicSet[1].CurrentActiveRelicCount : 1;
        private int SecondRelicSetMaxActiveCount => focusRelicSet.Count > 1 ? focusRelicSet[1].ActiveRelicCount : 1;
        [DataObservable]
        private string SecondRelicSetDetailDescription => focusRelicSet.Count > 1 ? focusRelicSet[1].DetailDescription : string.Empty;

        [DataObservable]
        private bool ThirdRelicSetExist => focusRelicSet.Count > 2;
        [DataObservable]
        private string ThirdRelicSetDisplayName
        {
            get
            {
                if (focusRelicSet?.Count < 3)
                {
                    return string.Empty;
                }

                if (focusRelicSet[2].IsActive)
                {
                    return $"<color=green><b>{ThirdRelicSetName} ({ThirdRelicSetCurrentActiveCount}/{ThirdRelicSetMaxActiveCount})</b></color>";
                }
                
                return $"{ThirdRelicSetName} ({ThirdRelicSetCurrentActiveCount}/{ThirdRelicSetMaxActiveCount})";
            }
        }
        private string ThirdRelicSetName => focusRelicSet.Count > 2 ? focusRelicSet[2].DisplayName : string.Empty;
        private int ThirdRelicSetCurrentActiveCount => focusRelicSet.Count > 2 ? focusRelicSet[2].CurrentActiveRelicCount : 0;
        private int ThirdRelicSetMaxActiveCount => focusRelicSet.Count > 2 ? focusRelicSet[2].ActiveRelicCount : 0;
        [DataObservable]
        private string ThirdRelicSetDetailDescription => focusRelicSet.Count > 2 ? focusRelicSet[2].DetailDescription : string.Empty;

        protected override void Awake()
        {
            base.Awake();
            CreatePlayerAllRelicItems();
        }

        private void OnDestroy()
        {
            relics.Clear();
        }

        private void OnEnable()
        {
            FilterGradeType();
            
            if (FocusRelic != null)
            {
                this.NotifyObserver();
            }
        }

        private void CreatePlayerAllRelicItems()
        {
            Debug.Log("RelicSubPanel.CreatePlayerAllRelicItems()");

            var playerRelics = D.SelfPlayer.RelicBag.AllList;
            int relicsCount = playerRelics.Count;
            
            for (int i = 0; i < relicsCount; i++)
            {
                int index = i;
                ObjectPoolManager.Instance.New("RelicInfo", relicParent, relicSlot =>
                {
                    var localPosition = relicSlot.transform.localPosition;
                    relicSlot.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0);
                    var relicInfo = relicSlot.GetComponent<RelicInfo>();
                    
                    relicInfo.Init(playerRelics[index], toggleGroup);
                    relics.Add(relicInfo);
                    relicInfo.onToggleAction.Add(OnToggleChangeRelicItem);
                    
                    if (index == relicsCount - 1 && FocusRelic == null && relics.Count > 0)
                    {
                        FilterGradeType();
                        toggleGroup.defaultToggle = relics[0].toggle;
                        relics[0].toggle.isOn = true;
                    }
                });
            }
        }

        public void OnToggleActiveFilter(float value)
        {
            isActiveFilter = value > 0.5;

            FilterGradeType();

            relicListScrollbar.value = 1;
        }
        
        private void OnToggleChangeRelicItem(Relic relic)
        {
            FocusRelic = relic;
        }

        private void FilterGradeType()
        {
            relics = relics.OrderByDescending(item => item.Relic.IsActive)
                                     .ThenByDescending(item => item.Relic.GradeType)
                                     .ThenBy(item => item.Relic.GetHashCode()).ToList();

            foreach (var relicInfo in relics)
            {
                relicInfo.transform.SetAsLastSibling();

                if (isActiveFilter)
                {
                    if (relicInfo.Relic.IsActive)
                    {
                        relicInfo.gameObject.SetActive(true);
                    }
                    else
                    {
                        relicInfo.gameObject.SetActive(false);
                    }
                }
                else
                {
                    relicInfo.gameObject.SetActive(true);
                }
            }
        }

        public void OnClickSellRelic()
        {
            Debug.Log($"RelicSubPanel.OnClickSellRelic(), Relic : {FocusRelic}");
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgRelic/Sell/Title");
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgRelic/Sell/Content"),FocusRelic.DisplayName, FocusRelic.GetSellGold());
                dialog.AddOKEvent(SellRelic);
            });
        }

        private void SellRelic()
        {
            D.SelfPlayer.RelicBag.Sell(FocusRelic);
            FilterGradeType();
            this.NotifyObserver();
        }
    }
}