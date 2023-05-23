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
using UnityEngine;

namespace ProjectL
{
    [SkillType(SkillAttackType.Special, SkillGradeType.Common)]
    public class EatUnit : Skill
    {
        [SerializeField]
        private Unit eatingUnit;
        private IEatingUnit EatingUnit { get; set; }
        
        private void Awake()
        {
            EatingUnit = eatingUnit as IEatingUnit;
        }

        protected override bool CheckTrigger()
        {
            Unit hitUnit = null;
            
            switch (Unit.ownerType)
            {
                case OwnerType.My: 
                    hitUnit = Unit.BasePoint.GetEnemyMobs().FirstOrDefault();
                    break;
                case OwnerType.Enemy: 
                    hitUnit = Unit.BasePoint.GetAllyMobs().FirstOrDefault();
                    break;
            }

            return hitUnit is not null;
        }
        
        protected override void _UseSkillToAll(DamageInfo damageInfo)
        {
        }

        protected override void _UseSkillToPoint(Point targetPoint, DamageInfo damageInfo)
        {
            Unit hitUnit = null;
            
            switch (Unit.ownerType)
            {
                case OwnerType.My: 
                    hitUnit = targetPoint.GetEnemyMobs().FirstOrDefault();
                    break;
                case OwnerType.Enemy: 
                    hitUnit = targetPoint.GetAllyMobs().FirstOrDefault();
                    break;
            }

            if (hitUnit != null)
            {
                _StartHitEffectRandomPos(hitUnit.transform.position, hitUnit.Scale.y);
                UseSkill(hitUnit);
            }
        }
        
        protected void UseSkill(Unit hitUnit)
        {
            EatingUnit.Eat(hitUnit, Cooldown);
        }
    }
}