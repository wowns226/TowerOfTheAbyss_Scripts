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
    public abstract class Mob : Unit
    {
        private CustomAction<Mob> onAttackMob => player.onSharedAttackMob;
        private CustomAction<Mob> onHitMob => player.onSharedHitMob;
        private CustomAction<Mob> onHealMob => player.onSharedHealMob;
        private CustomAction<Mob> onCriticalMob => player.onSharedCriticalMob;
        private CustomAction<Mob> onDrainHpMob => player.onSharedDrainHpMob;
        private CustomAction<Mob> onPreDeathMob => player.onSharedPreDeathMob;
        private CustomAction<Mob> onPostDeathMob => player.onSharedPostDeathMob;

        protected override void Awake()
        {
            base.Awake();

            unitType = UnitType.Mob;
        }

        protected override void _Attack() 
        {
            onAttackMob.Invoke(this);
        }

        public override void Hit(DamageInfo damageInfo, Point attackerPoint, Action<float> action = null)
        {
            if (IsInvincible)
            {
                Debug.Log($"Unit.Hit return Because IsInvincible, Unit : {this.name}");
                return;
            }
            
            base.Hit(damageInfo, attackerPoint, action);
            if (damageInfo.IsCritical)
            {
                onCriticalMob.Invoke(this);
            }
        }

        protected override void HitTrueDamage(float damage, Point attackerPoint, Action<float> action = null)
        {
            base.HitTrueDamage(damage, attackerPoint, action);
            onHitMob.Invoke(this);
        }

        public override void Death()
        {
            onPreDeathMob.Invoke(this);
            base.Death();
            onPostDeathMob.Invoke(this);
        }

        protected override void _PostDeath()
        {
            Clear();
        }

        public override void HealHp(DamageInfo healInfo)
        {
            base.HealHp(healInfo);
            onHealMob.Invoke(this);
        }

        public override void DrainHeal(float actualDamage)
        {
            base.DrainHeal(actualDamage);
            onDrainHpMob.Invoke(this);
        }
    }
}
