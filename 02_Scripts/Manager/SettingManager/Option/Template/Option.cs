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

namespace ProjectL
{
    public abstract class Option<T> : IOption where T : ISaveAndLoadToJson<T>, IEquatable<T>, ICloneable, new()
    {
        public Option(bool isApplyImmediately)
        {
            IsApplyImmediately = isApplyImmediately;
            Init();
        }

        private T data;
        private T tempData;
        public T OldData => data;
        public T NewData
        {
            get => tempData;
            set
            {
                IsChanged = value != null && !value.Equals(OldData);
                tempData = value;
                Apply();
            }
        }
        public T NewDataClone => (T)NewData.Clone();
        public T CurrentData => IsApplyImmediately ? NewData : OldData;

        public bool IsChanged { get; set; }
        public bool IsApplyImmediately { get; set; }

        public CustomAction<T> onValueChanged = new CustomAction<T>();
        
        private void Init()
        {
            Load();

            if (data == null)
            {
                Reset();
                Save();
                ResetTempData();
                return;
            }
            
            ResetTempData();
            _Apply();
        }

        private void ResetTempData() => tempData = data;
        
        public void Revert()
        {
            IsChanged = false;
            ResetTempData();
            Apply();
        }

        public void Reset()
        {
            NewData = new T();
            _Reset();
            Apply();
        }

        protected abstract void _Reset();

        public void Apply()
        {
            if (IsApplyImmediately)
            {
                _Apply();
            }
        }

        protected virtual void _Apply() => onValueChanged.Invoke(CurrentData);
        
        public virtual void Save()
        {
            IsChanged = false;
            data = tempData;
            data.Save();
            
            if (IsApplyImmediately == false)
            {
                _Apply();
            }
        }

        public virtual void Load()
        {
            data = data.Load();
        }
    }
}