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

using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public class PickUpItemInfo : DataContainer
    {
        private static readonly int isPickUp = Animator.StringToHash("Status");

        private bool isActive;
        public bool IsActive => isActive;

        [SerializeField]
        private Animator animator;
        [SerializeField]
        private DlgPickUp parentPanel;
        [SerializeField]
        private ChangePickUpUnitPanel changePickUpUnitPanel;
        
        private Unit unit;
        public Unit Unit
        {
            get => unit;
            set
            {
                unit = value;
                this.NotifyObserver();
            }
        }

        [DataObservable]
        public bool IsChangedUnit => Unit && D.SelfPlayer.Mobs.Any(mob => mob == Unit);
        
        [DataObservable]
        private string DisplayName => Unit?.DisplayName;        
        [DataObservable]
        private Sprite UnitImage => Unit?.UnitImage;
        
        [DataObservable]
        private bool IsNotLessThanCommon => Unit && Unit.GradeType >= GradeType.Common;
        [DataObservable]
        private bool IsNotLessThanRare => Unit && Unit.GradeType >= GradeType.Rare;
        [DataObservable]
        private bool IsNotLessThanUnique => Unit && Unit.GradeType >= GradeType.Unique;
        [DataObservable]
        private bool IsNotLessThanEpic => Unit && Unit.GradeType >= GradeType.Epic;
        [DataObservable]
        private bool IsNotLessThanSpecial => Unit && Unit.GradeType >= GradeType.Special;
        [DataObservable]
        private bool IsNotLessThanLegendary => Unit && Unit.GradeType >= GradeType.Legendary;
        [DataObservable]
        private bool IsNotLessThanAncient => Unit && Unit.GradeType >= GradeType.Ancient;
        
        [DataObservable]
        private bool IsLegendary => Unit && Unit.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => Unit && Unit.GradeType == GradeType.Ancient;
        
        [DataObservable]
        private bool IsMelee => Unit && Unit.AttackType == AttackType.Melee;
        [DataObservable]
        private bool IsHealer => Unit && Unit.AttackType == AttackType.Heal;
        [DataObservable]
        private bool IsRanged => Unit && Unit.AttackType == AttackType.Ranged;

        private void OnEnable()
        {
            if (D.SelfPlayer)
            {
                D.SelfPlayer.onAddUnit += this.NotifyObserver;
            }
            
            this.NotifyObserver();
        }

        private void OnDisable()
        {
            if (D.SelfPlayer)
            {
                D.SelfPlayer.onAddUnit -= this.NotifyObserver;
            }
        }

        private void NotifyObserver(Unit unit)
        {
            this.NotifyObserver();
        }
        
        public void StartPickUpAnim()
        {
            isActive = true;
            
            if (IsNotLessThanUnique)
            {
                animator.SetInteger(isPickUp, 3);
            }
            else
            {
                animator.SetInteger(isPickUp, 1);
            }
            
            parentPanel.NotifyObserver();
        }
        
        public void StartOpenedAnim()
        {
            animator.SetInteger(isPickUp, 2);
        }
        
        public void ResetPickUpAnim()
        {
            isActive = false;
            animator.SetInteger(isPickUp, 0);
        }

        public void ClickChangeUnit()
        {
            changePickUpUnitPanel.ChangedUnit = Unit;
            parentPanel.OpenPanel("Change");
            animator.SetInteger(isPickUp, 0);
        }
    }
}