using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    private PokemonBase _base;
    private int _level;

    public int HP { get; private set; }

    public List<Move> Moves { get; set; }

    public PokemonBase Base => _base;
    public int Level => _level;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _base = pBase;
        _level = pLevel;
        HP = MaxHP();

        Moves = new List<Move>();
        foreach (var move in _base.LearnableMoves)
        {
            if(move.Level <= _level)
            {
                Moves.Add(new Move(move.Base));
                if (Moves.Count >= 4) break;
            }
        }
    }

    public int Attack()
    {
        return Mathf.FloorToInt(((_base.Attack * _level) / 100f) + 5);
    }
    public int Defense()
    {
        return Mathf.FloorToInt(((_base.Defense * _level) / 100f) + 5);
    }
    public int SpAttack()
    {
        return Mathf.FloorToInt(((_base.SpAttack * _level) / 100f) + 5);
    }
    public int SpDefense()
    {
        return Mathf.FloorToInt(((_base.SpDefense * _level) / 100f) + 5);
    }
    public int Speed()
    {
        return Mathf.FloorToInt(((_base.Speed * _level) / 100f) + 5);
    }
    public int MaxHP()
    {
        return Mathf.FloorToInt(((_base.MaxHP * _level) / 100f) + 10);
    }
}
