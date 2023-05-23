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

// using UnityEngine;
//
// namespace ProjectL
// {
//     [MobType(GradeType.Special, MobType.Hero)]
//     public class August : PlayerMob
//     {
//         private Unit copyUnit;
//
//         private void OnEnable()
//         {
//             PlayRoundLogic.Instance.AddStatusEvent(RoundLogic.PlayRound, ChangeUnit);
//         }
//
//         private void OnDisable()
//         {
//             PlayRoundLogic.Instance.RemoveStatusEvent(RoundLogic.PlayRound, ChangeUnit);
//         }
//
//         private void ChangeUnit()
//         {
//             if(D.SelfPlayer.Mobs.Count == 1)
//             {
//                 return;
//             }
//
//             int index = Random.Range(0, D.SelfPlayer.Mobs.Count);
//             var unit = D.SelfPlayer.Mobs[index];
//
//             if(unit == this)
//             {
//                 index = index == 0 ? index + 1 : index - 1;
//                 unit = D.SelfPlayer.Mobs[index];
//             }
//
//             if (unit.name.Equals(name))
//             {
//                 unit = D.SelfPlayer.Mobs.Find(mob => mob.name.Equals(name) == false);
//
//                 if(unit == null)
//                 {
//                     return;
//                 }
//             }
//
//             ObjectPoolManager.Instance.New(unit.name, transform, obj =>
//             { 
//                 copyUnit = obj.GetComponent<Unit>();
//                 skillGroup = copyUnit.SkillGroup;
//                 attackType = copyUnit.AttackType;
//             });
//         }
//     }
// }
