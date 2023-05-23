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
using UnityEngine;

namespace ProjectL
{
    public class TimerInfo<T>
    {
        public TimerInfo(string name, Action<T> action, Action<T> endCallback, float endTime)
        {
            this.name = name;
            this.action = action;
            this.endCallback = endCallback;
            this.endTime = endTime;
            startTime = Time.time;
        }

        public string name;
        public Action<T> action;
        public Action<T> endCallback;
        public float startTime;
        public float endTime;
        public Coroutine coroutine;

        public float RemainTime => endTime - Time.time;
        public float RemainTimePercentage => (Time.time - startTime) / (endTime - startTime);
    }
}
