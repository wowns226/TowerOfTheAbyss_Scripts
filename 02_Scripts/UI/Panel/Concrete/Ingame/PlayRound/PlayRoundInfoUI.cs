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
using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class PlayRoundInfoUI : InGameUIBase
    {
        [Serializable]
        public class GameSpeed
        {
            public float speed;
            public Sprite sprite;
        }

        [SerializeField]
        private Animator menuAnimator;
        private bool isMenuOpen;

        [SerializeField]
        private Image hpImage;
        
        [SerializeField]
        private GameSpeed[] inGameSpeeds;

        [SerializeField]
        private List<UnitItemInfo> unitInfos = new List<UnitItemInfo>();
        [SerializeField]
        private List<SkillInfo> skillInfos = new List<SkillInfo>();

        [DataObservable]
        private string RoundInfo => $"{D.SelfRound?.RoundType} - {D.SelfRound?.Stage}";

        private int gameSpeedIndex;
        [DataObservable]
        private Sprite GameSpeedImage => inGameSpeeds[gameSpeedIndex].sprite;

        [DataObservable]
        private bool IsShowUnitRange => VisualManager.Instance.ShowUnitRange;

        [DataObservable]
        private string Gold => D.SelfPlayer.Gold.ToString();

        [DataObservable]
        private bool IsDroneExist => D.SelfPlayer.drones.Count > 0 && D.SelfPlayer.isDroneModeChangeDevelopmentCompleted;
        [DataObservable]
        private bool IsRepairMode => D.SelfPlayer.RepairDroneState == RepairDroneState.Repair;
        [DataObservable]
        private bool IsBuffMode => D.SelfPlayer.RepairDroneState == RepairDroneState.Buff;
        [DataObservable]
        private bool IsDeBuffMode => D.SelfPlayer.RepairDroneState == RepairDroneState.DeBuff;
        [DataObservable]
        private string DroneModeName => $"{D.SelfPlayer.RepairDroneState}"; 

        protected override void Active()
        {
            base.Active();

            D.SelfPlayer.Castle.onChangedHp += OnChangedHp;
            D.SelfPlayer.onGoldChanged += OnGoldChanged;
            
            D.SelfPlayer.onAddUnit += OnAddUnit;
            D.SelfPlayer.onClearedUnit += OnClearedUnit;
            
            D.SelfPlayer.onAddDrone += OnAddDrone;
            D.SelfPlayer.onRemoveDrone += OnRemoveDrone;
            D.SelfPlayer.onClearedDrone += OnClearedDrone;
            
            D.SelfPlayer.onDroneModeDevelop.Add(OnDevelopDroneMode);

            InitTabUnits();
            this.NotifyObserver();
        }

        protected override void InActive()
        {
            base.InActive();

            D.SelfPlayer.Castle.onChangedHp -= OnChangedHp;
            D.SelfPlayer.onGoldChanged -= OnGoldChanged;
            
            D.SelfPlayer.onAddUnit -= OnAddUnit;
            D.SelfPlayer.onClearedUnit -= OnClearedUnit;
            
            D.SelfPlayer.onAddDrone -= OnAddDrone;
            D.SelfPlayer.onRemoveDrone -= OnRemoveDrone;
            D.SelfPlayer.onClearedDrone -= OnClearedDrone;
            
            D.SelfPlayer.onDroneModeDevelop.Remove(OnDevelopDroneMode);
        }

        private void OnChangedHp(float value)
        {
            if (D.SelfPlayer == null || D.SelfPlayer.Castle == null)
            {
                return;
            }
            
            hpImage.fillAmount = value / D.SelfPlayer.Castle.MaxHp;
        }

        private void OnGoldChanged(float obj) => this.NotifyObserver("Gold");

        private void OnAddUnit(Unit unit)
        {
            if(unit.UnitType != UnitType.Mob)
            {
                return;
            }

            InitTabUnits();
            this.NotifyObserver();
        }

        private void OnClearedUnit()
        {
            InitTabUnits();
            this.NotifyObserver();
        }
        
        private void InitTabUnits()
        {
            for (int i = 0; i < skillInfos.Count; i++)
            {
                var mobs = D.SelfPlayer.Mobs;
                
                skillInfos[i].Unit = mobs?.Count > i ? mobs[i] : null;
            }
            
            for (int i = 0; i < unitInfos.Count; i++)
            {
                var mobs = D.SelfPlayer.Mobs;
                
                unitInfos[i].Unit = mobs?.Count > i ? mobs[i] : null;
            }
        }

        private void OnDevelopDroneMode(bool isDevelop) => this.NotifyObserver();
        private void OnAddDrone(Drone drone) => this.NotifyObserver();
        private void OnRemoveDrone(Drone drone) => this.NotifyObserver();
        private void OnClearedDrone() => this.NotifyObserver();
        
        public void OnClickMenuButton()
        {
            Debug.Log($"PlayRoundInfoUI.OnClickMenuButton() : {isMenuOpen}");

            isMenuOpen = !isMenuOpen;
            menuAnimator.SetBool("isOpen", isMenuOpen);
        }

        public void OnClickProfileButton()
        {
            Debug.Log("PlayRoundInfoUI.OnClickProfileButton()");
            DialogManager.Instance.OpenDialog<DlgProfile>("DlgProfile");
        }

        public void OnClickSettingButton()
        {
            Debug.Log("PlayRoundInfoUI.OnClickSettingButton()");
            DialogManager.Instance.OpenDialog<DlgSettings>("DlgSettings");
        }

        public void OnClickExitButton()
        {
            Debug.Log("PlayRoundInfoUI.OnClickExitButton()");
            DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
            {
                dialog.Title = Localization.GetLocalizedString("Common/Exit/ToLobby");
                dialog.Content = Localization.GetLocalizedString("Common/Exit/Content");
                dialog.AddOKEvent(() => PlayRoundLogic.Instance.EnterStatus(RoundLogic.EndRound));
            });
        }

        public void OnClickedInGameSpeedBtn()
        {
            gameSpeedIndex++;
            gameSpeedIndex %= inGameSpeeds.Length;

            TimeManager.Instance.ChangeTimeScaleBase(inGameSpeeds[gameSpeedIndex].speed);

            this.NotifyObserver();
            Debug.Log($"Game Speed : x{inGameSpeeds[gameSpeedIndex].speed}");
        }

        public void OnClickedInGamePauseBtn()
        {
            Debug.Log("Game Pause !");

            DialogManager.Instance.OpenDialog<DlgPause>("DlgPause");
        }

        public void OnClickShowUnitRangeBtn()
        {
            Debug.Log("PlayRoundInfoUI.OnClickShowUnitRangeBtn()");

            VisualManager.Instance.ShowUnitRange = true;
            this.NotifyObserver();
        }

        public void OnClickHideUnitRangeBtn()
        {
            Debug.Log("PlayRoundInfoUI.OnClickHideUnitRangeBtn()");

            VisualManager.Instance.ShowUnitRange = false;
            this.NotifyObserver();
        }

        public void OnClickChangeDroneState()
        {
            Debug.Log("PlayRoundInfoUI.OnClickChangeDroneState()");
            
            D.SelfPlayer.ChangeDroneState();
            
            this.NotifyObserver();
        }
    }
}
