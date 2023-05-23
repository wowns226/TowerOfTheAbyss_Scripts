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
    public class RangeEffect : MonoBehaviour
    {
        public new ParticleSystem particleSystem;
        private Coroutine rangeEffectCoroutine;
        private int range;
        
        public void Toggle(Unit unit, bool isOn, int size = 0)
        {
            if (isOn)
            {
                var main = particleSystem.main;
                range = size;
                main.startSize = range * 2f;
                particleSystem.Play();

                if (rangeEffectCoroutine != null)
                {
                    StopCoroutine(rangeEffectCoroutine);
                    rangeEffectCoroutine = null;
                }
                
                rangeEffectCoroutine = StartCoroutine(CheckUnitRange(unit));

                unit.onRotated.Add(OnRotated);
            }
            else
            {
                particleSystem.Stop();
                range = 0;
                
                if (rangeEffectCoroutine != null)
                {
                    StopCoroutine(rangeEffectCoroutine);
                    rangeEffectCoroutine = null;
                }
                
                unit.onRotated.Remove(OnRotated);
            }
        }

        IEnumerator CheckUnitRange(Unit unit)
        {
            while (true)
            {
                yield return null;

                var unitRange = unit.Range;

                if (unitRange.Equals(range))
                {
                    continue;
                }
                
                particleSystem.Stop();
                var main = particleSystem.main;
                range = unitRange;
                main.startSize = range * 2f;
                particleSystem.Play();
            }
        }

        private void OnRotated(Quaternion quaternion)
        {
            this.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

    }
}
