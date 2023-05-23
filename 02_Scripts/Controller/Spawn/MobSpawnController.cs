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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectL
{
    public class MobSpawnController : RoundControllerBase<MobSpawnController>
    {
        private Coroutine spawnMineCoroutine;
        private Coroutine spawnEnemyCoroutine;

        protected override void OnPlayRound()
        {
            base.OnPlayRound();

            SetSpawnSetting();
            StartSpawnEnemy();
            StartSpawnMine();
        }

        protected override void OnEndRound()
        {
            base.OnEndRound();

            if (spawnEnemyCoroutine != null)
            {
                StopCoroutine(spawnEnemyCoroutine);
                spawnEnemyCoroutine = null;
            }

            if (spawnMineCoroutine != null)
            {
                StopCoroutine(spawnMineCoroutine);
                spawnMineCoroutine = null;
            }

            D.SelfBoard.points.ForEach(point => point.IsBlockSpawn = true);
        }

        private void SetSpawnSetting()
        {
            Debug.Log("UnitSpawnController.SetSpawnSetting");

            var spawns = D.SelfBoard.EnemySpawns.OrderBy(_ => Guid.NewGuid()).Take(D.SelfRound.Stage);

            foreach (var spawn in spawns)
                spawn.IsBlockSpawn = false;
        }


        #region SpawnMob

        private void StartSpawnEnemy()
        {
            Debug.Log("UnitSpawnController.StartSpawnEnemy");

            if (CheckSpawnAllMobs(D.SelfEnemyPlayer.Mobs))
                return;

            if (spawnEnemyCoroutine != null)
                return;

            SpawnEnemyMobs();
        }
        private bool CheckSpawnAllMobs(List<Unit> mobs) => mobs.Any(mob => mob.IsSpawn == false) == false;

        private void SpawnEnemyMobs() => spawnEnemyCoroutine = StartCoroutine(SpawnEnemyMob(D.SelfBoard.EnemySpawns));

        private IEnumerator SpawnEnemyMob(List<Point> spawnPoints)
        {
            while (true)
            {
                yield return null;

                var mob = GetSpawnMob(D.SelfEnemyPlayer.Mobs);

                if (mob == null)
                    continue;

                var spawnPoint = GetSpawnPoint(spawnPoints);

                if (spawnPoint == null)
                    continue;

                mob.Spawn(spawnPoint);

                var path = D.SelfBoard.GetRandomCastlePath(mob.BasePoint).ToQueue();
                mob.SetTargetPoints(path);
            }
        }

        private void StartSpawnMine()
        {
            Debug.Log("MobSpawnController.StartSpawnMine");

            if (spawnMineCoroutine != null)
                return;

            SpawnMineMobs();
        }

        private void SpawnMineMobs() => spawnMineCoroutine = StartCoroutine(SpawnMineMob(D.SelfBoard.AllySpawns));

        private IEnumerator SpawnMineMob(List<Point> spawnPoints)
        {
            while (true)
            {
                yield return null;

                if (TimeManager.Instance.IsPause)
                {
                    continue;
                }
                
                var mob = GetSpawnMob(D.SelfPlayer.Mobs);

                if (mob == null)
                    continue;

                var spawnPoint = GetSpawnPoint(spawnPoints);

                if (spawnPoint == null)
                    continue;

                mob.Spawn(spawnPoint);
            }
        }

        private Unit GetSpawnMob(List<Unit> mobs)
        {
            return mobs.Find(mob => mob.IsSpawn == false);
        }

        private Point GetSpawnPoint(List<Point> spawnPoints)
        {
            var availableSpawns = spawnPoints.FindAll(spawnPoint => spawnPoint.AvailableSpawn);

            if (availableSpawns.Count == 0)
                return null;

            int index = Random.Range(0, availableSpawns.Count);

            return availableSpawns[index];
        }

        #endregion
    }
}
