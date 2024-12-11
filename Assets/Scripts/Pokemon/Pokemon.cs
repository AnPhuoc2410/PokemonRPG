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

    public bool TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100 <= 6.25f)
            critical = 1.5f;

        // Calculate type effectiveness
        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) *
                                  TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        // Calculate random modifier
        float modifier = critical * typeEffectiveness * Random.Range(0.85f, 1f);

        // Determine attack stat based on move category
        float attackStat = (move.Base.Category == MoveBase.MoveCategory.Physical) ? attacker.Attack : attacker.SpAttack;

        // Determine defense stat based on move category
        float defenseStat = (move.Base.Category == MoveBase.MoveCategory.Physical) ? this.Defense : this.SpDefense;

        // Calculate damage
        float damage = (((2 * attacker.Level / 5f + 2) * move.Base.Power * (attackStat / defenseStat)) / 50 + 2) * modifier;

        // Apply damage and return if the Pokémon fainted
        HP -= Mathf.FloorToInt(damage);
        HP = Mathf.Clamp(HP, 0, MaxHP); // Ensure HP stays between 0 and MaxHP

        return HP == 0; // Returns true if the Pokémon fainted
    }

}
