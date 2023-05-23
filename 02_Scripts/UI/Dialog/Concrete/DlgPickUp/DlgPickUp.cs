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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.Dark;
using UnityEngine;

namespace ProjectL
{
    public class DlgPickUp : DialogBase
    {
        [SerializeField]
        private MainPanelManager mainPanelManager;
        [SerializeField]
        private List<PickUpItemInfo> pickUpItems;
        
        [DataObservable]
        private string Gold => D.SelfPlayer?.Gold.ToString();
        
        [DataObservable]
        private bool IsAllOpen => pickUpItems.All(item => item.gameObject.activeInHierarchy == false || item.IsActive);

        [DataObservable]
        private string UnitPickUpCostOne => $"{D.SelfPlayer.UnitPickUpCost}";
        [DataObservable]
        private bool IsEnoughMoneyUnitPickUpOne => D.SelfPlayer.UnitPickUpCost <= D.SelfPlayer.Gold;
        [DataObservable]
        private string UnitPickUpCostTen => $"{D.SelfPlayer.UnitPickUpCost * 10}";
        [DataObservable]
        private bool IsEnoughMoneyUnitPickUpTen => D.SelfPlayer.UnitPickUpCost * 10 <= D.SelfPlayer.Gold;

        public override void CloseDialog()
        {
            base.CloseDialog();

            ClearPickUpItems();
        }

        public void OpenPanel(string panel)
        {
            mainPanelManager.OpenPanel(panel);
        }
        
        public void PickUpUnits(int count)
        {
            ClearPickUpItems();

            for (int i = 0; i < count; i++)
            {
                int index = i;
                
                pickUpItems[index].gameObject.SetActive(true);
                
                MobFactory.Instance.CreateMobInPickUp(OwnerType.My, (mob) =>
                {
                    pickUpItems[index].Unit = mob;
                });
            }
            
            this.NotifyObserver();
        }

        private void ClearPickUpItems()
        {
            pickUpItems.ForEach(item =>
            {
                if (item.Unit && item.IsChangedUnit == false)
                {
                    ObjectPoolManager.Instance.Return(item.Unit.gameObject);
                    item.Unit = null;
                }

                item.ResetPickUpAnim();
                item.gameObject.SetActive(false);
            });
        }

        public void StartOpenedAllPickUpUnits()
        {
            pickUpItems.ForEach(item =>
            {
                if (item.IsActive)
                {
                    item.StartOpenedAnim();
                }
            });
            
            this.NotifyObserver();
        }

        public void OpenAllPickUpUnits()
        {
            StartCoroutine(_OpenAllPickUpUnits());
        }

        IEnumerator _OpenAllPickUpUnits()
        {
            yield return null;
            
            foreach (var item in pickUpItems)
            {
                if (item.IsActive)
                {
                    continue;
                }
                
                item.StartPickUpAnim();
                yield return new WaitForSecondsRealtime(0.2f);
            }

            this.NotifyObserver();
        }

        public void ClickPickUpOne()
        {
            if (IsEnoughMoneyUnitPickUpOne)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.UnitPickUpCost;
                
                PickUpUnits(1);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.UnitPickUpCost}"; });
            }
        }

        public void ClickPickUpTen()
        {
            if (IsEnoughMoneyUnitPickUpTen)
            {
                D.SelfPlayer.Gold -= D.SelfPlayer.UnitPickUpCost * 10;
                
                PickUpUnits(10);
                this.NotifyObserver();
            }
            else
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog => { dialog.Text = $"{Localization.GetLocalizedString("Not enoughMoney.. Cost : ")}{D.SelfPlayer.UnitPickUpCost}"; });
            }
        }
    }
}