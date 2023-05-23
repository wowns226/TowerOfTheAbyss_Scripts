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
    public class TechnologySubPanel : DataContainer
    {
        [SerializeField]
        private Transform technologyParent;
        [SerializeField]
        private DefaultSetToggleGroup toggleGroup;

        [SerializeField]
        private Animator technologyAnimator;
        [SerializeField]
        private Animator firstTripodAnimator;
        [SerializeField]
        private Animator secondTripodAnimator;
        [SerializeField]
        private Animator thirdTripodAnimator;
        
        private Technology focusTechnology;
        private Technology FocusTechnology
        {
            get => focusTechnology;
            set
            {
                focusTechnology = value;
                this.NotifyObserver();
            }
        }

        private List<TechnologyInfo> technologyItems = new List<TechnologyInfo>();
        
        #region Observable

        
        [DataObservable]
        private bool IsTechnologyBuy => (FocusTechnology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.Activate;
        
        [DataObservable]
        private string TechnologyName => FocusTechnology?.DisplayName ?? string.Empty;
        [DataObservable]
        private string TechnologyShortDescription => FocusTechnology?.ShortDescription ?? string.Empty;
        [DataObservable]
        private string TechnologyFullDescription => FocusTechnology?.FullDescription ?? string.Empty;

        
        [DataObservable]
        private bool IsTechnologyFirstTripodBuy => (FocusTechnology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.FirstTripod;
        
        [DataObservable]
        private bool IsTechnologyFirstTripodFirstExist => (FocusTechnology?.firstTripods?.Count ?? 0) > 0;
        [DataObservable]
        private bool IsTechnologyFirstTripodFirstActive => IsTechnologyFirstTripodFirstExist && FocusTechnology.FirstTripodIndex <= 0;
        [DataObservable]
        private string TechnologyFirstTripodFirstName => IsTechnologyFirstTripodFirstExist ? FocusTechnology.firstTripods[0].DisplayName : string.Empty;
        [DataObservable]
        private string TechnologyFirstTripodFirstDescription => IsTechnologyFirstTripodFirstExist ? FocusTechnology.firstTripods[0].Description : string.Empty;
        
        [DataObservable]
        private bool IsTechnologyFirstTripodSecondExist => (FocusTechnology?.firstTripods?.Count ?? 0) > 1;
        [DataObservable]
        private bool IsTechnologyFirstTripodSecondActive => IsTechnologyFirstTripodSecondExist && FocusTechnology.FirstTripodIndex == 1;
        [DataObservable]
        private string TechnologyFirstTripodSecondName => IsTechnologyFirstTripodSecondExist ? FocusTechnology.firstTripods[1].DisplayName : string.Empty;
        [DataObservable]
        private string TechnologyFirstTripodSecondDescription => IsTechnologyFirstTripodSecondExist ? FocusTechnology.firstTripods[1].Description : string.Empty;

        
        [DataObservable]
        private bool IsTechnologySecondTripodBuy => (FocusTechnology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.SecondTripod;

        [DataObservable]
        private bool IsTechnologySecondTripodFirstExist => (FocusTechnology?.secondTripods?.Count ?? 0) > 0;
        [DataObservable]
        private bool IsTechnologySecondTripodFirstActive => IsTechnologySecondTripodFirstExist && FocusTechnology.SecondTripodIndex <= 0;
        [DataObservable]
        private string TechnologySecondTripodFirstName => IsTechnologySecondTripodFirstExist ? FocusTechnology.secondTripods[0].DisplayName : string.Empty;
        [DataObservable]
        private string TechnologySecondTripodFirstDescription => IsTechnologySecondTripodFirstExist ? FocusTechnology.secondTripods[0].Description : string.Empty;

        [DataObservable]
        private bool IsTechnologySecondTripodSecondExist => (FocusTechnology?.secondTripods?.Count ?? 0) > 1;
        [DataObservable]
        private bool IsTechnologySecondTripodSecondActive => IsTechnologySecondTripodSecondExist && FocusTechnology.SecondTripodIndex == 1;
        [DataObservable]
        private string TechnologySecondTripodSecondName => IsTechnologySecondTripodSecondExist ? FocusTechnology.secondTripods[1].DisplayName : string.Empty;
        [DataObservable]
        private string TechnologySecondTripodSecondDescription => IsTechnologySecondTripodSecondExist ? FocusTechnology.secondTripods[1].Description : string.Empty;

        
        [DataObservable]
        private bool IsTechnologyThirdTripodBuy => (FocusTechnology?.Status ?? TechnologyActiveStatus.None) >= TechnologyActiveStatus.ThirdTripod;

        [DataObservable]
        private bool IsTechnologyThirdTripodFirstExist => (FocusTechnology?.thirdTripods?.Count ?? 0) > 0;
        [DataObservable]
        private bool IsTechnologyThirdTripodFirstActive => IsTechnologyThirdTripodFirstExist && FocusTechnology.ThirdTripodIndex <= 0;
        [DataObservable]
        private string TechnologyThirdTripodFirstName => IsTechnologyThirdTripodFirstExist ? FocusTechnology.thirdTripods[0].DisplayName : string.Empty;
        [DataObservable]
        private string TechnologyThirdTripodFirstDescription => IsTechnologyThirdTripodFirstExist ? FocusTechnology.thirdTripods[0].Description : string.Empty;

        [DataObservable]
        private bool IsTechnologyThirdTripodSecondExist => (FocusTechnology?.thirdTripods?.Count ?? 0) > 1;
        [DataObservable]
        private bool IsTechnologyThirdTripodSecondActive => IsTechnologyThirdTripodSecondExist && FocusTechnology.ThirdTripodIndex == 1;
        [DataObservable]
        private string TechnologyThirdTripodSecondName => IsTechnologyThirdTripodSecondExist ? FocusTechnology.thirdTripods[1].DisplayName : string.Empty;
        [DataObservable]
        private string TechnologyThirdTripodSecondDescription => IsTechnologyThirdTripodSecondExist ? FocusTechnology.thirdTripods[1].Description : string.Empty;

        
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            CreatePlayerAllTechnologyItems();
        }

        private void OnDestroy()
        {
            technologyItems.Clear();
        }

        private void OnEnable()
        {
            if (FocusTechnology != null)
            {
                this.NotifyObserver();
            }
        }

        private void CreatePlayerAllTechnologyItems()
        {
            Debug.Log("TechnologySubPanel.CreatePlayerAllTechnologyItems()");

            var technologies = D.SelfPlayer.TechnologyBag.AllList;

            foreach (var technology in technologies)
            {
                ObjectPoolManager.Instance.New("TechnologyInfo", technologyParent, technologySlot =>
                {
                    technologySlot.transform.localPosition = Vector3.zero;
                    var technologyInfo = technologySlot.GetComponent<TechnologyInfo>();
                    
                    technologyInfo.Init(technology, toggleGroup);
                    technologyInfo.onToggleAction += OnChangedToggleTechnology;
                    technologyItems.Add(technologyInfo);
                    
                    if (FocusTechnology == null && technologyItems.Count > 0)
                    {
                        toggleGroup.defaultToggle = technologyItems[0].toggle;
                        technologyItems[0].toggle.isOn = true;
                    }
                });
            }
        }

        private void OnChangedToggleTechnology(Technology technology)
        {
            Debug.Log($"TechnologySubPanel.OnChangeToggleTechnology, technology : {technology.Name}");
            FocusTechnology = technology;
            
            UnlockTechnology(false);
            UnlockFirstTripod(false);
            UnlockSecondTripod(false);
            UnlockThirdTripod(false);
        }
        
        public void OnClickActivate()
        {
            Debug.Log("TechnologySubPanel.OnClickActivate()");

            if (FocusTechnology == null)
            {
                Debug.Log("TechnologySubPanel.OnClickActivate(), focusTechnology is null");
                return;
            }

            if (FocusTechnology.Status >= TechnologyActiveStatus.Activate)
            {
                Debug.Log($"TechnologySubPanel.OnClickActivate(), Status is same : {FocusTechnology.Status.ToString()}");
                return;
            }

            int price = FocusTechnology.ActivatePrice;
            if (D.SelfPlayer.Gold < price)
            {
                Debug.Log($"TechnologySubPanel.OnClickActivate(), Player Coin : {D.SelfPlayer.Gold.ToString()}, Price : {price.ToString()}");
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("DlgTechnology/Active/NotEnoughGold")} {price}"; });
                return;
            }
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgTechnology/Active/Title");
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgTechnology/Active/Content"),FocusTechnology.DisplayName, price);
                dialog.AddOKEvent(() => ActiveTechnology(price));
            });
        }

        private void ActiveTechnology(int price)
        {
            if (D.SelfPlayer.Gold < price)
            {
                Debug.Log($"TechnologySubPanel.ActiveTechnology(), Player Coin : {D.SelfPlayer.Gold.ToString()}, Price : {price.ToString()}");
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("DlgTechnology/Active/NotEnoughGold")} {price}"; });
                return;
            }
            
            D.SelfPlayer.Gold -= price;
            D.SelfTechnologyBag.Activate(FocusTechnology);
            technologyItems.ForEach(item => item.NotifyObserver());
            UnlockTechnology(true);
        }

        public void ActiveFirstTripod()
        {
            int price = FocusTechnology.FirstTripodPrice;

            if (IsTechnologyBuy == false)
            {
                return;
            }
            
            if (FocusTechnology.Status >= TechnologyActiveStatus.FirstTripod)
            {
                Debug.Log($"TechnologySubPanel.ActiveFirstTripod(), Status is same : {FocusTechnology.Status.ToString()}");
                return;
            }
            
            if (D.SelfPlayer.Gold < price)
            {
                Debug.Log($"TechnologySubPanel.ActiveFirstTripod(), Player Coin : {D.SelfPlayer.Gold.ToString()}, Price : {price.ToString()}");
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("DlgTechnology/Active/NotEnoughGold")} {price}"; });
                return;
            }
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgTechnology/ActiveTripod/Title");
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgTechnology/ActiveFirstTripod/Content"), price);
                dialog.AddOKEvent(() => _ActiveFirstTripod(price));
            });
        }

        private void _ActiveFirstTripod(int price)
        {
            D.SelfPlayer.Gold -= price;
            FocusTechnology.ActiveNextStatus();
            technologyItems.ForEach(item => item.NotifyObserver());
            UnlockFirstTripod(true);
        }

        public void ActiveSecondTripod()
        {
            int price = FocusTechnology.SecondTripodPrice;

            if (IsTechnologyFirstTripodBuy == false)
            {
                return;
            }
            
            if (FocusTechnology.Status >= TechnologyActiveStatus.SecondTripod)
            {
                Debug.Log($"TechnologySubPanel.ActiveSecondTripod(), Status is same : {FocusTechnology.Status.ToString()}");
                return;
            }
            
            if (D.SelfPlayer.Gold < price)
            {
                Debug.Log($"TechnologySubPanel.ActiveSecondTripod(), Player Coin : {D.SelfPlayer.Gold.ToString()}, Price : {price.ToString()}");
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("DlgTechnology/Active/NotEnoughGold")} {price}"; });
                return;
            }
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgTechnology/ActiveTripod/Title");
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgTechnology/ActiveSecondTripod/Content"), price);
                dialog.AddOKEvent(() => _ActiveSecondTripod(price));
            });
        }

        private void _ActiveSecondTripod(int price)
        {
            D.SelfPlayer.Gold -= price;
            FocusTechnology.ActiveNextStatus();
            technologyItems.ForEach(item => item.NotifyObserver());
            UnlockSecondTripod(true);
        }

        public void ActiveThirdTripod()
        {
            int price = FocusTechnology.ThirdTripodPrice;

            if (IsTechnologySecondTripodBuy == false)
            {
                return;
            }
            
            if (FocusTechnology.Status >= TechnologyActiveStatus.ThirdTripod)
            {
                Debug.Log($"TechnologySubPanel.ActiveThirdTripod(), Status is same : {FocusTechnology.Status.ToString()}");
                return;
            }
            
            if (D.SelfPlayer.Gold < price)
            {
                Debug.Log($"TechnologySubPanel.ActiveThirdTripod(), Player Coin : {D.SelfPlayer.Gold.ToString()}, Price : {price.ToString()}");
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("DlgTechnology/Active/NotEnoughGold")} {price}"; });
                return;
            }
            
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("DlgTechnology/ActiveTripod/Title");
                dialog.Content = string.Format(Localization.GetLocalizedString("DlgTechnology/ActiveThirdTripod/Content"), price);
                dialog.AddOKEvent(() => _ActiveThirdTripod(price));
            });
        }

        private void _ActiveThirdTripod(int price)
        {
            D.SelfPlayer.Gold -= price;
            FocusTechnology.ActiveNextStatus();
            technologyItems.ForEach(item => item.NotifyObserver());
            UnlockThirdTripod(true);
        }

        private void UnlockTechnology(bool isUnlock) => technologyAnimator.SetBool("Unlock", isUnlock);
        private void UnlockFirstTripod(bool isUnlock) => firstTripodAnimator.SetBool("Unlock", isUnlock);
        private void UnlockSecondTripod(bool isUnlock) => secondTripodAnimator.SetBool("Unlock", isUnlock);
        private void UnlockThirdTripod(bool isUnlock) => thirdTripodAnimator.SetBool("Unlock", isUnlock);

        public void OnToggleFirstTripodFirst(bool isOn) { if(isOn) FocusTechnology?.SelectTripod(TechnologyActiveStatus.FirstTripod, 0); }
        public void OnToggleFirstTripodSecond(bool isOn) { if(isOn) FocusTechnology?.SelectTripod(TechnologyActiveStatus.FirstTripod, 1); }
        public void OnToggleSecondTripodFirst(bool isOn) { if(isOn) FocusTechnology?.SelectTripod(TechnologyActiveStatus.SecondTripod, 0); }
        public void OnToggleSecondTripodSecond(bool isOn) { if(isOn) FocusTechnology?.SelectTripod(TechnologyActiveStatus.SecondTripod, 1); }
        public void OnToggleThirdTripodFirst(bool isOn) { if(isOn) FocusTechnology?.SelectTripod(TechnologyActiveStatus.ThirdTripod, 0); }
        public void OnToggleThirdTripodSecond(bool isOn) { if(isOn) FocusTechnology?.SelectTripod(TechnologyActiveStatus.ThirdTripod, 1); }

    }
}