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
using System.Reflection;
using UnityEngine;

namespace ProjectL
{
    public enum SkillType
    {
        None = 0,
        All = 1,
        Point = 2,
        Linear = 3,
    }

    public abstract class Skill : MonoBehaviour, ISkill, IRangeable, ISetting
    {
        [SettingValue]
        protected Sprite icon;
        public Sprite Icon => icon;

        [SettingValue]
        protected string displayName;
        public string DisplayName => Localization.GetLocalizedString(displayName);
        
        [SettingValue]
        protected string description;
        public string Description => Localization.GetLocalizedString(description);
        
        [SettingValue]
        protected SkillType attackType;
        public SkillAttackType SkillAttackType { get; private set; }
        public SkillGradeType GradeType { get; private set; }

        [SettingValue]
        protected float baseCooldown;
        public float Cooldown => baseCooldown * (1 - unit.CooldownReduceRate);

        public float UsedTime { get; protected set; }
        public float CanUseTime { get; protected set; }

        public float ElaspedCooldown => Time.time - UsedTime;
        public float RemainCooldown => CanUseTime - Time.time;

        [SettingValue]
        protected float skillStartDelay;
        [SettingValue]
        protected float skillEndDelay;
        [SettingValue]
        protected float castingTime;
        public bool IsCastingSkill => castingTime > 0;

        [SettingValue]
        protected bool isShakeCamera;
        [SettingValue]
        protected float shakeCameraStartDelay;
        [SettingValue]
        protected float shakeCameraDuration;
        
        [SerializeField]
        protected bool isUseLinearCastingEffect;
        
        [SerializeField]
        protected bool isOnlyRotationY;

        [SerializeField]
        protected GameObject skillEffect;
        [SerializeField]
        protected GameObject skillHitEffect;

        [SerializeField]
        protected SkillRangeData rangeData;

        protected string Name => GetType().Name;
        
        // 스킬 작업 끝나면 변수 및 디버깅 로그들 삭제
        public string DebugingName => Name;

        private Unit unit;
        public Unit Unit => unit;

        [SettingValue]
        protected float skillValuePercentage;
        protected virtual float SkillValue => skillValuePercentage;

        public CustomAction<Skill> onUsedSkill = new CustomAction<Skill>();

        public bool IsUse
        {
            get
            {
                if (CheckConditions() == false)
                    return false;

                return true;
            }
        }

        public void Init(Unit unit)
        {
            var attribute = this.GetType().GetCustomAttribute(typeof(SkillTypeAttribute)) as SkillTypeAttribute;

            if (attribute == null)
            {
                Debug.Log($"Skill.Init() attribute is null, Type : {this.GetType().Name}");
                return;
            }

            GradeType = attribute.GradeType;
            SkillAttackType = attribute.SkillAttackType;

            this.SetValue();
            
            this.unit = unit;
            this.unit.onDeath.Add(_StopAllCoroutine);

            _Init();
        }

        protected virtual void _Init(){}

        public virtual void Clear()
        {
            StopAllCoroutines();
            unit?.onDeath.Remove(_StopAllCoroutine);

            unit = null;
            UsedTime = 0;
        }

        private void _StopAllCoroutine(Unit unit) => StopAllCoroutines();
        
        public void Use(Point enemyPoint)
        {
            if (Unit == null)
            {
                return;
            }
            
            Debug.Log($"Skill.Use, Unit : {Unit.name}, Skill : {this.name}, AttackType : {this.attackType}");
            
            UsedTime = Time.time;
            CanUseTime = UsedTime + Cooldown;

            if (IsCastingSkill)
            {
                var point = enemyPoint;
                var nearestUnit = unit.NearestUnit;
                var nearestUnitPos = nearestUnit.transform.position;
                unit.SetCastingSkill(castingTime, () => UseSkillCasting(point, nearestUnitPos));

                if (unit.IsShowCastingRange)
                {
                    if (nearestUnit == null)
                    {
                        ShowSkillRange(enemyPoint.Position, castingTime);
                    }
                    else
                    {
                        ShowSkillRange(nearestUnit.transform.position, castingTime);
                    }
                }

                return;
            }

            UseSkill(enemyPoint);
            onUsedSkill.Invoke(this);
        }

