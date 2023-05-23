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
using HighlightPlus;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectL
{
    public enum OwnerType
    {
        None = 0,
        My = 1,
        Enemy = 2,
    }

    public enum UnitType
    {
        Mob = 0,
        Building = 1,
    }

    public enum AttackType
    {
        Melee = 0,
        Ranged = 1,
        Heal = 2,
    }

    public enum StatType
    {
        Damage = 0,
        Defense = 1,
        Heal = 2,
        Hp = 3,
        Speed = 4,
        CriticalProbability = 5,
        CriticalDamagePercentage = 6,
        DrainProbability = 7,
        Range = 8,
        DropGold = 9,
        NaturalHealingValue = 10,
        NaturalHealingTime = 11,
    }

    public struct DamageInfo
    {
        public DamageInfo(float damage, float criticalProbability, float criticalPercentage, bool isGuaranteedCritical, AttackType attackType)
        {
            this.damage = damage;
            this.criticalProbability = criticalProbability;
            this.criticalPercentage = criticalPercentage;
            isCritical = false;
            this.isGuaranteedCritical = isGuaranteedCritical;
            this.attackType = attackType;
        }

        private float damage;
        private float criticalProbability;
        private float criticalPercentage;
        private bool isCritical;
        private bool isGuaranteedCritical;
        private AttackType attackType;

        public float Damage
        {
            get => IsCritical ? damage * criticalPercentage * 0.01f : damage;
            set => damage = value;
        } 
        
        public bool IsCritical => isCritical;
        public AttackType AttackType => attackType;
        
        public void AddDamage(float value) => damage += value;
        public void AddDamagePercentage(float damagePercentage) => damage *= damagePercentage;

        public void Calc()
        {
            if (isGuaranteedCritical)
            {
                isCritical = true;
                return;
            }
            
            if(Random.Range(0f, 100f) <= criticalProbability)
            {
                isCritical = true;
            }
        }

    }

    public abstract class Unit : MonoBehaviour, ISetting, IUnitState
    {
        private const float DEFENSE_PER = 100;
        private static readonly int cutoff = Shader.PropertyToID("_Cutoff");

        #region Spline Field

        private PathCreator pathCreator;
        private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
        private float distanceTravelled;
        private bool isSplineMove;
        public bool IsSplineMove => isSplineMove;

        #endregion

        [SerializeField]
        protected Animator unitAnimator;

        public Transform renderMesh;
        public Transform deathMesh;
        [HideInInspector]
        public List<Renderer> deathMeshRenderer;

        [HideInInspector]
        public Player player;

        [HideInInspector]
        public OwnerType ownerType = OwnerType.None;

        [SettingValue]
        protected RaceType raceType;
        public RaceType RaceType => raceType;

        public GradeType GradeType { get; set; }

        public MobType MobType { get; set; }
        
        protected UnitState<Unit> state;

        private float endStunTime;

        public List<TimerInfo<Unit>> buffSkills = new List<TimerInfo<Unit>>();

        public CustomAction<Vector3> onMoved = new CustomAction<Vector3>();
        public CustomAction<Quaternion> onRotated = new CustomAction<Quaternion>();
        public CustomAction<Unit> onDeath = new CustomAction<Unit>();
        public CustomAction<Unit> onSpawn = new CustomAction<Unit>();

        public Vector3 Scale { get; protected set; }

        private Queue<Point> targetPoints = new Queue<Point>();
        public Queue<Point> TargetPoints => targetPoints;
        public Point DestinationPoint => TargetPoints.LastOrDefault();
        
        [SerializeField]
        protected SkillGroup skillGroup;
        public SkillGroup SkillGroup => skillGroup;
        public bool IsUseExtraSkill => skillGroup.ExtraSkill != null && skillGroup.ExtraSkill.IsUse;

        private float castingEndTime;
        public Action<float> onStartCasting;
        private Action castingCallback;

        [SettingValue]
        protected Sprite icon; 
        public Sprite Icon => icon;

        [SettingValue]
        protected Sprite unitMiniImage;
        public Sprite UnitMiniImage => unitMiniImage;
        
        [SettingValue]
        protected Sprite unitImage;
        public Sprite UnitImage => unitImage;
        
        [SettingValue]
        protected string displayName;
        public string DisplayName => Localization.GetLocalizedString(displayName);
        
        [SettingValue]
        protected string description;
        public string Description => Localization.GetLocalizedString(description);

        public bool IsUsingSkill { get; set; }
        public bool IsCasting => castingEndTime > Time.time;
        public bool IsCastingComplete => castingCallback != null && IsCasting == false;
        [SettingValue]
        protected bool isShowCastingRange;
        public bool IsShowCastingRange => isShowCastingRange;

        public bool IsInvincible { get; set; }
        public bool IsStunning => endStunTime > Time.time;

        protected bool isSpawn;
        public bool IsSpawn => isSpawn;

        protected bool isDeath;
        public bool IsDeath => isDeath;

        public bool IsEaten { get; set; }
        
        protected Point basePoint;
        public Point BasePoint => basePoint;
        protected Point targetPoint;
        public Point TargetPoint
        {
            get => targetPoint;
            set => targetPoint = value;
        }

        protected Vector3 targetPos;
        public Vector3 TargetPos => targetPos;
        public bool IsLeftToTarget => GetCrossToTargetPos(TargetPos).y < 0;
        public bool IsRightToTarget => GetCrossToTargetPos(TargetPos).y > 0;
        public bool IsSideToTarget
        {
            get
            {
                float dot = GetDotToTargetPos(TargetPos);

                return dot is > -0.3f and < 0.3f;
            }
        }
        public bool IsBackToTarget
        {
            get
            {
                float dot = GetDotToTargetPos(TargetPos);

                return dot <= -0.3f;
            }
        }

        public Point BattlePoint => AttackType == AttackType.Heal ? AllyPoint : EnemyPoint;
        public Point EnemyPoint => SearchEnemyPoint();
        public Point AllyPoint => SearchAllyPoint();

        [SettingValue]
        public float DeathDelay { get; set; }

        [SettingValue]
        protected AttackType attackType;
        public AttackType AttackType => attackType;
        public bool IsMelee => attackType == AttackType.Melee;

        [SettingValue]
        protected UnitType unitType;
        public UnitType UnitType => unitType;

        private Dictionary<StatType, int> upgrades = new Dictionary<StatType, int>();
        private Dictionary<StatType, float> changeStats = new Dictionary<StatType, float>();
        private Dictionary<StatType, float> statPercentages = new Dictionary<StatType, float>();

        public Action<float> onChangedHp;

        [SerializeField]
        protected HighlightEffect highlightEffect;

        protected abstract List<Unit> BattleUnits { get; }

        public Unit NearestUnit => BattleUnits?.OrderBy(unit => Vector3.Distance(transform.position, unit.transform.position) - unit.Scale.x * 0.4f).First();

        public bool IsNearestUnitInAttackRange => Vector3.Distance(transform.position, NearestUnit.transform.position) > AttackRange + NearestUnit.Scale.x * 0.4f ? false : true;

        //value 0 ~ 1
        public float CooldownReduceRate => player.SkillCooldownReduceRate;

        #region Stats

        public DamageInfo DamageInfo =>
            new DamageInfo(AttackType == AttackType.Heal ? Heal : Damage, CriticalProbability, CriticalDamagePercentage,
                IsGuaranteedCritical, AttackType);

        #region Natural Healing Value

        private float lastHealingTime;
        public bool IsDoneNaturalHealingTime => lastHealingTime + NaturalHealingTime < Time.time;

        [SettingValue]
        protected float naturalHealingValue;
        public float BaseNaturalHealingValue => naturalHealingValue;
        public float NaturalHealingValue
        {
            get
            {
                float result = naturalHealingValue + changeStats[StatType.NaturalHealingValue];

                result += upgrades[StatType.NaturalHealingValue] * UpgradePerNaturalHealingValue;
                result *= (100 + NaturalHealingValuePercentage) * 0.01f;

                return result;
            }
            protected set => naturalHealingValue = value;
        }

        private float UpgradePerNaturalHealingValue { get; } = 1f;

        public float NaturalHealingValuePercentage => statPercentages[StatType.NaturalHealingValue];

        public string NaturalHealingValueAddedDisplayValue => $"{(NaturalHealingValueAddedValue >= 0 ? NaturalHealingValueAddedValue : "<color=red>" + NaturalHealingValueAddedValue + "</color>"):+#;-#;#,##0}";
        public float NaturalHealingValueAddedValue => NaturalHealingValue - BaseNaturalHealingValue;
        
        #endregion

        #region Natural Healing Time

        [SettingValue]
        protected float naturalHealingTime;
        public float BaseNaturalHealingTime => naturalHealingTime;
        public float NaturalHealingTime
        {
            get
            {
                float result = naturalHealingTime + changeStats[StatType.NaturalHealingTime];

                result += upgrades[StatType.NaturalHealingTime] * UpgradePerNaturalHealingTime;
                result *= (100 + NaturalHealingTimePercentage) * 0.01f;

                return result;
            }
            protected set => naturalHealingTime = value;
        }

        private float UpgradePerNaturalHealingTime { get; } = -1f;

        public float NaturalHealingTimePercentage => statPercentages[StatType.NaturalHealingTime];
        
        public string NaturalHealingTimeAddedDisplayValue => $"{(NaturalHealingTimeAddedValue >= 0 ? NaturalHealingTimeAddedValue : "<color=red>" + NaturalHealingTimeAddedValue + "</color>"):+#;-#;#,##0}";
        public float NaturalHealingTimeAddedValue => NaturalHealingTime - BaseNaturalHealingTime;
        
        #endregion

        #region Hp

        private const float MAXHP_MINVALUE = 100f;
        
        protected float hp;
        public float Hp
        {
            get => hp;
            protected set
            {
                var result = value;

                if (MaxHp < result)
                    result = MaxHp;

                hp = result;
                onChangedHp?.Invoke(hp);

                if (hp < 0)
                    Death();
            }
        }

        [SettingValue]
        protected float maxHp;
        public float BaseMaxHp => maxHp;
        public float MaxHp
        {
            get
            {
                float result = maxHp + changeStats[StatType.Hp];

                result += upgrades[StatType.Hp] * UpgradePerHp;
                result *= (100 + HpPercentage) * 0.01f;

                if (result < MAXHP_MINVALUE)
                {
                    result = MAXHP_MINVALUE;
                }
                
                return result;
            }
            protected set
            {
                float result = value;
                
                if (result < MAXHP_MINVALUE)
                {
                    result = MAXHP_MINVALUE;
                }
                
                float diff = result - maxHp;
                Hp += diff;
                maxHp = result;
            }
        }

        [SettingValue]
        protected float UpgradePerHp { get; set; }

        public float HpPercentage => statPercentages[StatType.Hp];
        
        public float RemainHpPercentage => (Hp / MaxHp) * 100;

        public string MaxHpAddedDisplayValue => $"{(MaxHpAddedValue >= 0 ? MaxHpAddedValue : "<color=red>" + MaxHpAddedValue + "</color>"):+#;-#;#,##0}";
        public float MaxHpAddedValue => MaxHp - BaseMaxHp;
        
        #endregion

        #region Damage

        [SettingValue]
        protected float damage;
        public float BaseDamage => damage;
        public float Damage
        {
            get
            {
                float result = damage + changeStats[StatType.Damage];

                result += upgrades[StatType.Damage] * UpgradePerDamage;
                result *= (100 + DamagePercentage) * 0.01f;

                return result;
            }
            protected set => damage = value;
        }

        [SettingValue]
        protected float UpgradePerDamage { get; set; }
        public float DamagePercentage => statPercentages[StatType.Damage];
        
        public string DamageAddedDisplayValue => $"{(DamageAddedValue >= 0 ? DamageAddedValue : "<color=red>" + DamageAddedValue + "</color>"):+#;-#;#,##0}";
        public float DamageAddedValue => Damage - BaseDamage;
        
        #endregion

        #region Heal

        [SettingValue]
        protected float heal;
        public float BaseHeal => heal;
        public float Heal
        {
            get
            {
                float result = heal + changeStats[StatType.Heal];

                result += upgrades[StatType.Heal] * UpgradePerHeal;
                result *= (100 + HealPercentage) * 0.01f;

                return result;
            }
            protected set => heal = value;
        }

        [SettingValue]
        protected float UpgradePerHeal { get; set; }

        public float HealPercentage => statPercentages[StatType.Heal];
        
        public string HealAddedDisplayValue => $"{(HealAddedValue >= 0 ? HealAddedValue : "<color=red>" + HealAddedValue + "</color>"):+#;-#;#,##0}";
        public float HealAddedValue => Heal - BaseHeal;
        
        #endregion

        #region CriticalProbability

        public bool IsGuaranteedCritical { get; set; }
        [SettingValue]
        protected float criticalProbability;
        public float BaseCriticalProbability => criticalProbability;
        public float CriticalProbability
        {
            get
            {
                float result = criticalProbability + changeStats[StatType.CriticalProbability];

                result += upgrades[StatType.CriticalProbability] * UpgradePerCriticalProbability;
                result *= (100 + CriticalProbabilityPercentage) * 0.01f;

                return result;
            }
            protected set => criticalProbability = value;
        }

        private float UpgradePerCriticalProbability { get; } = 1f;
        public float CriticalProbabilityPercentage => statPercentages[StatType.CriticalProbability];
        
        public string CriticalProbabilityAddedDisplayValue => $"{(CriticalProbabilityAddedValue >= 0 ? CriticalProbabilityAddedValue : "<color=red>" + CriticalProbabilityAddedValue + "</color>"):+#;-#;#,##0}";
        public float CriticalProbabilityAddedValue => CriticalProbability - BaseCriticalProbability;
        
        #endregion

        #region CriticalDamagePercentage

        [SettingValue]
        protected float criticalDamagePercentage;
        public float BaseCriticalDamagePercentage => criticalDamagePercentage;
        public float CriticalDamagePercentage
        {
            get
            {
                float result = criticalDamagePercentage + changeStats[StatType.CriticalDamagePercentage];

                result += upgrades[StatType.CriticalDamagePercentage] * UpgradePerCriticalDamagePercentage;
                result *= (100 + CriticalDamageFinalPercentage) * 0.01f;

                return result;
            }
            protected set => criticalDamagePercentage = value;
        }

        private float UpgradePerCriticalDamagePercentage { get; } = 1f;

        public float CriticalDamageFinalPercentage => statPercentages[StatType.CriticalDamagePercentage];
        
        public string CriticalDamagePercentageAddedDisplayValue => $"{(CriticalDamagePercentageAddedValue >= 0 ? CriticalDamagePercentageAddedValue : "<color=red>" + CriticalDamagePercentageAddedValue + "</color>"):+#;-#;#,##0}";
        public float CriticalDamagePercentageAddedValue => CriticalDamagePercentage - BaseCriticalDamagePercentage;
        
        #endregion

        #region Defense

        
        [SettingValue]
        protected float reductionMeleeAttack;
        public float ReductionMeleeAttack => reductionMeleeAttack;
        [SettingValue]
        protected float reductionRangedAttack;
        public float ReductionRangedAttack => reductionRangedAttack;
        
        public float ReductionByDefenseRate
        {
            get
            {
                float defense = Defense;
                float rate = 50f;
                float result = 0;

                if (defense > DEFENSE_PER * 6)
                {
                    return 0.99f;
                }

                while (defense > 0)
                {
                    float value = defense > DEFENSE_PER ? DEFENSE_PER : defense % DEFENSE_PER;
                    result += rate * (value / DEFENSE_PER);

                    rate *= 0.5f;
                    defense -= value;
                }

                return result * 0.01f;
            }
        }

        [SettingValue]
        protected float defense;
        public float BaseDefense => defense;
        public float Defense
        {
            get
            {
                float result = defense + changeStats[StatType.Defense];

                result += upgrades[StatType.Defense] * UpgradePerDefense;
                result *= (100 + DefensePercentage) * 0.01f;

                return result;
            }
            protected set => defense = value;
        }

        [SettingValue]
        protected float UpgradePerDefense { get; set; }
        public float DefensePercentage => statPercentages[StatType.Defense];
        
        public string DefenseAddedDisplayValue => $"{(DefenseAddedValue >= 0 ? DefenseAddedValue : "<color=red>" + DefenseAddedValue + "</color>"):+#;-#;#,##0}";
        public float DefenseAddedValue => Defense - BaseDefense;
        
        #endregion

        #region Range

        public int AttackRange => AttackType == AttackType.Melee ? (int)(Scale.x * 0.7f) + 1 : Range;
        [SettingValue]
        protected int range;
        public int BaseRange => range;
        public int Range
        {
            get
            {
                if (AttackType == AttackType.Melee)
                {
                    return Point.POINT_SCALE + 1;
                }

                float result = range + changeStats[StatType.Range];

                result += upgrades[StatType.Range] * UpgradePerRange;
                result *= (100 + RangePercentage) * 0.01f;

                return (int)result;
            }
            protected set => range = value;
        }

        private int UpgradePerRange { get; } = 1;
        public float RangePercentage => statPercentages[StatType.Range];
        
        public string RangeAddedDisplayValue => $"{(RangeAddedValue >= 0 ? RangeAddedValue : "<color=red>" + RangeAddedValue + "</color>"):+#;-#;#,##0}";
        public float RangeAddedValue => Range - BaseRange;
        
        #endregion

        #region Speed

        [SettingValue]
        protected float speed;
        public float BaseSpeed => speed;
        public float Speed
        {
            get
            {
                float result = speed + changeStats[StatType.Speed];

                result += upgrades[StatType.Speed] * UpgradePerSpeed;
                result *= (100 + SpeedPercentage) * 0.01f;

                return result;
            }
            protected set => speed = value;
        }

        private float UpgradePerSpeed { get; } = 1;
        public float SpeedPercentage => statPercentages[StatType.Speed];
        
        public string SpeedAddedDisplayValue => $"{(SpeedAddedValue >= 0 ? SpeedAddedValue : "<color=red>" + SpeedAddedValue + "</color>"):+#;-#;#,##0}";
        public float SpeedAddedValue => Speed - BaseSpeed;
        
        #endregion

        #region DrainProbability

        [SettingValue]
        protected float drainProbability;
        public float BaseDrainProbability => drainProbability;
        public float DrainProbability
        {
            get
            {
                float result = drainProbability + changeStats[StatType.DrainProbability];

                result += upgrades[StatType.DrainProbability] * UpgradePerDrainProbability;
                result *= (100 + DrainProbabilityPercentage) * 0.01f;

                return result;
            }
            protected set => drainProbability = value;
        }

        private float UpgradePerDrainProbability { get; } = 1;

        public float DrainProbabilityPercentage => statPercentages[StatType.DrainProbability];
        
        public string DrainProbabilityAddedDisplayValue => $"{(DrainProbabilityAddedValue >= 0 ? DrainProbabilityAddedValue : "<color=red>" + DrainProbabilityAddedValue + "</color>"):+#;-#;#,##0}";
        public float DrainProbabilityAddedValue => DrainProbability - BaseDrainProbability;
        
        #endregion

        #region DropGold

        [SettingValue]
        protected int dropGold;
        public int BaseDropGold => dropGold;
        public int DropGold
        {
            get
            {
                float result = dropGold + changeStats[StatType.DropGold];

                result += upgrades[StatType.DropGold] * UpgradePerDropGold;
                result *= (100 + DropGoldPercentage) * 0.01f;

                return Mathf.RoundToInt(result);
            }
            protected set => dropGold = value;
        }

        private float UpgradePerDropGold { get; } = 1;

        public float DropGoldPercentage => statPercentages[StatType.DropGold];
        
        public string DropGoldAddedDisplayValue => $"{(DropGoldAddedValue >= 0 ? DropGoldAddedValue : "<color=red>" + DropGoldAddedValue + "</color>"):+#;-#;#,##0}";
        public float DropGoldAddedValue => DropGold - BaseDropGold;
        
        #endregion

        #endregion

        protected virtual void Awake()
        {
            using (new PerfTimerRegion("deathMesh.GetComponentsInChildren<Renderer>().ToList()"))
            {
                if (deathMesh != null)
                {
                    deathMeshRenderer = deathMesh.GetComponentsInChildren<Renderer>().ToList();
                }
            }

            InitDictionary();
        }

        protected virtual void Start()
        {
        }

        public virtual void Init(Player player)
        {
            Debug.Log($"{this.gameObject.name} Init In Player {player}");

            this.SetValue();
            
            InitPlayer(player);
            InitChangeStat();
            InitStatPercentage();
            InitUpgrade();
            InitSkills();
            InitState();
            
            player.AddUnit(this);
            Scale = transform.localScale;
        }

        private void Update()
        {
            if (IsDeath || IsSpawn == false)
            {
                return;
            }

            if (state == null)
            {
                return;
            }

            if (IsDoneNaturalHealingTime)
            {
                NaturalHealing();
            }

            if (IsStunning)
            {
                if (IsCasting)
                {
                    CancelCastingSkill();
                }

                return;
            }

            if (IsCasting)
            {
                return;
            }

            if (IsCastingComplete)
            {
                ExecuteCastingSkill();
            }

            state.Execute(this);
        }

        private void NaturalHealing()
        {
            HealHp(NaturalHealingValue, false);
            lastHealingTime = Time.time;
        }

        public virtual void Clear()
        {
            Debug.Log($"Unit.Clear(), Name : {name}");

            StopAllCoroutines();
            
            IsInvincible = false;
            IsGuaranteedCritical = false;
            
            skillGroup.Clear();

            isSpawn = false;
            isDeath = false;

            basePoint = null;
            targetPoint = null;

            targetPoints.Clear();

            ClearBuffSkill();
            
            ClearStats();
            ClearUpgrade();

            Hp = MaxHp;
            
            ObjectPoolManager.Instance.Return(gameObject);
        }

        private void ClearStats()
        {
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                changeStats[statType] = 0;
                statPercentages[statType] = 0;
            }
        }

        private void ClearUpgrade()
        {
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                upgrades[statType] = 0;
            }
        }
        
        public virtual void InitPlayer(Player player)
        {
            this.player = player;
        }
        
        public virtual void InitState()
        {
            state = new Idle<Unit>();
        }

        #region StatPercentage & Upgrade

        private void InitDictionary()
        {
            foreach (StatType stat in Enum.GetValues(typeof(StatType)))
            {
                upgrades.TryAdd(stat, 0);
                changeStats.TryAdd(stat, 0);
                statPercentages.TryAdd(stat, 0);
            }
        }

        private void InitUpgrade()
        {
            Debug.Log($"Unit.InitUpgrade(), Unit : {this.name}");
            
            if (player == null)
            {
                Debug.Log("Unit.InitUpgrade : Player is null");
                return;
            }

            foreach (var statType in Enum.GetValues(typeof(StatType)))
            {
                int upgradeValue = player.GetUpgradeLevel(UnitType, (StatType)statType, attackType);
                ChangeUpgradeValue((StatType)statType, upgradeValue);
            }
        }

        public void Upgrade(StatType statType, int upgradeValue)
        {
            Debug.Log($"Unit.Upgrade(), Unit : {name}, StatType : {statType}, CurrentValue : {upgrades[statType]}, AddValue : {upgradeValue}");
            
            upgrades[statType] += upgradeValue;

            if (statType == StatType.Hp)
            {
                Hp += UpgradePerHp * upgradeValue;
            }
        }
        
        public void ChangeUpgradeValue(StatType statType, int changeValue)
        {
            Debug.Log($"Unit.ChangeUpgradeValue(), Unit : {name}, StatType : {statType}, CurrentValue : {upgrades[statType]}, ChangeValue : {changeValue}");
            
            var diff = changeValue - upgrades[statType];
            upgrades[statType] = changeValue;

            if (statType == StatType.Hp)
            {
                Hp += UpgradePerHp * diff;
            }
        }
        
        private void InitChangeStat()
        {
            Debug.Log($"Unit.InitChangeStat(), Unit : {this.name}");
            
            if (player == null)
            {
                Debug.Log("Unit.InitChangeStat : Player is null");
                return;
            }

            foreach (var statType in Enum.GetValues(typeof(StatType)))
            {
                float changeStat = player.GetChangeStat(UnitType, (StatType)statType, attackType);
                ChangeStat((StatType)statType, changeStat);
            }
        }

        public void UpgradeStat(StatType statType, float changeValue)
        {
            Debug.Log($"Unit.UpgradeStat(), Unit : {name}, StatType : {statType}, CurrentValue : {changeStats[statType]}, AddValue : {changeValue}");
            
            changeStats[statType] += changeValue;
            
            if (statType == StatType.Hp)
            {
                Hp += changeValue;
            }
        }
        
        public void ChangeStat(StatType statType, float changeValue)
        {
            Debug.Log($"Unit.ChangeStat(), Unit : {name}, StatType : {statType}, CurrentValue : {changeStats[statType]}, ChangeValue : {changeValue}");
            
            var diff = changeValue - changeStats[statType];
            changeStats[statType] = changeValue;
            
            if (statType == StatType.Hp)
            {
                Hp += diff;
            }
        }
        
        private void InitStatPercentage()
        {
            Debug.Log($"Unit.InitStatPercentage(), Unit : {this.name}");
            
            if (player == null)
            {
                Debug.Log("Unit.InitStatPercentage : Player is null");
                return;
            }

            foreach (var statType in Enum.GetValues(typeof(StatType)))
            {
                float percentage = player.GetStatPercentage(UnitType, (StatType)statType, attackType);
                ChangeStatPercentage((StatType)statType, percentage);
            }
        }

        public void UpgradeStatPercentage(StatType statType, float changeValue)
        {
            Debug.Log($"Unit.UpgradeStatPercentage(), Unit : {name}, StatType : {statType}, CurrentValue : {statPercentages[statType]}, AddValue : {changeValue}");
            statPercentages[statType] += changeValue;
            
            if (statType == StatType.Hp)
            {
                HealHpPercentage(changeValue);
            }
        }
        
        public void ChangeStatPercentage(StatType statType, float changeValue)
        {
            Debug.Log($"Unit.ChangeStatPercentage(), Unit : {name}, StatType : {statType}, CurrentValue : {statPercentages[statType]}, ChangeValue : {changeValue}");
            var diff = changeValue - statPercentages[statType];
            statPercentages[statType] = changeValue;
            
            if (statType == StatType.Hp)
            {
                HealHpPercentage(diff);
            }
        }
        
        public int GetUpgradeCount(StatType statType) => upgrades[statType];
        
        #endregion

        #region Battle

        private void InitSkills()
        {
            Debug.Log($"Unit.InitSkills(), Unit : {this.name}");
            skillGroup.Init(this);
        }

        protected void ClearBuffSkill()
        {
            foreach (var buffSkill in buffSkills)
            {
                buffSkill.endCallback?.Invoke(this);
                StopCoroutine(buffSkill.coroutine);
            }

            buffSkills.Clear();
        }

        public void AddBuffSkill(string name, Action<Unit> action, Action<Unit> endCallback, float endTime)
        {
            Debug.Log($"Unit.AddBuffSkill(), Start, Unit : {this.transform.name}, Buff : {name}");
            var buffSkill = buffSkills.Find(buffSkill => buffSkill.action.Equals(action));
            if (buffSkill != null)
            {
                Debug.Log($"Unit.AddBuffSkill(), already is Added, Unit : {this.transform.name}, Buff : {name}");
                RemoveBuffSkill(buffSkill);
            }

            buffSkill = new TimerInfo<Unit>(name, action, endCallback, endTime);
            buffSkills.Add(buffSkill);
            buffSkill.coroutine = StartCoroutine(InvokeSkill(buffSkill));
        }

        private void RemoveBuffSkill(TimerInfo<Unit> buffSkill)
        {
            buffSkill.endCallback?.Invoke(this);
            StopCoroutine(buffSkill.coroutine);
            buffSkills.Remove(buffSkill);
        }

        private IEnumerator InvokeSkill(TimerInfo<Unit> buffSkill)
        {
            Debug.Log($"Unit.InvokeSkill Pre, Unit : {name}, Skill : {buffSkill.name}");

            buffSkill.action?.Invoke(this);

            yield return new WaitForSeconds(buffSkill.RemainTime);

            Debug.Log($"Unit.InvokeSkill Post, Unit : {name}, Skill : {buffSkill.name}");

            RemoveBuffSkill(buffSkill);
        }

        public void UseExtraSkill()
        {
            if (skillGroup.ExtraSkill == null)
            {
                Debug.Log($"Unit.UseExtraSkill() ExtraSkill is null, Unit : {name}");
                return;
            }

            if (skillGroup.ExtraSkill.IsUse)
            {
                Point targetPoint;
                
                if (SkillGroup.ExtraSkill.SkillAttackType == SkillAttackType.Attack)
                {
                    targetPoint = D.SelfBoard.FindEnemyPoint(BasePoint, int.MaxValue, false, this);
                }
                else
                {
                    targetPoint = BasePoint;
                }
               
                if (targetPoint == null)
                {
                    return;
                }
                
                skillGroup.ExtraSkill.Use(targetPoint);
            }
        }

        public void SetCastingSkill(float castingTIme, Action castingCallback)
        {
            castingEndTime = Time.time + castingTIme;
            this.castingCallback += castingCallback;

            onStartCasting?.Invoke(castingEndTime);
        }

        private void ExecuteCastingSkill()
        {
            castingCallback?.Invoke();

            castingEndTime = 0;
            castingCallback = null;
        }

        private void CancelCastingSkill()
        {
            castingEndTime = 0;
            castingCallback = null;
        }

        public void Idle()
        {
            IdleAnim();
        }
        protected virtual void IdleAnim() { }

        public void Attack()
        {
            if (skillGroup == null)
            {
                return;
            }
            
            if (IsUsingSkill)
            {
                return;
            }
            
            var skills = skillGroup.GetRandomAllSkills();

            if (skills != null)
            {
                foreach (var skill in skills)
                {
                    if (skill.IsUse)
                    {
                        Point targetPoint;
                        
                        if (skill.SkillAttackType == SkillAttackType.Attack
                            || skill.SkillAttackType == SkillAttackType.Heal)
                        {
                            targetPoint = BattlePoint;
                        }
                        else
                        {
                            targetPoint = BasePoint;
                        }
                        
                        skill.Use(targetPoint);
                        _Attack();
                        AttackAnim();
                        break;
                    }
                }
            }
        }

        protected virtual void _Attack() { }

        protected virtual void AttackAnim() { }

        public void Kill()
        {
            Debug.Log($"Unit.Kill(), Unit : {this.name}");
            Hp = -1f;
        }

        public void Eaten()
        {
            Debug.Log($"Unit.Eaten(), Unit : {this.name}");
            
            if (ownerType == OwnerType.My)
            {
                BasePoint?.UnSetMyMob(this);
            }
            else
            {
                BasePoint?.UnSetEnemyMob(this);
            }

            basePoint = null;
            TargetPoint = null;
            targetPoints.Clear();
            StopAllCoroutines();
        }
        
        public virtual void Hit(DamageInfo damageInfo, Point attackerPoint = null, Action<float> action = null)
        {
            if (IsInvincible)
            {
                Debug.Log($"Unit.Hit return Because IsInvincible, Unit : {this.name}");
                return;
            }
            
            damageInfo.Calc();

            var reduceByAttackType = GetReduceByAttackType(damageInfo);

            float trueDamage = damageInfo.Damage * (1 - ReductionByDefenseRate) * (1 - reduceByAttackType);

            var popupType = damageInfo.IsCritical ? PopupType.CriticalDamage : PopupType.Damage;
            PopupManager.Instance.OpenPopup(popupType, transform.position, trueDamage);

            HitTrueDamage(trueDamage, attackerPoint, action);
        }

        private float GetReduceByAttackType(DamageInfo damageInfo)
        {
            float reducePercentage = 0f;

            switch (damageInfo.AttackType)
            {
                case AttackType.Melee:
                    reducePercentage = ReductionMeleeAttack * 0.01f;
                    break;
                case AttackType.Ranged:
                    reducePercentage = ReductionRangedAttack * 0.01f;
                    break;
            }

            return reducePercentage;
        }

        protected virtual void HitTrueDamage(float damage, Point attackerPoint = null, Action<float> action = null)
        {
            Debug.Log($"Unit.HitTrueDamage, Name : {name}, Hp : {Hp}, Damage : {damage}" +
                $", Defense : {Defense}, ReductionByDefenseRate : {ReductionByDefenseRate}");

            Hp -= damage;
            action?.Invoke(damage);
            HitAnim();

            if(attackerPoint != null)
            {
                ChangeTargetToAttacker(attackerPoint);
            }
        }
        
        protected virtual void HitAnim() { }
        protected virtual void ChangeTargetToAttacker(Point attackerPoint) { }

        public virtual void Stun(float time)
        {
            Debug.Log($"Unit.Stun, Name : {name}, Time : {time}");
            
            if (IsInvincible)
            {
                Debug.Log($"Unit.Stun return Because IsInvincible, Unit : {this.name}");
                return;
            }

            endStunTime = Time.time + time;
            StunAnim();
        }
        protected virtual void StunAnim() { }

        public virtual void Knockback()
        {
            Debug.Log($"Unit.Knockback, Name : {name}");
            
            if (IsInvincible)
            {
                Debug.Log($"Unit.Knockback return Because IsInvincible, Unit : {this.name}");
                return;
            }
            
            KnockbackAnim();
        }
        protected virtual void KnockbackAnim() { }

        public virtual void Death()
        {
            if (IsDeath) return;

            Debug.Log($"Unit.Death, Name : {name}, Hp : {Hp}");

            isDeath = true;
            SkillGroup.PassiveSkill?.Clear();

            onDeath.Invoke(this);
            DeathAnim();
            
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DeathAsync());
            }
        }

        protected virtual void DeathAnim()
        {
        }

        private IEnumerator DeathAsync()
        {
            var disslove = Disslove();

            while (disslove.MoveNext())
            {
                yield return disslove.Current;
            }

            yield return new WaitForSeconds(DeathDelay);

            _PostDeath();
        }

        protected abstract void _PostDeath();
        
        private IEnumerator Disslove()
        {
            Debug.Log($"Unit.Disslove() Start, Name : {name}");

            while (unitAnimator != null && unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

            Debug.Log($"Unit.Disslove() Anim End, Name : {name}");

            if (deathMesh != null)
            {
                renderMesh.gameObject.SetActive(false);
                deathMesh?.gameObject.SetActive(true);
            }

            float cutoff = 0;
            float speed = 0.4f;

            while (cutoff < 1f)
            {
                deathMeshRenderer.ForEach(renderer => Disslove(renderer, cutoff));
                cutoff += Time.deltaTime * speed;
                yield return null;
            }

            Debug.Log($"Unit.Disslove() Disslove End, Name : {name}");
        }

        private void Disslove(Renderer renderer, float value)
        {
            var mats = renderer.materials;
            foreach (var mat in mats)
            {
                mat.SetFloat(cutoff, value);
            }
            renderer.materials = mats;
        }

        public void FullHealHpWithoutEvent()
        {
            Debug.Log($"Unit.FullHealHpWithoutEvent, Name : {name}, Hp : {Hp}");

            Hp = MaxHp;
        }
        
        public void FullHealHp()
        {
            Debug.Log($"Unit.FullHealHp, Name : {name}, Hp : {Hp}");

            Hp = MaxHp;
        }

        public virtual void HealHp(DamageInfo healInfo)
        {
            healInfo.Calc();
            HealHp(healInfo.Damage);
        }
        public void HealHp(float healPoint, bool isOpenPopUp = true)
        {
            Debug.Log($"Unit.HealHp, Name : {name}, Hp : {Hp}, HealPoint : {healPoint}");

            if (isOpenPopUp)
            {
                PopupManager.Instance.OpenPopup(PopupType.Heal, transform.position, healPoint);
            }

            Hp += healPoint;
        }

        public void HealHpPercentage(float percentage)
        {
            Debug.Log($"Unit.HealHpPercentage, Name : {name}, Hp : {Hp}, HealPercentage : {percentage}");

            var healPoint = MaxHp * percentage * 0.01f;

            Hp += healPoint;
        }

        public virtual void DrainHeal(float actualDamage)
        {
            Debug.Log($"Unit.DrainHeal, Name : {name}, Hp : {Hp}, drainPercentage : {DrainProbability}");

            HealHp(actualDamage * DrainProbability * 0.01f);
        }

        #endregion

        #region Move

        public virtual void Spawn(Point spawnPoint)
        {
            Debug.Log($"{this.gameObject.name} virtual Spawn In Point {spawnPoint}");

            Hp = MaxHp;
            basePoint = spawnPoint;

            isSpawn = true;
            IsUsingSkill = false;
            transform.position = spawnPoint.Position;
            gameObject.SetActive(true);

            onSpawn.Invoke(this);
            SkillGroup.PassiveSkill?.Use(BasePoint);

            if (deathMesh != null)
            {
                renderMesh.gameObject.SetActive(true);
                deathMesh?.gameObject.SetActive(false);
            }

            deathMeshRenderer.ForEach(meshRenderer => Disslove(meshRenderer, 0));
        }
        protected virtual void SpawnAnim() { }

        protected virtual void PreSetTargetPoints(Queue<Point> targets) { }
        public void SetTargetPoints(Queue<Point> targets)
        {
            if (targets.Count == 0)
                return;

            PreSetTargetPoints(targets);
            targetPoints = targets;
            SetNextPoint();
        }

        public void SetDestinationPoint(Point destinationPoint)
        {
            var paths = GetPathsFunc()(BasePoint, destinationPoint).ToQueue();
            SetTargetPoints(paths);
        }
        protected abstract Func<Point, Point, List<Point>> GetPathsFunc();
        
        protected abstract Point SearchEnemyPoint();

        protected abstract Point SearchAllyPoint();
        
        protected abstract Point SearchNotFullHpAllyUnitPoint();

        public virtual void MoveToNearestBattleUnit()
        {
            Move(NearestUnit.transform.position);

            if (IsInnerTargetPos(BasePoint.Position, transform.position) == false)
            {
                var point = D.SelfBoard.FindPoint(transform.position);
                if (point is not null)
                {
                    if (ownerType == OwnerType.My)
                    {
                        basePoint.UnSetMyMob(this);
                        point.SetMyMob(this);
                    }
                    else
                    {
                        basePoint.UnSetEnemyMob(this);
                        point.SetEnemyMob(this);
                    }
                    
                    basePoint = point;
                }

                var destinationPoint = DestinationPoint;
                if (destinationPoint != null)
                {
                    SetDestinationPoint(destinationPoint);
                }
                
            }
        }

        public void MoveToTarget()
        {
            if (pathCreator is not null)
                MoveSpline();
            else
                Move(TargetPos);

            if (IsReachPoint())
                ReachPoint();
        }

        private void Move(Vector3 targetPos)
        {
            if (IsLookTargetPos(targetPos) == false)
            {
                Walk(targetPos);
            }
            else
            {
                Run(targetPos);
            }
        }

        public bool IsLookTargetPos(Vector3 targetPos) => Vector3.Dot((targetPos - transform.position).normalized, transform.forward) > 0.5f;
        public float GetDotToTargetPos(Vector3 targetPos) => Vector3.Dot((targetPos - transform.position).normalized, transform.forward);
        public Vector3 GetCrossToTargetPos(Vector3 targetPos) => Vector3.Cross((targetPos - transform.position).normalized, transform.forward);

        private void Walk(Vector3 targetPos)
        {
            WalkAnim(IsLeftToTarget, IsBackToTarget, IsSideToTarget);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime * 0.3f);
            onMoved.Invoke(transform.position);
        }

        private void Run(Vector3 targetPos)
        {
            if (Speed < 4)
            {
                WalkAnim(IsLeftToTarget, IsBackToTarget, IsSideToTarget);
            }
            else
            {
                RunAnim(IsLeftToTarget, IsBackToTarget, IsSideToTarget);
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
            onMoved.Invoke(transform.position);
        }

        private void MoveSpline()
        {
            if (IsSplineMove == false)
            {
                MoveSplineStartPos();
            }

            if (IsSplineMove)
            {
                _MoveSpline();
            }
        }

        private void MoveSplineStartPos()
        {
            var destination = pathCreator.path.GetPoint(0);
            var destDirection = pathCreator.path.GetPoint(1);

            bool isMove = Vector3.Distance(destination, transform.position) > 0.3;
            bool isRotate = Vector3.Dot(transform.forward, (destDirection - transform.position).normalized) < 0.8f;

            if (isMove)
            {
                Move(destination);
            }

            if (isRotate)
            {
                Rotate(destDirection);
            }

            if (!isRotate && !isMove)
            {
                isSplineMove = true;
            }
        }

        private void _MoveSpline()
        {
            distanceTravelled += Speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

            var destination = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
            if (Vector3.Distance(transform.position,destination) < 0.1)
            {
                ResetPathData();
            }
            
            if (Speed < 4)
            {
                WalkAnim(IsLeftToTarget, IsBackToTarget, IsSideToTarget);
            }
            else
            {
                RunAnim(IsLeftToTarget, IsBackToTarget, IsSideToTarget);
            }
            
            onMoved.Invoke(transform.position);
            onRotated.Invoke(transform.rotation);
        }

        private void ResetPathData()
        {
            pathCreator = null;
            distanceTravelled = 0;
            isSplineMove = false;
        }

        protected virtual void WalkAnim(bool isLeft, bool isBack, bool isSide) { }
        protected virtual void RunAnim(bool isLeft, bool isBack, bool isSide) { }

        public void RotateXZ(Vector3 targetPos)
        {
            var targetPosXZ = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            Rotate(targetPosXZ);
        }
        
        public void Rotate(Vector3 targetPos)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), Time.deltaTime * Speed * 0.6f);
            onRotated.Invoke(transform.rotation);
        }

        public void RotateImmediately(Vector3 targetPos)
        {
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
            onRotated.Invoke(transform.rotation);
        }

        private bool IsReachPoint() => IsInnerTargetPos(TargetPos, transform.position);
        private void ReachPoint()
        {
            if (ownerType == OwnerType.My)
            {
                basePoint.UnSetMyMob(this);
                targetPoint.SetMyMob(this);
            }
            else
            {
                basePoint.UnSetEnemyMob(this);
                targetPoint.SetEnemyMob(this);
            }

            SetBasePoint();
            SetNextPoint();
        }

        private void SetBasePoint()
        {
            basePoint = targetPoint;
            pathCreator = null;
        }

        private void SetNextPoint()
        {
            if (targetPoints.Count > 0)
            {
                targetPoint = targetPoints.Dequeue();
                pathCreator = basePoint.GetTargetPointPathCreator(targetPoint);

                targetPos = targetPoint.RandomPos;
                
                Debug.Log($"Unit.SetNextPoint(), Unit : {name}, targetPoint : {targetPoint.name}");
            }
            else
            {
                ArrivalDestination();
            }
        }

        public void ChangeBasePoint(Point point)
        {
            Debug.Log($"Unit.ChangeBasePoint(), Unit : {name}, Point : {point.name}");
            
            if (ownerType == OwnerType.My)
            {
                BasePoint?.UnSetMyMob(this);
                point.SetMyMob(this);
            }
            else
            {
                BasePoint?.UnSetEnemyMob(this);
                point.SetEnemyMob(this);
            }

            basePoint = point;
            TargetPoint = null;
            targetPoints.Clear();
            transform.position = point.RandomPos;
        }
        
        protected virtual void ArrivalDestination()
        {
            TargetPoint = null;
        }

        private bool IsInnerTargetPos(Vector3 targetPos, Vector3 position)
        {
            float offset = Point.POINT_SCALE * 0.1f;

            float minX = targetPos.x - offset;
            float maxX = targetPos.x + offset;

            float minZ = targetPos.z - offset;
            float maxZ = targetPos.z + offset;

            float diffY = (targetPos.y - position.y) * (targetPos.y - position.y);

            if (IsInnerCoordinate(position.x, minX, maxX) && IsInnerCoordinate(position.z, minZ, maxZ) && diffY < 1)
                return true;

            return false;
        }

        private bool IsInnerCoordinate(float x, float minX, float maxX)
        {
            if (minX < x && x < maxX)
                return true;

            return false;
        }

        #endregion
    }
}
