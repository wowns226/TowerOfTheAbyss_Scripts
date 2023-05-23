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
using UnityEngine;

namespace ProjectL
{
    public class MobFactory: Singleton<MobFactory>
    {
        public void CreateMob((GradeType gradeType, MobType mobType, string type) key, Player player, Action<Mob> complete = null)
        {
            if (string.IsNullOrEmpty(key.type))
            {
                Debug.Log("MobFactory.CreateMob(), key is null or empty");
                return;
            }

            Debug.Log($"MobFactory.CreateMob(), key : {key.type}");

            ObjectPoolManager.Instance.New(key.type, activeImmediately: false, onComplete: obj =>
            {
                var mob = obj.GetComponent<Mob>();
                mob.GradeType = key.gradeType;
                mob.MobType = key.mobType;
                mob.Init(player);
                complete?.Invoke(mob);
                obj.SetActive(false);
            });
        }
            
        public void CreateMobInPickUp(OwnerType ownerType, Action<Mob> complete)
        {
            Debug.Log($"MobFactory.CreateMob(OwnerType ownerType, Action<Mob> complete)");

            var grade = GradeUtil.GetRandomGrade();
            
            var mobs = MobManager.Instance.GetRandomMobNames((RoundManager.Instance.Chapter, ownerType, grade, MobType.Hero), 1);

            mobs.ForEach(key =>
            {
                Debug.Log($"MobFactory.CreateMob(OwnerType ownerType, Action<Mob> complete), Mob : {key.type}");
                
                ObjectPoolManager.Instance.New(key.type, activeImmediately: false, onComplete: obj =>
                {
                    var mob = obj.GetComponent<Mob>();
                    mob.GradeType = key.gradeType;
                    mob.MobType = key.mobType;
                    mob.SetValue();
                    mob.SkillGroup.Init(mob);
                    complete?.Invoke(mob);
                    obj.SetActive(false);
                });
            });
        }
    }
}
