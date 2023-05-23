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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectL
{
    public abstract class BuffSkill : Skill
    {
        [SettingValue]
        protected float durationTime;
        private float EndTime => Time.time + durationTime;

        protected override void _UseSkillToAll(DamageInfo damageInfo)
        {
            List<Player> players = new List<Player>();

            switch (Unit.ownerType)
            {
                case OwnerType.My:
                    players = D.SelfPlayerGroup.allyPlayers;
                    break;
                case OwnerType.Enemy:
                    players = D.SelfPlayerGroup.enemyPlayers;
                    break;
            }
            
            Unit.AddBuffSkill(Name, StartMineBuffSkill, EndMineBuffSkill, EndTime);
            
            players.ForEach(targetPlayer =>
            {
                targetPlayer.SpawnUnits.ToList().ForEach(unit =>
                {
                    _StartHitEffect(unit.transform.position);
                    unit.AddBuffSkill(Name, StartBuffSkill, EndBuffSkill, EndTime);
                });
            });
        }

        protected override void _UseSkillToPoint(Point targetPoint, DamageInfo damageInfo)
        {
            Unit.AddBuffSkill(Name, StartMineBuffSkill, EndMineBuffSkill, EndTime);
            
            switch (Unit.ownerType)
            {
                case OwnerType.My:
                    targetPoint.GetAllyMobs().ToList().ForEach(hitUnit =>
                    {
                        _StartHitEffect(hitUnit.transform.position);
                        hitUnit.AddBuffSkill(Name, StartBuffSkill, EndBuffSkill, EndTime);
                    });
                    break;
                case OwnerType.Enemy:
                    targetPoint.GetEnemyMobs().ToList().ForEach(hitUnit =>
                    {
                        _StartHitEffect(hitUnit.transform.position);
                        hitUnit.AddBuffSkill(Name, StartBuffSkill, EndBuffSkill, EndTime);
                    });
                    break;
            }
        }

        protected virtual void StartMineBuffSkill(Unit target) { }
        protected virtual void EndMineBuffSkill(Unit target) { }
        protected virtual void StartBuffSkill(Unit target) { }
        protected virtual void EndBuffSkill(Unit target) { }
    }
}
