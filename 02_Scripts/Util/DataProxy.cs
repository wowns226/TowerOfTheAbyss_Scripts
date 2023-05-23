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
    public static class D
    {
        public static Board SelfBoard { get; set; }
        public static User SelfUser { get; set; }
        public static Player SelfPlayer { get; set; }
        public static Player SelfEnemyPlayer { get; set; }
        public static PlayerGroup SelfPlayerGroup { get; set; }
        public static Round SelfRound { get; set; }

        public static Point SelfPoint { get; set; }
        public static Skill SelfSkill { get; set; }
        public static Unit SelfUnit { get; set; }
        public static Drone SelfDrone { get; set; }

        public static TechnologyBag SelfTechnologyBag => SelfPlayer == null ? null : SelfPlayer.TechnologyBag;
        public static Relic SelfRelic { get; set; }
        public static RelicBag SelfRelicBag => SelfPlayer == null ? null : SelfPlayer.RelicBag;
        public static RelicSetBag SelfRelicSetBag => SelfPlayer == null ? null : SelfPlayer.RelicSetBag;
    }
}
