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
    public abstract class UnitState<T> where T : MonoBehaviour, IUnitState
    {
        public UnitState() { }
        public UnitState(UnitState<T> nextState) => this.nextState = nextState;

        private UnitState<T> nextState;

        public void Execute(T unit)
        {
            if (CheckState(unit))
            {
                Rotate(unit);
                ExecuteState(unit);
                return;
            }

            nextState?.Execute(unit);
        }

        protected abstract bool CheckState(T unit);
        protected abstract void ExecuteState(T unit);

        protected virtual void Rotate(T unit) { }
    }
}