        private void ShowSkillRange(Vector3 enemyPos, float showTime)
        {
            if (isUseLinearCastingEffect)
            {
                ShowSkillRangeLinear(enemyPos, showTime);
                return;
            }
            
            switch (attackType)
            {
                case SkillType.All:
                    DialogManager.Instance.OpenDialog<DlgWarning>("DlgWarning", dlg =>
                    {
                        dlg.Text = "Extra Skill is coming";
                        dlg.AutoClose = 2.5f;
                    });
                    break;
                case SkillType.Point:
                    ShowSkillRangePoint(enemyPos, showTime);
                    break;
                case SkillType.Linear:
                    ShowSkillRangeLinear(enemyPos, showTime);
                    break;
            }
        }

        private void ShowSkillRangePoint(Vector3 enemyPos, float showTime)
        {
            int maxRange = rangeData.MaxRange;
            var centerIndexOffset = rangeData.CenterIndexOffset;
            var direction = GetLeftAndForwardDirection(enemyPos);

            Vector2 targetPosOffset = (centerIndexOffset.y) * direction.forward + (centerIndexOffset.x) * direction.left;
            Vector3 skillRangePos = enemyPos + new Vector3(targetPosOffset.x, 0, targetPosOffset.y);

            ObjectPoolManager.Instance.New("CastingSkillRange", null, obj =>
              {
                  obj.transform.position = skillRangePos;
                  obj.transform.localScale = Vector3.one * maxRange;

                  var castingSkillRange = obj.GetComponent<CastingSkillRange>();
                  castingSkillRange.ShowSkillRange(showTime);
              });
        }

        private void ShowSkillRangeLinear(Vector3 enemyPos, float showTime)
        {
            var direction = (enemyPos - Unit.transform.position).normalized;
            
            ObjectPoolManager.Instance.New("LinearCastingSkillRange", null, obj =>
            {
                var castingSkillRange = obj.GetComponent<LinearCastingSkillRange>();
                var startPos = Unit.transform.position + direction * 0.5f + new Vector3(0, 1, 0);
                var endPos = enemyPos + new Vector3(0, 1, 0);
                castingSkillRange.ShowSkillRange(startPos, endPos, showTime);
            });
        }
        
        private void UseSkill(Point enemyPoint)
        {
            if (isShakeCamera)
            {
                StartCoroutine(ShakeCamera());
            }
            
            switch (attackType)
            {
                case SkillType.All:
                    StartCoroutine(UseSkillToAll(enemyPoint));
                    break;
                case SkillType.Point:
                    StartCoroutine(UseSkillToPoint(enemyPoint));
                    break;
                case SkillType.Linear:
                    StartCoroutine(UseSkillToLinear(enemyPoint));
                    break;
            }
        }

        IEnumerator ShakeCamera()
        {
            Debug.Log($"Skill.ShakeCamera(), delay : {shakeCameraStartDelay}, duration : {shakeCameraDuration}");
            
            yield return new WaitForSeconds(shakeCameraStartDelay);
            
            CameraManager.Instance.ShakeCamera(shakeCameraDuration);
        }
        
        private void UseSkillCasting(Point enemyPoint, Vector3 castingEnemyPos)
        {
            switch (attackType)
            {
                case SkillType.All:
                    StartCoroutine(UseSkillToAll(enemyPoint, castingEnemyPos));
                    break;
                case SkillType.Point:
                    StartCoroutine(UseSkillToPoint(enemyPoint, castingEnemyPos));
                    break;
                case SkillType.Linear:
                    StartCoroutine(UseSkillToLinear(enemyPoint, castingEnemyPos));
                    break;
            }
        }
        
