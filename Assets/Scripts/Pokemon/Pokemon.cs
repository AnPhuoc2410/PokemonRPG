using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    private PokemonBase _base;
    private int _level;

    public int HP { get; private set; }

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _base = pBase;
        _level = pLevel;
        HP = MaxHP;

        //Generate Moves
        Moves = new List<Move>();
        foreach (var move in _base.LearnableMoves)
        {
            if (move.Level <= _level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 4) break;

        }
    }

    public PokemonBase Base => _base;
    public int Level => _level;

    public int Attack => Mathf.FloorToInt(((_base.Attack * _level) / 100f) + 5);
    public int Defense => Mathf.FloorToInt(((_base.Defense * _level) / 100f) + 5);
    public int SpAttack => Mathf.FloorToInt(((_base.SpAttack * _level) / 100f) + 5);
    public int SpDefense => Mathf.FloorToInt(((_base.SpDefense * _level) / 100f) + 5);
    public int Speed => Mathf.FloorToInt(((_base.Speed * _level) / 100f) + 5);
    public int MaxHP =>  Mathf.FloorToInt(((_base.MaxHP * _level) / 100f) + 10);

    public DamageDetail TakeDamage(Move move, Pokemon attacker)
    {
        float critical = Random.value <= 0.0625f ? 1.5f : 1f; // 6.25% chance for a critical hit

        // Calculate type effectiveness
        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) *
                                  TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        DamageDetail detail = new DamageDetail()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };

        // Calculate random modifier
        float modifier = critical * typeEffectiveness * Random.Range(0.85f, 1f);

        // Calculate damage using the Attack stat of the attacker and Defense stat of the target
        float damage = (((2 * attacker.Level / 5f + 2) * move.Base.Power * (attacker.Attack / (float)Defense)) / 50 + 2) * modifier;

        // Apply damage to HP
        HP -= Mathf.FloorToInt(damage);
        HP = Mathf.Clamp(HP, 0, MaxHP); // Ensure HP stays between 0 and MaxHP

        if (HP == 0) detail.Fainted = true;
        return detail;
    }
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
