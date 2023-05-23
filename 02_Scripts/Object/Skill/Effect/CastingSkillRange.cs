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
    public class CastingSkillRange : MonoBehaviour
    {
        public new ParticleSystem[] particleSystem;

        public void ShowSkillRange(float castingTime)
        {
            StartCoroutine(FadeOutEffect(castingTime));
        }

        private IEnumerator FadeOutEffect(float castingTime)
        {
            float endTime = Time.time + castingTime;

            SetAlpha(1);

            while (Time.time <= endTime)
            {
                float reaminTime = endTime - Time.time;
                float remainRate = (castingTime - reaminTime) / castingTime;

                SetAlpha(remainRate);

                yield return null;
            }

            ObjectPoolManager.Instance.Return(gameObject);
        }

        private void SetAlpha(float alpha)
        {
            foreach (var particle in particleSystem)
            {
                var main = particle.main;
                Color color = main.startColor.color;
                color.a = alpha;
                main.startColor = color;
            }
        }
    }
}
