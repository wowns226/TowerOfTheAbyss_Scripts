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

using UnityEngine;

namespace ProjectL
{
    public class RoundControllerBase<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        protected virtual void Start()
        {
            PlayRoundLogic.Instance.AddStatusEvent(RoundLogic.SetupRound, OnSetupRound);
            PlayRoundLogic.Instance.AddStatusEvent(RoundLogic.PlayRound, OnPlayRound);
            PlayRoundLogic.Instance.AddStatusEvent(RoundLogic.ResultRound, OnResultRound);
            PlayRoundLogic.Instance.AddStatusEvent(RoundLogic.EndRound, OnEndRound);
        }

        protected virtual void OnDestroy()
        {
            if (PlayRoundLogic.Instance == null)
                return;

            PlayRoundLogic.Instance.RemoveStatusEvent(RoundLogic.SetupRound, OnSetupRound);
            PlayRoundLogic.Instance.RemoveStatusEvent(RoundLogic.PlayRound, OnPlayRound);
            PlayRoundLogic.Instance.RemoveStatusEvent(RoundLogic.ResultRound, OnResultRound);
            PlayRoundLogic.Instance.RemoveStatusEvent(RoundLogic.EndRound, OnEndRound);
        }

        protected virtual void OnSetupRound() { }
        protected virtual void OnPlayRound() { }
        protected virtual void OnResultRound() { }
        protected virtual void OnEndRound() { }
    }
}
