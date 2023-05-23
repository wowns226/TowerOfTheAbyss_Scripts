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
    public class TimeManager : MonoSingleton<TimeManager>
    {
        private float timeScale = 1f;
        
        private bool isPause;
        public bool IsPause => isPause;
        
        public float TimeScale
        {
            get => timeScale;
            private set
            {
                Debug.Log($"TimeManager.TimeScale.Set(), TimeScale : {value}, prevTimeScale : {timeScale}");
                timeScale = value;
                Time.timeScale = timeScale;
            }
        }

        public void ChangeTimeScaleBase(float baseTimeScale)
        {
            Debug.Log($"TimeManager.ChangeTimeScaleBase(), TimeScale : {baseTimeScale}");
            isPause = false;
            TimeScale = baseTimeScale;
        }

        public void ChangeTimeScale(float changeScale)
        {
            Debug.Log($"TimeManager.ChangeTimeScale(), TimeScale : {TimeScale * changeScale}");
            isPause = false;
            Time.timeScale = TimeScale * changeScale;
        }

        public void PauseTimeScale()
        {
            Debug.Log("TimeManager.PauseTimeScale()");
            isPause = true;
            Time.timeScale = 0;
        }

        public void ReturnTimeScale()
        {
            Debug.Log("TimeManager.ResumeTimeScale()");
            isPause = false;
            Time.timeScale = TimeScale;
        }
    }
}
