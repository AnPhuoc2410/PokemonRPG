using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.Posion, new Condition
            {
                Name = "Posion",
                Description = "This is a posion condition",
                Message = "has been posioned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    int damage = Mathf.Max(pokemon.MaxHP / 8, 1);
                    pokemon.UpdateHP(damage);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt due to poison");
                }
            }
        },
        {
            ConditionID.Burn, new Condition
            {
                Name = "Burn",
                Description = "This is a burn condition",
                Message = "has been burned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    int damage = Mathf.Max(pokemon.MaxHP / 16, 1);
                    pokemon.UpdateHP(damage);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt due to burn");
                }
            }
        },
        {
            ConditionID.Paralysis,
            new Condition
            {
                Name = "Paralysis",
                Description = "This is a paralysis condition",
                Message = "has been paralyzed",
                OnBeforeTurn = (Pokemon pokemon) =>
                {
                    if (Random.Range(1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fully paralyzed");
                        return false;
                    }
                    return true;
                }
            }
        },

    };

}