        private IEnumerator UseSkillToAll(Point enemyPoint, Vector3? castingEnemyPos = null)
        {
            if (D.SelfPlayerGroup is null || D.SelfPlayerGroup.enemyPlayers.Count == 0)
                yield break;

            unit.IsUsingSkill = true;

            if (SkillAttackType == SkillAttackType.Buff
                || SkillAttackType == SkillAttackType.Passive)
            {
                _StartSkillEffect(Unit.transform.position);
            }
            else if (castingEnemyPos != null)
            {
                _StartSkillEffect((Vector3)castingEnemyPos);
            }
            else
            {
                _StartSkillEffect(enemyPoint.Position);
            }
            
            var baseDamageInfo = unit.DamageInfo;
            baseDamageInfo.AddDamagePercentage(SkillValue * 0.01f); //시전 시 공격력을 기준으로 데미지 계산(중간에 버프가 끝날 수 있으므로)

            Debug.LogWarning($"SkillName : {this.Name} will start");
            yield return new WaitForSeconds(skillStartDelay);
            Debug.LogWarning($"SkillName : {this.Name} start");

            foreach (var data in rangeData.rangeInfos)
            {
                var currentDamageInfo = baseDamageInfo;
                currentDamageInfo.AddDamagePercentage(data.damagePercentage * 0.01f); //시전 시 공격력을 기준으로 데미지 계산(중간에 버프가 끝날 수 있으므로)

                _UseSkillToAll(currentDamageInfo);

                yield return new WaitForSeconds(data.duration);
            }

            Debug.LogWarning($"SkillName : {this.Name} is will end");
            yield return new WaitForSeconds(skillEndDelay);
            Debug.LogWarning($"SkillName : {this.Name} is end");

            unit.IsUsingSkill = false;
        }

        protected abstract void _UseSkillToAll(DamageInfo damageInfo);

