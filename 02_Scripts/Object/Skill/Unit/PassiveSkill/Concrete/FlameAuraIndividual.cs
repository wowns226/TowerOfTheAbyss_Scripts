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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectL
{
    [SkillType(SkillAttackType.Passive, SkillGradeType.Individual)]
    public class FlameAuraIndividual : PassiveSkill
    {
        private Coroutine flameAuraCoroutine;
        
        protected override void Active()
        {
            Debug.Log($"FlameAuraIndividual.Active(), Skill : {this.name}");
            
            if (flameAuraCoroutine != null)
            {
                StopCoroutine(flameAuraCoroutine);
                flameAuraCoroutine = null;
            }
            
            flameAuraCoroutine = StartCoroutine(FlameAura());
        }

        protected override void InActive()
        {
            Debug.Log($"FlameAuraIndividual.InActive(), Skill : {this.name}");
            
            if (flameAuraCoroutine != null)
            {
                StopCoroutine(flameAuraCoroutine);
                flameAuraCoroutine = null;
            }
        }
        
        IEnumerator FlameAura()
        {
            while (true)
            {
                var enemies = new List<Unit>();
                
                if (Unit.ownerType == OwnerType.My)
                {
                     enemies = Unit.BasePoint.GetEnemyMobs();
                }
                else
                {
                     enemies = Unit.BasePoint.GetAllyMobs();
                }

                var damageInfo = Unit.DamageInfo;
                damageInfo.Damage = Unit.Defense * SkillValue * 0.01f;
                
                _StartSkillEffect(Unit.transform.position);
                
                enemies.ForEach(enemy =>
                {
                    enemy.Hit(damageInfo);
                    _StartHitEffectRandomPos(enemy.transform.position, enemy.Scale.y);
                });

                yield return new WaitForSeconds(Cooldown);
            }
        }

    }
}