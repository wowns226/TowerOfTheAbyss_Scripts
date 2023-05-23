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
    public class GoldSellRelic : Relic
    {
        public override string CommonDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), commonSellGold * 10);
        public override string RareDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), rareSellGold * 10);
        public override string UniqueDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), uniqueSellGold * 10);
        public override string EpicDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), epicSellGold * 10);
        public override string SpecialDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), specialSellGold * 10);
        public override string LegendaryDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), legendarySellGold * 10);
        public override string AncientDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), ancientSellGold * 10);

        public override void Init(Player player)
        {
            base.Init(player);

            commonSellGold *= 10;
            rareSellGold *= 10;
            uniqueSellGold *= 10;
            epicSellGold *= 10;
            specialSellGold *= 10;
            legendarySellGold *= 10;
            ancientSellGold *= 10;
        }

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