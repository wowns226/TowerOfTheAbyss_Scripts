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

namespace ProjectL
{
    public class DetectorRelic : Relic
    {
        // 은신 몬스터 제작 시 구현
        public override bool CompletedDev => false;

        protected override void InitRelicSet() { }
        protected override void _ActivateCommon() { }
        protected override void _ActivateRare() { }
        protected override void _ActivateUnique() { }
        protected override void _ActivateEpic() { }
        protected override void _ActivateSpecial() { }
        protected override void _ActivateLegendary() { }
        protected override void _ActivateAncient() { }
        protected override void _InActivateCommon() { }
        protected override void _InActivateRare() { }
        protected override void _InActivateUnique() { }
        protected override void _InActivateEpic() { }
        protected override void _InActivateSpecial() { }
        protected override void _InActivateLegendary() { }
        protected override void _InActivateAncient() { }
    }
}