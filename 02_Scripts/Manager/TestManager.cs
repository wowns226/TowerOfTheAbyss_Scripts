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
    public class TestManager : MonoSingleton<TestManager>
    {
        [SerializeField]
        private bool isDemoMode;
        public bool IsDemoMode => isDemoMode;
        
        public float timeScale = 1;

        [EditorButton("SetTimeScale")]
        public bool setTimeScale;
        public void SetTimeScale()
        {
            TimeManager.Instance.ChangeTimeScaleBase(timeScale);
        }

        private void Awake()
        {
#if UNITY_EDITOR
            SetTimeScale();
#endif
        }

        [EditorButton("PlayerUnitHpInfinity")]
        public bool playerUnitHpInfinity;
        public void PlayerUnitHpInfinity()
        {
            D.SelfPlayer.Mobs.ForEach(x => x.UpgradeStat(StatType.Hp, 1_000_000));
            D.SelfPlayer.Mobs.ForEach(x => x.FullHealHp());
        }

        [EditorButton("EnemyUnitHpInfinity")]
        public bool enemyUnitHpInfinity;
        public void EnemyUnitHpInfinity()
        {
            D.SelfEnemyPlayer.Mobs.ForEach(x => x.UpgradeStat(StatType.Hp, 1_000_000));
            D.SelfEnemyPlayer.Mobs.ForEach(x => x.FullHealHp());
        }

        [EditorButton("GiveMoney")]
        public bool giveMoney;
        public void GiveMoney()
        {
            D.SelfPlayer.Gold += 100_000;
        }
    }
}

