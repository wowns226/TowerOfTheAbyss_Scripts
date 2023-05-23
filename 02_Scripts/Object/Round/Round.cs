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
    public class Round
    {
        public RoundType RoundType { get; }
        public IngameMapScene IngameMapScene { get; }
        public ChapterType Chapter => IngameMapScene.ToChapterType();

        private int stage;
        public int Stage => stage;
        private int successedStage;
        public int SuccessedStage => successedStage;

        public int TotalEnemyMobsCount => TotalCommonEnemyMobsCount + TotalNamedEnemyMobsCount + TotalBossEnemyMobsCount;
        public int TotalCommonEnemyMobsCount => Stage > 10 ? 20 : Stage * 2;
        public int TotalNamedEnemyMobsCount => Stage > 50 ? 10 : Stage / 5;
        public int TotalBossEnemyMobsCount => (Stage % 10 == 0) ? (Stage > 50 ? 5 : Stage / 10) : 0;

        public bool IsSetRound
        {
            get
            {
                int totalCount = 0;
                D.SelfPlayerGroup.enemyPlayers.ForEach(player => totalCount += player.Mobs.Count);
                return TotalEnemyMobsCount == totalCount;
            }
        }

        public Round(int stage, RoundType roundType, IngameMapScene scene)
        {
            Debug.Log($"Round.Create, Stage : {stage}");

            D.SelfRound = this;
            this.stage = stage;
            this.successedStage = stage - 1;
            this.RoundType = roundType;
            this.IngameMapScene = scene;
            Init();
        }

        private void Init()
        {
            SetMonsterMobs();
        }

        public void SuccessStage()
        {
            successedStage = Stage;
        }
        
        private void SetMonsterMobs()
        {
            Debug.Log($"Round.SetMonsterUnit(), Round : {stage}, TotalEnemyUnitCount : {TotalCommonEnemyMobsCount}, " +
                $"TotalNamedEnemyUnitCount : {TotalNamedEnemyMobsCount}, TotalBossEnemyUnitCount : {TotalBossEnemyMobsCount}");

            var units = MobManager.Instance.GetRandomMobNames((Chapter, OwnerType.Enemy, GradeType.Common, MobType.Common), TotalCommonEnemyMobsCount);
            units.AddRange(MobManager.Instance.GetRandomMobNames((Chapter, OwnerType.Enemy, GradeType.Common, MobType.Named), TotalNamedEnemyMobsCount));
            units.AddRange(MobManager.Instance.GetRandomMobNames((Chapter, OwnerType.Enemy, GradeType.Common, MobType.Boss), TotalBossEnemyMobsCount));

            units.Shuffle();
            
            //units.ForEach(unit => MobFactory.Instance.CreateMob(unit, D.SelfEnemyPlayer));
            units.ForEach(unit => MobFactory.Instance.CreateMob((GradeType.Common, MobType.Boss, "Crustaspikan"), D.SelfEnemyPlayer));
        }
    }
}
