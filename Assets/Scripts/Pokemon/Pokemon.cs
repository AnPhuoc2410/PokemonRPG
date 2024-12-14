using System.Collections.Generic;
using UnityEngine;
using static PokemonData;

[System.Serializable]
public class Pokemon
{
    [SerializeField] private PokemonBase _base;
    [SerializeField] private int _level;

    public int HP { get; private set; }
    public int MaxHP { get; private set; }

    public List<Move> Moves { get; private set; }
    public Dictionary<Stat, int> Stats { get; private set; }

    public IV IndividualValues { get; private set; }
    public EV EffortValues { get; private set; }

    public PokemonBase Base => _base;
    public int Level => _level;

    public int Attack => GetStat(Stat.Attack);
    public int Defense => GetStat(Stat.Defense);
    public int SpAttack => GetStat(Stat.SpAttack);
    public int SpDefense => GetStat(Stat.SpDefense);
    public int Speed => GetStat(Stat.Speed);

    public void Init()
    {
        Moves = GenerateMoves();
        IndividualValues = new IV();
        IndividualValues.Initialize();
        EffortValues = new EV();
        StatsInit();
        HP = MaxHP;
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

    private void StatsInit()
    {
        Stats = new Dictionary<Stat, int>
        {
            { Stat.Attack, CalculateStat(_base.Attack, IndividualValues.Attack, EffortValues.Attack, _level) },
            { Stat.Defense, CalculateStat(_base.Defense, IndividualValues.Defense, EffortValues.Defense, _level) },
            { Stat.SpAttack, CalculateStat(_base.SpAttack, IndividualValues.SpAttack, EffortValues.SpAttack, _level) },
            { Stat.SpDefense, CalculateStat(_base.SpDefense, IndividualValues.SpDefense, EffortValues.SpDefense, _level) },
            { Stat.Speed, CalculateStat(_base.Speed, IndividualValues.Speed, EffortValues.Speed, _level) },
            { Stat.HP, CalculateHPStat(_base.MaxHP, IndividualValues.HP, EffortValues.HP, _level) }
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
        if (Stats.ContainsKey(stat))
        {
            return Stats[stat];
        }
        return 0;
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

        HP -= damage;
        HP = Mathf.Clamp(HP, 0, MaxHP);

        if (HP == 0) detail.Fainted = true;
        return detail;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
