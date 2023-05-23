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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class SkillInfo : DataContainer
    {
        private Unit unit;

        public Unit Unit
        {
            get => unit;
            set
            {
                RemoveEvent();
                unit = value;
                this.NotifyObserver();
                RegisterEvent();
            }
        }

        [SerializeField]
        private Image remainTimeImage;
        [SerializeField]
        private TextMeshProUGUI remainTimeText;
        private Coroutine remainTimeCoroutine;

        [DataObservable]
        private bool IsExistExtraSkill => Unit?.SkillGroup.ExtraSkill != null;
        [DataObservable]
        private Sprite ExtraSkillIcon => Unit?.SkillGroup.ExtraSkill?.Icon;
        [DataObservable]
        private bool IsReadyToUse => Unit?.IsUseExtraSkill ?? false;

        private void RegisterEvent()
        {
            if (Unit != null)
            {
                Unit.SkillGroup.ExtraSkill?.onUsedSkill.Add(OnUsedSkill);
            }
        }

        private void RemoveEvent()
        {
            if (Unit != null)
            {
                Unit.SkillGroup.ExtraSkill?.onUsedSkill.Remove(OnUsedSkill);
            }
        }

        private void OnUsedSkill(Skill skill)
        {
            if (remainTimeCoroutine != null)
            {
                StopCoroutine(remainTimeCoroutine);
                remainTimeCoroutine = null;
            }
            remainTimeCoroutine = StartCoroutine(ShowRemainTime(skill));
        }

        IEnumerator ShowRemainTime(Skill skill)
        {
            remainTimeImage.gameObject.SetActive(true);
            remainTimeText.gameObject.SetActive(true);
            
            while (skill.RemainCooldown > 0)
            {
                float totalTime = skill.CanUseTime - skill.UsedTime;
                yield return null;
                float remainRate = 1 - skill.ElaspedCooldown / totalTime;
                
                remainTimeImage.fillAmount = remainRate;
                remainTimeText.text = skill.RemainCooldown.ToString("F0");
            }
            
            remainTimeImage.fillAmount = 1;
            remainTimeText.text = "";
            remainTimeImage.gameObject.SetActive(false);
            remainTimeText.gameObject.SetActive(false);
            this.NotifyObserver();
        }

        public void ClickExtraSkill()
        {
            if (Unit == null)
            {
                return;
            }

            if (Unit.IsDeath)
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog =>
                {
                    dialog.Text = $"{Localization.GetLocalizedString("Ingame/ExtraSkill/NotUseByDeath")}";
                });
                return;
            }
            
            if (Unit.IsUseExtraSkill == false)
            {
                DialogManager.Instance.OpenDialog<DlgToolTip>("DlgToolTip", dialog =>
                {
                    dialog.Text = $"{Localization.GetLocalizedString("Ingame/ExtraSkill/NotUse")}";
                });
                return;
            }
            
            Unit.UseExtraSkill();
        }
    }
}
