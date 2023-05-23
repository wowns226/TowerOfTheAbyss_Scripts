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
    public class HpbuffRelic : Relic
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
        private float commonValue;
        [SettingValue] 
        private float rareValue;
        [SettingValue] 
        private float uniqueValue;
        [SettingValue] 
        private float epicValue;
        [SettingValue] 
        private float specialValue;
        [SettingValue] 
        private float legendaryValue;
        [SettingValue] 
        private float ancientValue;
        
        protected override void InitRelicSet()
        {
            AddRelicSet(Player.RelicSetBag.Get(nameof(DefenseRelicSet)));
        }

        protected override void _ActivateCommon()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, commonValue);
        }

        protected override void _ActivateRare()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, rareValue);
        }

        protected override void _ActivateUnique()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, uniqueValue);
        }

        protected override void _ActivateEpic()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, epicValue);
        }

        protected override void _ActivateSpecial()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, specialValue);
        }

        protected override void _ActivateLegendary()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, legendaryValue);
        }

        protected override void _ActivateAncient()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, ancientValue);
        }

        protected override void _InActivateCommon()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, commonValue * -1);
        }

        protected override void _InActivateRare()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, rareValue * -1);
        }

        protected override void _InActivateUnique()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, uniqueValue * -1);
        }

        protected override void _InActivateEpic()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, epicValue * -1);
        }

        protected override void _InActivateSpecial()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, specialValue * -1);
        }

        protected override void _InActivateLegendary()
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, legendaryValue * -1);
        }

        protected override void _InActivateAncient() 
        {
            Player.AllUpgradeStat(UnitType.Mob, StatType.Hp, ancientValue * -1);
        }
    }
}
