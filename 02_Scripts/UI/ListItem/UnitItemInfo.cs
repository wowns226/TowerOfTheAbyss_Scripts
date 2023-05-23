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
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class UnitItemInfo : DataContainer
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
        private Image hpImage;
        [SerializeField]
        private Image remainTimeImage;
        [SerializeField]
        private TextMeshProUGUI remainTimeText;
        private Coroutine remainTimeCoroutine;

        [DataObservable]
        private bool IsExistUnit => Unit != null;
        
        [DataObservable]
        private Sprite Icon => Unit?.Icon;
        
        [DataObservable]
        private bool IsCommon => Unit && Unit.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => Unit && Unit.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => Unit && Unit.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => Unit && Unit.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => Unit && Unit.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => Unit && Unit.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => Unit && Unit.GradeType == GradeType.Ancient;

        private void OnDestroy()
        {
            RemoveEvent();
        }

        private void RegisterEvent()
        {
            if (Unit == null)
            {
                return;
            }
            
            if (hpImage != null)
            {
                Unit.onChangedHp += OnChangedHp;
            }

            if (remainTimeImage != null && remainTimeText != null)
            {
                Unit.onDeath.Add(OnDeath);
            }
        }

        private void RemoveEvent()
        {
            if (Unit == null)
            {
                return;
            }
            
            if (hpImage != null)
            {
                Unit.onChangedHp -= OnChangedHp;
            }
            
            if (remainTimeImage != null && remainTimeText != null)
            {
                Unit.onDeath.Remove(OnDeath);
                
                if (remainTimeCoroutine != null)
                {
                    StopCoroutine(remainTimeCoroutine);
                    remainTimeCoroutine = null;
                }
                
                remainTimeImage.fillAmount = 1;
                remainTimeText.text = "";
                remainTimeImage.gameObject.SetActive(false);
                remainTimeText.gameObject.SetActive(false);
                this.NotifyObserver();
            }
            
            
        }

        private void OnChangedHp(float value)
        {
            if (hpImage == null)
            {
                return;
            }
            
            hpImage.fillAmount = Unit.RemainHpPercentage * 0.01f;
        }
        
        private void OnDeath(Unit unit)
        {
            if (remainTimeImage == null)
            {
                return;
            }
            
            if (remainTimeCoroutine != null)
            {
                StopCoroutine(remainTimeCoroutine);
                remainTimeCoroutine = null;
            }
            remainTimeCoroutine = StartCoroutine(ShowRemainTime(unit));
        }

        IEnumerator ShowRemainTime(Unit unit)
        {
            remainTimeImage.gameObject.SetActive(true);
            remainTimeText.gameObject.SetActive(true);

            float deathTime = Time.time;
            float deathDelay = unit.DeathDelay;
            float reviveTime = deathTime + deathDelay;
            
            while (reviveTime > Time.time)
            {
                yield return null;
                float elaspedTime = Time.time - deathTime;
                float remainRate = 1 - elaspedTime / deathDelay;
                
                remainTimeImage.fillAmount = remainRate;
                remainTimeText.text = (deathDelay - elaspedTime).ToString("F0");
            }
            
            remainTimeImage.fillAmount = 1;
            remainTimeText.text = "";
            remainTimeImage.gameObject.SetActive(false);
            remainTimeText.gameObject.SetActive(false);
            this.NotifyObserver();
        }

    }
}