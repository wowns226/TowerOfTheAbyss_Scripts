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

namespace ProjectL
{
    public class LinearCastingSkillRange : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;

        public void ShowSkillRange(Vector3 startPos, Vector3 endPos, float castingTime)
        {
            lineRenderer.SetPositions(new Vector3[] { startPos, endPos });
            StartCoroutine(FadeOutEffect(castingTime));
        }

        private IEnumerator FadeOutEffect(float castingTime)
        {
            float endTime = Time.time + castingTime;
            
            lineRenderer.enabled = true;
            SetAlpha(1);

            while (Time.time <= endTime)
            {
                float reaminTime = endTime - Time.time;
                float remainRate = (castingTime - reaminTime) / castingTime;

                SetAlpha(remainRate);

                yield return null;
            }

            lineRenderer.enabled = false;
            ObjectPoolManager.Instance.Return(gameObject);
        }

        private void SetAlpha(float alpha)
        {
            Color color = Color.red;
            color.a = alpha;
            lineRenderer.material.color = color;
        }
    }
}