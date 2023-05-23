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
using System.Collections.Generic;
using System.Linq;

namespace ProjectL
{
    [Flags]
    public enum AttackSkillType
    {
        Damage = 1 << 0,
        Stun = 1 << 1,
        Knockback = 2 << 2,
    }

    public abstract class AttackSkill : Skill
    {
        [SettingValue]
        protected AttackSkillType attackSkillType;
        [SettingValue]
        protected float stunTime;

        protected override void _UseSkillToAll(DamageInfo damageInfo)
        {
            List<Player> players = new List<Player>();

            switch (Unit.ownerType)
            {
                case OwnerType.My:
                    players = D.SelfPlayerGroup.enemyPlayers;
                    break;
                case OwnerType.Enemy:
                    players = D.SelfPlayerGroup.allyPlayers;
                    break;
            }

            players.ForEach(targetPlayer =>
            {
                targetPlayer.SpawnUnits.ToList().ForEach(unit =>
                {
                    _StartHitEffectRandomPos(unit.transform.position, unit.Scale.y);
                    UseAttackSkill(damageInfo, unit);
                });
            });
        }

        protected override void _UseSkillToPoint(Point targetPoint, DamageInfo damageInfo)
        {
            switch (Unit.ownerType)
            {
                case OwnerType.My:
                    targetPoint.GetEnemyMobs().ToList().ForEach(hitUnit =>
                    {
                        _StartHitEffectRandomPos(hitUnit.transform.position, hitUnit.Scale.y);
                        UseAttackSkill(damageInfo, hitUnit);
                    });
                    break;
                case OwnerType.Enemy:
                    targetPoint.GetAllyMobs().ToList().ForEach(hitUnit =>
                    {
                        _StartHitEffectRandomPos(hitUnit.transform.position, hitUnit.Scale.y);
                        UseAttackSkill(damageInfo, hitUnit);
                    });
                    targetPoint.GetBuildings().ToList().ForEach(hitBuilding =>
                    {
                        _StartHitEffectRandomPos(hitBuilding.transform.position, hitBuilding.Scale.y);
                        hitBuilding.Hit(damageInfo, Unit.BasePoint);
                    });
                    break;
            }
        }

        protected void UseAttackSkill(DamageInfo damageInfo, Unit hitUnit)
        {
            if (attackSkillType.HasFlag(AttackSkillType.Damage))
            {
                var attackerPoint = Unit.BasePoint;
                hitUnit.Hit(damageInfo, attackerPoint, actualDamage => Unit.DrainHeal(actualDamage));
            }

            if (attackSkillType.HasFlag(AttackSkillType.Stun))
            {
                hitUnit.Stun(stunTime);
            }

            if (attackSkillType.HasFlag(AttackSkillType.Knockback))
            {
                hitUnit.Knockback();
            }
        }
    }
}
