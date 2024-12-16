using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            Condition condition = kvp.Value;
            condition.ID = kvp.Key;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
{
    {
        ConditionID.psn, new Condition
        {
            Name = "Poison",
            Description = "The Pokémon is poisoned and takes damage over time.",
            Message = "was poisoned!",
            OnAfterTurn = (Pokemon pokemon) =>
            {
                int damage = Mathf.Max(pokemon.MaxHP / 8, 1);
                pokemon.UpdateHP(damage);
                pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} was hurt by poison.");
            }
        }
    },
    {
        ConditionID.brn, new Condition
        {
            Name = "Burn",
            Description = "The Pokémon is burned and takes damage over time.",
            Message = "was burned!",
            OnAfterTurn = (Pokemon pokemon) =>
            {
                int damage = Mathf.Max(pokemon.MaxHP / 16, 1);
                pokemon.UpdateHP(damage);
                pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} was hurt by its burn.");
            }
        }
    },
    {
        ConditionID.par, new Condition
        {
            Name = "Paralysis",
            Description = "The Pokémon is paralyzed and may not move.",
            Message = "was paralyzed!",
            OnBeforeTurn = (Pokemon pokemon) =>
            {
                if (Random.Range(1, 5) == 1) // 25% chance
                {
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fully paralyzed and cannot move.");
                    return false;
                }
                return true;
            }
        }
    },
    {
        ConditionID.frz, new Condition
        {
            Name = "Freeze",
            Description = "The Pokémon is frozen and may thaw out over time.",
            Message = "was frozen solid!",
            OnBeforeTurn = (Pokemon pokemon) =>
            {
                if (Random.Range(1, 5) == 1) // 25% chance
                {
                    pokemon.CureStatus();
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} thawed out!");
                    return true;
                }
                return false;
            }
        }
    },
    {
        ConditionID.slp, new Condition
        {
            Name = "Sleep",
            Description = "The Pokémon is asleep and cannot move for a few turns.",
            Message = "fell asleep!",
            OnStart = (Pokemon pokemon) =>
            {
                // Sleep condition lasts for 1-3 turns
                pokemon.StatusTime = Random.Range(1, 4);
                Debug.Log($"Sleep Time: {pokemon.StatusTime}");
            },
            OnBeforeTurn = (Pokemon pokemon) =>
            {
                if (pokemon.StatusTime <= 0)
                {
                    pokemon.CureStatus();
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up!");
                    return true;
                }
                pokemon.StatusTime--;
                pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fast asleep.");
                return false;
            }
        }
    }
};

}
