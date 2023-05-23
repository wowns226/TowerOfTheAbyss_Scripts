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
using System.Collections.Generic;
using System.Linq;

namespace ProjectL
{
    public class CustomAction
    {
        private List<Action> actions = new List<Action>();

        public void Add(Action action)
        {
            if(actions.Any(item => item.Equals(action)))
            {
                return;
            }

            actions.Add(action);
        }

        public void Remove(Action action)
        {
            if (actions.Any(item => item.Equals(action)))
            {
                actions.Remove(action);
            }
        }

        public void Clear()
        {
            actions.Clear();
        }

        public void Invoke()
        {
            if (actions == null)
            {
                return;
            }

            actions.ForEach(action => action?.Invoke());
        }
    }

    public class CustomAction<T>
    {
        private List<Action<T>> actions = new List<Action<T>>();

        public void Add(Action<T> action)
        {
            if (actions.Any(item => item.Equals(action)))
            {
                return;
            }

            actions.Add(action);
        }

        public void Remove(Action<T> action)
        {
            if (actions.Any(item => item.Equals(action)))
            {
                actions.Remove(action);
            }
        }

        public void Clear()
        {
            actions.Clear();
        }

        public void Invoke(T value)
        {
            if (actions == null)
            {
                return;
            }

            actions.ForEach(action => action?.Invoke(value));
        }
    }
}
