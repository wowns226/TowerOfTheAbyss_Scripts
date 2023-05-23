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

using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class DlgSettings : DialogBase
    {
        [Header("GamePlay")]
        [Space(10)]
        [SerializeField]
        private MainPanelManager mainPanelManager;
        [SerializeField]
        private HorizontalSelector languageSelector;
        [SerializeField]
        private HorizontalSelector resolutionSelector;
        [SerializeField]
        private SwitchManager fullScreenModeSwitch;

        [Header("Control")]
        [Space(10)]
        [SerializeField]
        private InputKeyCheckAndAction inputKeyCheckAndAction;
        [SerializeField]
        private Slider keyboardSensitivitySlider;
        [SerializeField]
        private Slider mouseSensitivitySlider;
        [SerializeField]
        private Slider zoomSensitivitySlider;

        [Header("Audio")]
        [Space(10)]
        [SerializeField]
        private Slider masterVolumeSlider;
        [SerializeField]
        private Slider musicVolumeSlider;
        [SerializeField]
        private Slider sfxVolumeSlider;
        [SerializeField]
        private SwitchManager masterVolumeSwitch;

        [Header("Visual")]
        [Space(10)]
        [SerializeField]
        private HorizontalSelector qualitySelector;
        [SerializeField]
        private SwitchManager skillEffectSwitch;

        [DataObservable]
        private bool IsOptionChanged => SettingManager.Instance.IsOptionChanged;
        
        #region DataObservable - Control

        [DataObservable]
        private string MoveForwardKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.MoveForward]}";
        [DataObservable]
        private string MoveBackwardKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.MoveBackward]}";
        [DataObservable]
        private string MoveLeftKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.MoveLeft]}";
        [DataObservable]
        private string MoveRightKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.MoveRight]}";
        [DataObservable]
        private string MoveSpeedUpKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.MoveSpeedUp]}";
        [DataObservable]
        private string MoveResetKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.MoveReset]}";
        [DataObservable]
        private string ProfileKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Profile]}";
        [DataObservable]
        private string TechnologyKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Technology]}";
        [DataObservable]
        private string CustomUnitKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.CustomUnit]}";
        [DataObservable]
        private string RelicKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Relic]}";
        [DataObservable]
        private string UpgradeKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Upgrade]}";
        [DataObservable]
        private string BuildingKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Building]}";
        [DataObservable]
        private string SystemKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.System]}";
        [DataObservable]
        private string ExitKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Exit]}";
        [DataObservable]
        private string Focus1Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Focus1]}";
        [DataObservable]
        private string Focus2Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Focus2]}";
        [DataObservable]
        private string Focus3Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Focus3]}";
        [DataObservable]
        private string Focus4Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Focus4]}";
        [DataObservable]
        private string Focus5Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Focus5]}";
        [DataObservable]
        private string Skill1Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Skill1]}";
        [DataObservable]
        private string Skill2Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Skill2]}";
        [DataObservable]
        private string Skill3Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Skill3]}";
        [DataObservable]
        private string Skill4Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Skill4]}";
        [DataObservable]
        private string Skill5Key => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.Skill5]}";
        [DataObservable]
        private string ScreenShotKey => $"{SettingManager.Instance.KeyBindingOption.NewData.bindingDatas[InputType.ScreenShot]}";
        
        #endregion
        
        public override void OpenDialog()
        {
            base.OpenDialog();
            mainPanelManager.OpenFirstTab();

            TimeManager.Instance.PauseTimeScale();

            RefreshUI();
            this.NotifyObserver();
        }

        public void ResetAllSettingValue()
        {
            ResetSettingValue();
            
            ResetLanguage();
            ResetResolution();
            ResetFullScreenMode();
            
            ResetMasterVolume();
            ResetMusicVolume();
            ResetSFXVolume();
            ResetMasterVolumeMuteSwitch();
            
            ResetMouseSensitivity();
            ResetKeyBoardSensitivity();
            ResetZoomSensitivity();
            
            ResetQuality();
            ResetSkillEffect();
        }
        
        private void RefreshUI()
        {
            RefreshLanguage();
            RefreshResolution();
            RefreshFullScreenMode();
            RefreshKeyBoardSensitivity();
            RefreshMouseSensitivity();
            RefreshZoomSensitivity();
            RefreshMasterVolume();
            RefreshMusicVolume();
            RefreshSFXVolume();
            RefreshMasterVolumeMuteSwitch();
            RefreshQuality();
            RefreshSkillEffect();
        }

        public override void CloseDialog()
        {
            if (IsOptionChanged)
            {
                DialogManager.Instance.OpenDialog<DlgMessageBox>("DlgMessageBox", dialog =>
                {
                    dialog.Title = Localization.GetLocalizedString("DlgSettings/ExitAndSave/Title");
                    dialog.Content = Localization.GetLocalizedString("DlgSettings/ExitAndSave/Content");
                    dialog.AddOKEvent(base.CloseDialog);
                    dialog.AddOKEvent(() => TimeManager.Instance.ReturnTimeScale());
                    dialog.AddOKEvent(RevertSettingValue);
                    dialog.AddOKEvent(() => inputKeyCheckAndAction.gameObject.SetActive(false));
                });

                return;
            }

            base.CloseDialog();
            TimeManager.Instance.ReturnTimeScale();
            inputKeyCheckAndAction.gameObject.SetActive(false);
        }

        private void ResetSettingValue()
        {
            SettingManager.Instance.ResetAllOption();
            this.NotifyObserver("IsOptionChanged");
        }

        private void RevertSettingValue()
        {
            SettingManager.Instance.RevertAllOption();
            this.NotifyObserver("IsOptionChanged");
        }

        public void SaveSettingValue()
        {
            SettingManager.Instance.SaveAllOption();
            this.NotifyObserver("IsOptionChanged");
        }

        #region GamePlay
        
        public void SetLanguage(int index)
        {
            Debug.Log($"DlgSettings.SetLanguage(), int : {index}");

            var data = SettingManager.Instance.LanguageOption.NewDataClone;
            data.languageType = (LanguageType)index;
            SettingManager.Instance.LanguageOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetLanguage()
        {
            languageSelector.Index = 0;
            languageSelector.UpdateUI();
        }
        
        private void RefreshLanguage()
        {
            languageSelector.Index = (int)SettingManager.Instance.LanguageOption.NewData.languageType;
            languageSelector.UpdateUI();
        }
        
        public void SetResolution(int index)
        {
            Debug.Log($"DlgSettings.SetResolution(), int : {index}");
            
            var data = SettingManager.Instance.ResolutionOption.NewDataClone;
            data.resolutionType = (ResolutionType)index;
            SettingManager.Instance.ResolutionOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetResolution()
        {
            resolutionSelector.Index = 0;
            resolutionSelector.UpdateUI();
        }
        
        private void RefreshResolution()
        {
            resolutionSelector.Index = (int)SettingManager.Instance.ResolutionOption.NewData.resolutionType;
            resolutionSelector.UpdateUI();
        }
        
        public void SetFullScreenReverse()
        {
            fullScreenModeSwitch.isOn = !fullScreenModeSwitch.isOn;
        }
        
        public void SetFullScreen(bool isOn)
        {
            Debug.Log($"DlgSettings.SetFullScreen(), bool : {isOn}");

            var data = SettingManager.Instance.ResolutionOption.NewDataClone;
            data.isFullScreen = isOn;
            SettingManager.Instance.ResolutionOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetFullScreenMode()
        {
            fullScreenModeSwitch.isOn = true;
        }
        
        private void RefreshFullScreenMode()
        {
            fullScreenModeSwitch.isOn = SettingManager.Instance.ResolutionOption.NewData.isFullScreen;
        }

        #endregion

        #region Control

        public void ChangeMouseSensitivity(float value)
        {
            var data = SettingManager.Instance.ControlOption.NewDataClone;
            data.mouseSensitivity = value;
            SettingManager.Instance.ControlOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetMouseSensitivity()
        {
            mouseSensitivitySlider.value = 50f;
        }
        
        private void RefreshMouseSensitivity()
        {
            mouseSensitivitySlider.value = SettingManager.Instance.ControlOption.NewData.mouseSensitivity;
        }
        
        public void ChangeKeyboardSensitivity(float value)
        {
            var data = SettingManager.Instance.ControlOption.NewDataClone;
            data.keyboardSensitivity = value;
            SettingManager.Instance.ControlOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetKeyBoardSensitivity()
        {
            keyboardSensitivitySlider.value = 50f;
        }
        
        private void RefreshKeyBoardSensitivity()
        {
            keyboardSensitivitySlider.value = SettingManager.Instance.ControlOption.NewData.keyboardSensitivity;
        }
        
        public void ChangeZoomSensitivity(float value)
        {
            var data = SettingManager.Instance.ControlOption.NewDataClone;
            data.zoomSensitivity = value;
            SettingManager.Instance.ControlOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetZoomSensitivity()
        {
            zoomSensitivitySlider.value = 50f;
        }
        
        private void RefreshZoomSensitivity()
        {
            zoomSensitivitySlider.value = SettingManager.Instance.ControlOption.NewData.zoomSensitivity;
        }
        
        public void OnClickMoveForwardChanged() => ChangeKeyBinding(InputType.MoveForward);
        public void OnClickMoveBackwardChanged() => ChangeKeyBinding(InputType.MoveBackward);
        public void OnClickMoveLeftChanged() => ChangeKeyBinding(InputType.MoveLeft);
        public void OnClickMoveRightChanged() => ChangeKeyBinding(InputType.MoveRight);
        public void OnClickMoveSpeedUpChanged() => ChangeKeyBinding(InputType.MoveSpeedUp);
        public void OnClickMoveResetChanged() => ChangeKeyBinding(InputType.MoveReset);
        public void OnClickProfileChanged() => ChangeKeyBinding(InputType.Profile);
        public void OnClickTechnologyChanged() => ChangeKeyBinding(InputType.Technology);
        public void OnClickCustomUnitChanged() => ChangeKeyBinding(InputType.CustomUnit);
        public void OnClickRelicChanged() => ChangeKeyBinding(InputType.Relic);
        public void OnClickUpgradeChanged() => ChangeKeyBinding(InputType.Upgrade);
        public void OnClickBuildingChanged() => ChangeKeyBinding(InputType.Building);
        public void OnClickSystemChanged() => ChangeKeyBinding(InputType.System);
        public void OnClickExitChanged() => ChangeKeyBinding(InputType.Exit);
        public void OnClickFocus1Changed() => ChangeKeyBinding(InputType.Focus1);
        public void OnClickFocus2Changed() => ChangeKeyBinding(InputType.Focus2);
        public void OnClickFocus3Changed() => ChangeKeyBinding(InputType.Focus3);
        public void OnClickFocus4Changed() => ChangeKeyBinding(InputType.Focus4);
        public void OnClickFocus5Changed() => ChangeKeyBinding(InputType.Focus5);
        public void OnClickSkill1Changed() => ChangeKeyBinding(InputType.Skill1);
        public void OnClickSkill2Changed() => ChangeKeyBinding(InputType.Skill2);
        public void OnClickSkill3Changed() => ChangeKeyBinding(InputType.Skill3);
        public void OnClickSkill4Changed() => ChangeKeyBinding(InputType.Skill4);
        public void OnClickSkill5Changed() => ChangeKeyBinding(InputType.Skill5);
        public void OnClickScreenShotChanged() => ChangeKeyBinding(InputType.ScreenShot);
        
        private void ChangeKeyBinding(InputType inputType)
        {
            inputKeyCheckAndAction.RegisterAction((keyCode) =>
            {
                SettingManager.Instance.KeyBindingOption.Bind(inputType, keyCode);
                this.NotifyObserver();
            });
        }

        #endregion

        #region Audio

        public void ChangeMasterVolume(float value)
        {
            var data = SettingManager.Instance.VolumeOption.NewDataClone;
            data.masterVolume = value;
            SettingManager.Instance.VolumeOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetMasterVolume()
        {
            masterVolumeSlider.value = 50f;
        }
        
        private void RefreshMasterVolume()
        {
            masterVolumeSlider.value = SettingManager.Instance.VolumeOption.NewData.masterVolume;
        }

        public void ChangeMusicVolume(float value)
        {
            var data = SettingManager.Instance.VolumeOption.NewDataClone;
            data.musicVolume = value;
            SettingManager.Instance.VolumeOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetMusicVolume()
        {
            musicVolumeSlider.value = 50f;
        }
        
        private void RefreshMusicVolume()
        {
            musicVolumeSlider.value = SettingManager.Instance.VolumeOption.NewData.musicVolume;
        }

        public void ChangeSFXVolume(float value)
        {
            var data = SettingManager.Instance.VolumeOption.NewDataClone;
            data.sfxVolume = value;
            SettingManager.Instance.VolumeOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }
        
        private void ResetSFXVolume()
        {
            sfxVolumeSlider.value = 50f;
        }
        
        private void RefreshSFXVolume()
        {
            sfxVolumeSlider.value = SettingManager.Instance.VolumeOption.NewData.sfxVolume;
        }

        public void SetMasterVolumeMuteReverse()
        {
            masterVolumeSwitch.isOn = !masterVolumeSwitch.isOn;
        }
        
        public void SetMasterVolumeMute(bool isOn)
        {
            Debug.Log($"DlgSettings.SetMasterSound(), bool : {isOn}");

            var data = SettingManager.Instance.VolumeOption.NewDataClone;
            data.muteMasterVolume = isOn;
            SettingManager.Instance.VolumeOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetMasterVolumeMuteSwitch()
        {
            masterVolumeSwitch.isOn = false;
        }
        
        private void RefreshMasterVolumeMuteSwitch()
        {
            masterVolumeSwitch.isOn = SettingManager.Instance.VolumeOption.NewData.muteMasterVolume;
        }
        
        #endregion

        #region Visual

        public void SetQuality(int index)
        {
            Debug.Log($"DlgSettings.SetQuality(), int : {index}");
            
            var data = SettingManager.Instance.QualityOption.NewDataClone;
            data.qualityType = (QualityType)index;
            SettingManager.Instance.QualityOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetQuality()
        {
            qualitySelector.Index = 3;
            qualitySelector.UpdateUI();
        }

        private void RefreshQuality()
        {
            qualitySelector.Index = (int)SettingManager.Instance.QualityOption.CurrentData.qualityType;
            qualitySelector.UpdateUI();
        }
        
        public void SetSkillEffectReverse()
        {
            skillEffectSwitch.isOn = !skillEffectSwitch.isOn;
        }
        
        public void SetSkillEffect(bool isOn)
        {
            Debug.Log($"DlgSettings.SetSkillEffect(), bool : {isOn}");

            var data = SettingManager.Instance.QualityOption.NewDataClone;
            data.isEnableSkillEffect = isOn;
            SettingManager.Instance.QualityOption.NewData = data;
            
            this.NotifyObserver("IsOptionChanged");
        }

        private void ResetSkillEffect()
        {
            skillEffectSwitch.isOn = true;
        }
        
        private void RefreshSkillEffect()
        {
            skillEffectSwitch.isOn = SettingManager.Instance.QualityOption.NewData.isEnableSkillEffect;
        }

        #endregion
        
        public void OnClickClose()
        {
            CloseDialog();
        }
    }
}
