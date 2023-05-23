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
    public class GoldController : RoundControllerBase<GoldController>, ISetting
    {
        [SettingValue]
        public int IncreaseGoldAmount { get; set; }

        [SettingValue]
        public float IncreaseGoldInterval { get; set; }

        public float ReducePercentageGoldCycleTime { get; set; } = 0f;

        private float WaitTime => IncreaseGoldInterval * (1 - ReducePercentageGoldCycleTime * 0.01f);
        
        private Coroutine goldCoroutine;

        protected override void Start()
        {
            base.Start();

            this.SetValue();
        }

        protected override void OnPlayRound()
        {
            base.OnPlayRound();

            if (goldCoroutine != null)
            {
                StopCoroutine(goldCoroutine);
                goldCoroutine = null;
            }

            goldCoroutine = StartCoroutine(IncreaseGold());
        }

        protected override void OnResultRound()
        {
            base.OnResultRound();

            if(goldCoroutine != null)
            {
                StopCoroutine(goldCoroutine);
                goldCoroutine = null;
            }
        }

        private IEnumerator IncreaseGold()
        {
            while (true)
            {
                yield return new WaitForSeconds(WaitTime);

                D.SelfPlayer.Gold += IncreaseGoldAmount;
            }
        }
    }
}
