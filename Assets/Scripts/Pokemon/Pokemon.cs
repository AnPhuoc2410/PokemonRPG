using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] private PokemonBase _base;
    [SerializeField] private int _level;

    public int HP { get; private set; }
    public int MaxHP { get; private set; }
    public bool isHpChanged { get; set; } = false;

    public event System.Action OnStatusChanged;
    public List<Move> Moves { get; private set; }
    public Move CurrentMove { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public IV IndividualValues { get; private set; }
    public EV EffortValues { get; private set; }
    public Condition Status { get; private set; }
    public Condition VolatileStatus { get; private set; }
    public int VolatieStatusTime { get; set; }
    public int StatusTime { get; set; }
    public PokemonBase Base => _base;
    public int Level => _level;

    public int Attack => GetStat(Stat.Attack);
    public int Defense => GetStat(Stat.Defense);
    public int SpAttack => GetStat(Stat.SpAttack);
    public int SpDefense => GetStat(Stat.SpDefense);
    public int Speed => GetStat(Stat.Speed);
    public int Accuracy => GetStat(Stat.Accuracy);

    public void Init()
    {
        Moves = GenerateMoves();
        IndividualValues = new IV();
        IndividualValues.Initialize();
        EffortValues = new EV();
        StatsInit();
        HP = MaxHP;
        StatBootsInit();
        Status = null;
        VolatileStatus = null;
    }

    private List<Move> GenerateMoves()
    {
        var moves = new List<Move>();
        foreach (var move in _base.LearnableMoves)
        {
            if (move.Level <= _level)
            {
                moves.Add(new Move(move.Base));
            }
            if (moves.Count >= 4) break;
        }
        return moves;
    }
    private void StatBootsInit()
    {
        StatBoosts = new Dictionary<Stat, int>
        {
            { Stat.Attack, 0 },
            { Stat.Defense, 0 },
            { Stat.SpAttack, 0 },
            { Stat.SpDefense, 0 },
            { Stat.Speed, 0 },
            { Stat.Accuracy, 0 },
            { Stat.Evasion, 0 }
        };
    }

    private void StatsInit()
    {
        Stats = new Dictionary<Stat, int>
        {
            { Stat.Attack, CalculateStat(_base.Attack, IndividualValues.Attack, EffortValues.Attack, _level) },
            { Stat.Defense, CalculateStat(_base.Defense, IndividualValues.Defense, EffortValues.Defense, _level) },
            { Stat.SpAttack, CalculateStat(_base.SpAttack, IndividualValues.SpAttack, EffortValues.SpAttack, _level) },
            { Stat.SpDefense, CalculateStat(_base.SpDefense, IndividualValues.SpDefense, EffortValues.SpDefense, _level) },
            { Stat.Speed, CalculateStat(_base.Speed, IndividualValues.Speed, EffortValues.Speed, _level) },
            { Stat.HP, CalculateHPStat(_base.MaxHP, IndividualValues.HP, EffortValues.HP, _level) },
        };
        MaxHP = Stats[Stat.HP];
    }

    private int CalculateStat(int baseStat, int iv, int ev, int level)
    {
        return Mathf.FloorToInt(((2 * baseStat + iv + Mathf.FloorToInt(ev / 4)) * level / 100f) + 5);
    }

    private int CalculateHPStat(int baseStat, int iv, int ev, int level)
    {
        return Mathf.FloorToInt(((2 * baseStat + iv + Mathf.FloorToInt(ev / 4)) * level / 100f) + level + 10);
    }

    private int GetStat(Stat stat)
    {
        int statVal = Stats.ContainsKey(stat) ? Stats[stat] : 0;
        int boosts = StatBoosts.ContainsKey(stat) ? StatBoosts[stat] : 0;
        var boostTable = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 5f };

        if (boosts >= 0)
            statVal = Mathf.FloorToInt(statVal * boostTable[boosts]);
        else
            statVal = Mathf.FloorToInt(statVal / boostTable[-boosts]);
        return statVal;
    }
    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if (boost > 0)
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            else
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");

            Debug.Log($"The pokemon stat {stat} is {StatBoosts[stat]}");

        }
    }
    public void SetStatus(ConditionID conditionID)
    {
        if (Status != null) return;
        Status = ConditionsDB.Conditions[conditionID];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {Status.Message}");
        OnStatusChanged?.Invoke();
    }

    public DamageDetail TakeDamage(Move move, Pokemon attacker)
    {
        float critical = Random.value <= 0.0625f ? 1.5f : 1f;
        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, Base.Type1) *
                                  TypeChart.GetEffectiveness(move.Base.Type, Base.Type2);

        DamageDetail detail = new()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };

        if (move.Base.Power == 0)
            return detail; // Status move, no damage.

        float attack = move.Base.Category == MoveCategory.Special ? attacker.SpAttack : attacker.Attack;
        float defense = move.Base.Category == MoveCategory.Special ? SpDefense : Defense;

        float modifier = critical * typeEffectiveness * Random.Range(0.85f, 1f);
        int damage = Mathf.FloorToInt(((2 * attacker.Level / 5f + 2) * move.Base.Power * (attack / defense) / 50 + 2) * modifier);

        UpdateHP(damage);
        return detail;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        isHpChanged = true;
    }
    public Move GetRandomMove()
    {
        var movesWithPP = Moves.FindAll(m => m.PP > 0).ToList();
        int r = Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }
    public bool OnBeforeTurn()
    {
        bool canPerformMove = true;
        if (Status?.OnBeforeTurn != null)
        {
            if (!Status.OnBeforeTurn(this)) canPerformMove = false;
        }
        if (VolatileStatus?.OnBeforeTurn != null)
        {
            if (!VolatileStatus.OnBeforeTurn(this)) canPerformMove = false;
        }
        return canPerformMove;
    }
    public void OnBattleOver()
    {
        VolatileStatus = null;
        StatusChanges.Clear();
        StatBootsInit();
    }
    public void SetVolatileStatus(ConditionID conditionID)
    {
        if (VolatileStatus != null) return;
        VolatileStatus = ConditionsDB.Conditions[conditionID];
        VolatileStatus?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {VolatileStatus.Message}");
    }

    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }
}
