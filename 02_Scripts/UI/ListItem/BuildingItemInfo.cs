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
    public class BuildingItemInfo : DataContainer
    {
        private Building building;
        public Building Building
        {
            get => building;
            set
            {
                building = value;
                this.NotifyObserver();
            }
        }

        [DataObservable]
        private Sprite Icon => Building?.Icon;
        
        [DataObservable]
        private bool IsCommon => Building && Building.GradeType == GradeType.Common;
        [DataObservable]
        private bool IsRare => Building && Building.GradeType == GradeType.Rare;
        [DataObservable]
        private bool IsUnique => Building && Building.GradeType == GradeType.Unique;
        [DataObservable]
        private bool IsEpic => Building && Building.GradeType == GradeType.Epic;
        [DataObservable]
        private bool IsSpecial => Building && Building.GradeType == GradeType.Special;
        [DataObservable]
        private bool IsLegendary => Building && Building.GradeType == GradeType.Legendary;
        [DataObservable]
        private bool IsAncient => Building && Building.GradeType == GradeType.Ancient;
        
    }
}