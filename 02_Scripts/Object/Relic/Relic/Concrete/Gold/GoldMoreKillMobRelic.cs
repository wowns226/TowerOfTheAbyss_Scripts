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
    public class GoldMoreKillMobRelic : Relic
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
            AddRelicSet(Player.RelicSetBag.Get(nameof(GoldRelicSet)));
        }

        protected override void _ActivateCommon()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, commonValue);
        }
        
        protected override void _ActivateRare()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, rareValue);
        }

        protected override void _ActivateUnique()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, uniqueValue);
        }

        protected override void _ActivateEpic()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, epicValue);
        }

        protected override void _ActivateSpecial()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, specialValue);
        }

        protected override void _ActivateLegendary()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, legendaryValue);
        }

        protected override void _ActivateAncient()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, ancientValue);
        }

        protected override void _InActivateCommon()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, commonValue * -1);
        }

        protected override void _InActivateRare()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, rareValue * -1);
        }

        protected override void _InActivateUnique()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, uniqueValue * -1);
        }

        protected override void _InActivateEpic()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, epicValue * -1);
        }

        protected override void _InActivateSpecial()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, specialValue * -1);
        }

        protected override void _InActivateLegendary()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, legendaryValue * -1);
        }

        protected override void _InActivateAncient()
        {
            D.SelfEnemyPlayer.AllUpgradeStatPercentage(UnitType.Mob, StatType.DropGold, ancientValue * -1);
        }

    }
}