        private IEnumerator UseSkillToPoint(Point targetPoint, Vector3? castingEnemyPos = null)
        {
            if (D.SelfBoard is null)
                yield break;

            unit.IsUsingSkill = true;

            StartSkillEffectPoint(targetPoint, castingEnemyPos);
            
            var baseDamageInfo = unit.DamageInfo;
            baseDamageInfo.AddDamagePercentage(SkillValue * 0.01f); //시전 시 공격력을 기준으로 데미지 계산(중간에 버프가 끝날 수 있으므로)
            
            var direction = GetLeftAndForwardDirection(targetPoint.Position);

            Vector2 targetPointPos = new Vector2(targetPoint.Position.x, targetPoint.Position.z);

            Debug.LogWarning($"SkillName : {this.Name} will start");
            yield return new WaitForSeconds(skillStartDelay);
            Debug.LogWarning($"SkillName : {this.Name} start");

            foreach (var data in rangeData.rangeInfos)
            {
                var currentDamageInfo = baseDamageInfo;
                currentDamageInfo.AddDamagePercentage(data.damagePercentage * 0.01f); //시전 시 공격력을 기준으로 데미지 계산(중간에 버프가 끝날 수 있으므로)
                var rangeRow = data.rangeRow;

                int xCenterIndex = SkillRangeData.SKILL_RANGE / 2;
                int zCenterIndex = SkillRangeData.SKILL_RANGE / 2;

                for (int i = 0; i < SkillRangeData.SKILL_RANGE; i++)
                {
                    for (int j = 0; j < SkillRangeData.SKILL_RANGE; j++)
                    {
                        if (rangeRow[i].rangeData[j])
                        {
                            Vector2 targetPos = targetPointPos + (j - zCenterIndex) * direction.forward + (i - xCenterIndex) * direction.left;

                            var attackPoint = D.SelfBoard.FindPoint(targetPos, targetPoint.Depth - data.bottomDepth, targetPoint.Depth + data.topDepth);

                            if (attackPoint is not null)
                            {
                                Debug.Log($"Skill.UseSkillToPoint(), skill : {name}, i : {i}, j : {j}, TargetPos : {targetPos} AttackPoint : {attackPoint.name}, damage : {currentDamageInfo.Damage}");
                                _UseSkillToPoint(attackPoint, currentDamageInfo);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(data.duration);
            }

            Debug.LogWarning($"SkillName : {this.Name} is will end");
            yield return new WaitForSeconds(skillEndDelay);
            Debug.LogWarning($"SkillName : {this.Name} is end");

            unit.IsUsingSkill = false;
        }

        private IEnumerator UseSkillToLinear(Point targetPoint, Vector3? castingEnemyPos = null)
        {
            if (D.SelfBoard is null)
                yield break;

            unit.IsUsingSkill = true;
            
            StartSkillEffectLinear(targetPoint, castingEnemyPos);

            var baseDamageInfo = unit.DamageInfo;
            baseDamageInfo.AddDamagePercentage(SkillValue * 0.01f); //시전 시 공격력을 기준으로 데미지 계산(중간에 버프가 끝날 수 있으므로)

            Debug.LogWarning($"SkillName : {this.Name} will start");
            yield return new WaitForSeconds(skillStartDelay);
            Debug.LogWarning($"SkillName : {this.Name} start");

            foreach (var data in rangeData.rangeInfos)
            {
                var currentDamageInfo = baseDamageInfo;
                currentDamageInfo.AddDamagePercentage(data.damagePercentage * 0.01f); //시전 시 공격력을 기준으로 데미지 계산(중간에 버프가 끝날 수 있으므로)

                _UseSkillToPoint(targetPoint, currentDamageInfo);
                
                yield return new WaitForSeconds(data.duration);
            }

            Debug.LogWarning($"SkillName : {this.Name} is will end");
            yield return new WaitForSeconds(skillEndDelay);
            Debug.LogWarning($"SkillName : {this.Name} is end");

            unit.IsUsingSkill = false;
        }

        private void StartSkillEffectPoint(Point targetPoint, Vector3? castingEnemyPos)
        {
            var nearestUnit = unit.NearestUnit;

            if (SkillAttackType == SkillAttackType.Buff
                || SkillAttackType == SkillAttackType.Passive)
            {
                _StartSkillEffect(Unit.transform.position, Unit.transform);
            }
            else if (castingEnemyPos != null)
            {
                _StartSkillEffect((Vector3)castingEnemyPos);
            }
            else if (nearestUnit == null)
            {
                _StartSkillEffect(targetPoint.Position);
            }
            else
            {
                _StartSkillEffect(nearestUnit.transform.position);
            }
        }

        private void StartSkillEffectLinear(Point targetPoint, Vector3? castingEnemyPos)
        {
            var nearestUnit = unit.NearestUnit;

            var endPos = Vector3.zero;

            if (SkillAttackType == SkillAttackType.Buff
                || SkillAttackType == SkillAttackType.Passive)
            {
                endPos = Unit.transform.position;
            }
            else if (castingEnemyPos != null)
            {
                endPos = (Vector3)castingEnemyPos;
            }
            else if (nearestUnit == null)
            {
                endPos = targetPoint.Position;
            }
            else
            {
                endPos = nearestUnit.transform.position;
            }

            float distance = Vector3.Distance(Unit.transform.position, endPos);
            _StartSkillEffectLinear(endPos, distance);
        }

        private (Vector2 left, Vector2 forward) GetLeftAndForwardDirection(Vector3 targetPos)
        {
            Vector3 direction = targetPos - unit.transform.position;

            if (direction == Vector3.zero)
            {
                direction = unit.transform.forward;
            }
            
            Vector2 directionXZ = new Vector2(direction.x, direction.z).normalized;
            Vector2 left = new Vector2(directionXZ.y * -1, directionXZ.x) * Point.POINT_SCALE;
            Vector2 forward = directionXZ * Point.POINT_SCALE;

            return (left, forward);
        }

        protected abstract void _UseSkillToPoint(Point targetPoint, DamageInfo damageInfo);

        private bool CheckConditions()
        {
            if (CheckCoolDown() == false)
                return false;

            if (CheckTrigger() == false)
                return false;

            return true;
        }

        private bool CheckCoolDown()
        {
            if (Time.time > CanUseTime)
            {
                return true;
            }

            return false;
        }

        protected virtual bool CheckTrigger() => true;

        /// <summary>
        /// 쿨타임 감소(남은 시간 대비 감소)
        /// </summary>
        /// <param name="rate">0 ~ 1</param>
        public void ReduceCooldownRate(float rate)
        {
            float reduceTime = RemainCooldown * rate;
            CanUseTime -= reduceTime;
        }
        
        /// <summary>
        /// 스킬 이펙트 재생
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxMoveDistance">-1 is infinity</param>
        protected void _StartSkillEffect(Vector3 position, Transform parent = null)
        {
            if (SkillManager.Instance.isShowSkillEffect == false)
            {
                return;
            }
            
            if(skillEffect == null)
            {
                return;
            }

            bool isFixedYRotation = isOnlyRotationY;
            ObjectPoolManager.Instance.New(skillEffect.name, parent, obj =>
            {
                obj.transform.position = position;
                
                if (isFixedYRotation)
                {
                    var positionXZ = new Vector3(position.x, Unit.transform.position.y, position.z);
                    obj.transform.rotation = Quaternion.LookRotation(positionXZ - Unit.transform.position);
                }
                else
                {
                    obj.transform.rotation = Quaternion.LookRotation(position - Unit.transform.position);
                }
                
                var effect = obj.GetComponent<SkillEffect>();
                effect.StartEffect(this);
            });
        }
        
        /// <summary>
        /// 스킬 이펙트 재생
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxMoveDistance">-1 is infinity</param>
        protected void _StartSkillEffectLinear(Vector3 position, float maxMoveDistance, Transform parent = null)
        {
            if (SkillManager.Instance.isShowSkillEffect == false)
            {
                return;
            }
            
            if(skillEffect == null)
            {
                return;
            }

            ObjectPoolManager.Instance.New(skillEffect.name, parent, obj =>
            {
                obj.transform.position = Unit.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(position - Unit.transform.position);
                var effect = obj.GetComponent<SkillEffect>();
                effect.MaxMoveDistance = maxMoveDistance;
                
                if (effect.collider != null)
                {
                    effect.collider.position = position;
                }
                
                effect.StartEffect(this);
            });
        }
        
        protected void _StartHitEffectRandomPos(Vector3 position, float scaleY, Transform parent = null)
        {
            var halfScaleY = scaleY * 0.5f;
            var randomOffsetX = UnityEngine.Random.Range(-halfScaleY, halfScaleY);
            var randomOffsetY = UnityEngine.Random.Range(-halfScaleY, halfScaleY);
            var randomOffsetZ = UnityEngine.Random.Range(-halfScaleY, halfScaleY);

            var randomPos = position + new Vector3(randomOffsetX, halfScaleY + randomOffsetY, randomOffsetZ);

            _StartHitEffect(randomPos, parent);
        }
        
        protected void _StartHitEffect(Vector3 position, Transform parent = null)
        {
            if (SkillManager.Instance.isShowSkillEffect == false)
            {
                return;
            }
            
            if (skillHitEffect == null)
            {
                return;
            }

            ObjectPoolManager.Instance.New(skillHitEffect.name, parent, obj =>
            {
                obj.transform.position = position;
                obj.transform.rotation = Quaternion.LookRotation(Unit.transform.position - position);
                var effect = obj.GetComponent<SkillEffect>();
                effect.StartEffect(this);
            });
        }
    }
}
