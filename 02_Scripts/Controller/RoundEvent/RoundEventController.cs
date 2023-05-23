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
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectL
{
    public enum PointBuffType
    {
        DamageUp = 0,
        DefenseUp = 1,
    }

    public enum PointDeBuffType
    {
        DamageDown = 0,
        DefenseDown = 1,
    }

    public class RoundEventController : RoundControllerBase<RoundEventController>
    {
        private const float POINTEVENT_PER_TIME = 30.0f;

        private Coroutine upgradeEnemyCoroutine;
        private Coroutine bigWaveCoroutine;
        private Coroutine buffPointCoroutine;
        private Coroutine debuffPointCoroutine;
        
        private int eventKey;
        public int EventKey => eventKey;

        private Array buffEventTypes;
        private Array BuffEventTypes => buffEventTypes;

        private Array deBuffEventTypes;
        private Array DeBuffEventTypes => deBuffEventTypes;

        private void Awake()
        {
            eventKey = GetHashCode();
            buffEventTypes = Enum.GetValues(typeof(PointBuffType));
            deBuffEventTypes = Enum.GetValues(typeof(PointDeBuffType));
        }

        protected override void OnPlayRound()
        {
            base.OnPlayRound();

            if (upgradeEnemyCoroutine != null)
            {
                StopCoroutine(upgradeEnemyCoroutine);
                upgradeEnemyCoroutine = null;
            }
            upgradeEnemyCoroutine = StartCoroutine(UpgradeEnemy());
            
            if (bigWaveCoroutine != null)
            {
                StopCoroutine(bigWaveCoroutine);
                bigWaveCoroutine = null;
            }
            bigWaveCoroutine = StartCoroutine(BigWave());
            
            if (buffPointCoroutine != null)
            {
                StopCoroutine(buffPointCoroutine);
                buffPointCoroutine = null;
            }
            buffPointCoroutine = StartCoroutine(BuffPoint());
            
            if (debuffPointCoroutine != null)
            {
                StopCoroutine(debuffPointCoroutine);
                debuffPointCoroutine = null;
            }
            debuffPointCoroutine = StartCoroutine(DeBuffPoint());
        }

        protected override void OnResultRound()
        {
            base.OnResultRound();

            if (upgradeEnemyCoroutine != null)
            {
                StopCoroutine(upgradeEnemyCoroutine);
                upgradeEnemyCoroutine = null;
            }

            if (bigWaveCoroutine != null)
            {
                StopCoroutine(bigWaveCoroutine);
                bigWaveCoroutine = null;
            }
            
            if (buffPointCoroutine != null)
            {
                StopCoroutine(buffPointCoroutine);
                buffPointCoroutine = null;
            }
            
            if (debuffPointCoroutine != null)
            {
                StopCoroutine(debuffPointCoroutine);
                debuffPointCoroutine = null;
            }
            
            ResetEnemyUpgrade();
        }

        private IEnumerator UpgradeEnemy()
        {
            D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Hp, D.SelfRound.Stage);
            D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Damage, D.SelfRound.Stage);
            D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Heal, D.SelfRound.Stage);
            D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Defense, D.SelfRound.Stage);
            
            yield return new WaitForSeconds(120.0f);
            
            DialogManager.Instance.OpenDialog<DlgWarning>("DlgWarning", dlg =>
            {
                dlg.Text = Localization.GetLocalizedString("DlgWarning/EnemyUpgrade/Title");
                dlg.AutoClose = 2.5f;
            });
            
            while (true)
            {
                D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Hp, 1);
                D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Damage, 1);
                D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Heal, 1);
                D.SelfEnemyPlayer.AllUpgrade(UnitType.Mob, StatType.Defense, 1);
                
                yield return new WaitForSeconds(10.0f);
            }
        }
        
        private IEnumerator BigWave()
        {
            while (true)
            {
                if (D.SelfEnemyPlayer.units.Count * 0.7 < D.SelfEnemyPlayer.units.Count(unit => unit.IsDeath))
                {
                    Debug.Log("RoundEventController.BigWave() is Start");
                    D.SelfBoard.points.ForEach(point => point.IsBlockSpawn = false);
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator BuffPoint()
        {
            Debug.Log("RoundEventController.BuffPoint() is Start");

            while (true)
            {
                int pointCount = Random.Range(0, 3);
                var points = D.SelfBoard.GetRandomPoint(pointCount);

                points.ForEach(point =>
                {
                    var eventType = (PointBuffType)Random.Range(0, BuffEventTypes.Length);
                    var activeFunc = GetActivePointBuffEvent(eventType);
                    var inActiveFunc = GetInActivePointBuffEvent(eventType);

                    point.SetState(PointState.Buff);
                    point.AddActiveBuff(EventKey, activeFunc, inActiveFunc);
                });

                yield return new WaitForSeconds(POINTEVENT_PER_TIME);

                points.ForEach(point =>
                {
                    point.UnSetState(PointState.Buff);
                    point.RemoveActiveBuff(EventKey);
                });
            }
        }

        private IEnumerator DeBuffPoint()
        {
            Debug.Log("RoundEventController.DeBuffPoint() is Start");

            while (true)
            {
                int pointCount = Random.Range(0, 3);
                var points = D.SelfBoard.GetRandomPoint(pointCount);

                points.ForEach(point =>
                {
                    var eventType = (PointDeBuffType)Random.Range(0, DeBuffEventTypes.Length);
                    var activeFunc = GetActivePointDeBuffEvent(eventType);
                    var inActiveFunc = GetInActivePointDeBuffEvent(eventType);

                    point.SetState(PointState.DeBuff);
                    point.AddActiveBuff(EventKey, activeFunc, inActiveFunc);
                });

                yield return new WaitForSeconds(POINTEVENT_PER_TIME);

                points.ForEach(point =>
                {
                    point.UnSetState(PointState.DeBuff);
                    point.RemoveActiveBuff(EventKey);
                });
            }
        }

        private Action<Unit> GetActivePointBuffEvent(PointBuffType pointBuffType)
        {
            switch (pointBuffType)
            {
                case PointBuffType.DamageUp:
                    return unit =>
                    {
                        unit.UpgradeStatPercentage(StatType.Damage, 20);
                    };
                case PointBuffType.DefenseUp:
                    return unit =>
                    {
                        unit.UpgradeStatPercentage(StatType.Defense, 20);
                    };
            }
            return null;
        }

        private Action<Unit> GetInActivePointBuffEvent(PointBuffType pointBuffType)
        {
            switch (pointBuffType)
            {
                case PointBuffType.DamageUp:
                    return unit =>
                    {
                        unit.UpgradeStatPercentage(StatType.Damage, -20);
                    };
                case PointBuffType.DefenseUp:
                    return unit =>
                    {
                        unit.UpgradeStatPercentage(StatType.Defense, -20);
                    };
            }
            return null;
        }

        private Action<Unit> GetActivePointDeBuffEvent(PointDeBuffType pointBuffType)
        {
            switch (pointBuffType)
            {
                case PointDeBuffType.DamageDown:
                    return unit => unit.UpgradeStatPercentage(StatType.Damage, -20);
                case PointDeBuffType.DefenseDown:
                    return unit => unit.UpgradeStatPercentage(StatType.Defense, -20);
            }
            return null;
        }

        private Action<Unit> GetInActivePointDeBuffEvent(PointDeBuffType pointBuffType)
        {
            switch (pointBuffType)
            {
                case PointDeBuffType.DamageDown:
                    return unit => unit.UpgradeStatPercentage(StatType.Damage, 20);
                case PointDeBuffType.DefenseDown:
                    return unit => unit.UpgradeStatPercentage(StatType.Defense, 20);
            }
            return null;
        }

        private void ResetEnemyUpgrade()
        {
            D.SelfEnemyPlayer.ClearUpgradeLevel();
        }
    }
}
