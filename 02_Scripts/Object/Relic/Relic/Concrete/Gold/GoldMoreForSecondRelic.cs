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
    public class GoldMoreForSecondRelic : Relic
    {
        public override string CommonDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), commonValue);
        public override string RareDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), rareValue);
        public override string UniqueDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), uniqueValue);
        public override string EpicDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), epicValue);
        public override string SpecialDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), specialValue);
        public override string LegendaryDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), legendaryValue);
        public override string AncientDetailDescription =>
            string.Format(Localization.GetLocalizedString(description), ancientValue);
        
        [SettingValue] 
        private int commonValue;
        [SettingValue] 
        private int rareValue;
        [SettingValue] 
        private int uniqueValue;
        [SettingValue] 
        private int epicValue;
        [SettingValue] 
        private int specialValue;
        [SettingValue] 
        private int legendaryValue;
        [SettingValue] 
        private int ancientValue;
        
        protected override void InitRelicSet()
        {
            AddRelicSet(Player.RelicSetBag.Get(nameof(GoldRelicSet)));
        }

        protected override void _ActivateCommon()
        {
            GoldController.Instance.IncreaseGoldAmount += commonValue;
        }

        protected override void _ActivateRare()
        {
            GoldController.Instance.IncreaseGoldAmount += rareValue;
        }

        protected override void _ActivateUnique()
        {
            GoldController.Instance.IncreaseGoldAmount += uniqueValue;
        }

        protected override void _ActivateEpic()
        {
            GoldController.Instance.IncreaseGoldAmount += epicValue;
        }

        protected override void _ActivateSpecial()
        {
            GoldController.Instance.IncreaseGoldAmount += specialValue;
        }

        protected override void _ActivateLegendary()
        {
            GoldController.Instance.IncreaseGoldAmount += legendaryValue;
        }

        protected override void _ActivateAncient()
        {
            GoldController.Instance.IncreaseGoldAmount += ancientValue;
        }

        protected override void _InActivateCommon()
        {
            GoldController.Instance.IncreaseGoldAmount -= commonValue;
        }

        protected override void _InActivateRare()
        {
            GoldController.Instance.IncreaseGoldAmount -= rareValue;
        }

        protected override void _InActivateUnique()
        {
            GoldController.Instance.IncreaseGoldAmount -= uniqueValue;
        }

        protected override void _InActivateEpic()
        {
            GoldController.Instance.IncreaseGoldAmount -= epicValue;
        }

        protected override void _InActivateSpecial()
        {
            GoldController.Instance.IncreaseGoldAmount -= specialValue;
        }

        protected override void _InActivateLegendary()
        {
            GoldController.Instance.IncreaseGoldAmount -= legendaryValue;
        }

        protected override void _InActivateAncient() 
        {
            GoldController.Instance.IncreaseGoldAmount -= ancientValue;
        }
    }
}