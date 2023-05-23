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
using UnityEngine;
using UnityEngine.UI;

namespace ProjectL
{
    public class UnitWorldUI : MonoBehaviour
    { 
        [SerializeField]
        private Unit unit;

        [SerializeField]
        private Image hpImage;
        [SerializeField]
        private Image castingImage;

        private Vector3 uiRotation;

        // Start is called before the first frame update
        private void Start()
        {
            unit.onRotated.Add(SetDirection);
            unit.onChangedHp += OnChangeHp;
            unit.onStartCasting += OnStartCasting;
            uiRotation = Camera.main.transform.forward;
        }

        private void OnDestroy()
        {
            unit.onRotated.Remove(SetDirection);
            unit.onChangedHp -= OnChangeHp;
            unit.onStartCasting -= OnStartCasting;
        }

        private void OnEnable()
        {
            SetDirection(Quaternion.identity);
            castingImage.fillAmount = 0;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void SetDirection(Quaternion quaternion)
        {
            transform.LookAt(transform.position + uiRotation);
        }

        private void OnChangeHp(float hp)
        {
            float remainHpRate = hp / unit.MaxHp;
            hpImage.fillAmount = remainHpRate;
        }

        private void OnStartCasting(float endTime)
        {
            Debug.Log($"UnitWorldUI.OnStartCasting, Unit : {unit.name}, endTime : {endTime}");
            StartCoroutine(OnStartCastingAsync(endTime));
        }

        private IEnumerator OnStartCastingAsync(float endTime)
        {
            float startTime = Time.time;
            float currentTime = startTime;
            float castingTime = endTime - startTime;

            while (currentTime < endTime)
            {
                float elapsedTimeRate = (currentTime - startTime) / castingTime;
                castingImage.fillAmount = elapsedTimeRate;
                yield return null;

                currentTime = Time.time;
            }

            castingImage.fillAmount = 0;
        }

    }

}
