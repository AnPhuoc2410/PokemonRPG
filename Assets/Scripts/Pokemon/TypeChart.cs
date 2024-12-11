using System.Collections.Generic;

public static class TypeChart
{
    // Define type effectiveness as a dictionary of dictionaries
    private static readonly Dictionary<PokemonType, Dictionary<PokemonType, float>> chart =
        new Dictionary<PokemonType, Dictionary<PokemonType, float>>()
        {
            { PokemonType.Normal, new Dictionary<PokemonType, float>
                {
                    { PokemonType.Rock, 0.5f },
                    { PokemonType.Ghost, 0f },
                    { PokemonType.Steel, 0.5f }
                }
            },
            { PokemonType.Fire, new Dictionary<PokemonType, float>
                {
                    { PokemonType.Grass, 2f },
                    { PokemonType.Ice, 2f },
                    { PokemonType.Bug, 2f },
                    { PokemonType.Steel, 2f },
                    { PokemonType.Fire, 0.5f },
                    { PokemonType.Water, 0.5f },
                    { PokemonType.Rock, 0.5f },
                    { PokemonType.Dragon, 0.5f }
                }
            },
            { PokemonType.Water, new Dictionary<PokemonType, float>
                {
                    { PokemonType.Fire, 2f },
                    { PokemonType.Ground, 2f },
                    { PokemonType.Rock, 2f },
                    { PokemonType.Water, 0.5f },
                    { PokemonType.Grass, 0.5f },
                    { PokemonType.Dragon, 0.5f }
                }
            },
            { PokemonType.Grass, new Dictionary<PokemonType, float>
                {
                    { PokemonType.Water, 2f },
                    { PokemonType.Ground, 2f },
                    { PokemonType.Rock, 2f },
                    { PokemonType.Fire, 0.5f },
                    { PokemonType.Grass, 0.5f },
                    { PokemonType.Poison, 0.5f },
                    { PokemonType.Flying, 0.5f },
                    { PokemonType.Bug, 0.5f },
                    { PokemonType.Dragon, 0.5f },
                    { PokemonType.Steel, 0.5f }
                }
            },
            { PokemonType.Electric, new Dictionary<PokemonType, float>
                {
                    { PokemonType.Water, 2f },
                    { PokemonType.Flying, 2f },
                    { PokemonType.Ground, 0f },
                    { PokemonType.Electric, 0.5f },
                    { PokemonType.Grass, 0.5f },
                    { PokemonType.Dragon, 0.5f }
                }
            },
            // Add more types as needed...
        };

    /// <summary>
    /// Gets the effectiveness of an attacking type against a defending type.
    /// </summary>
    /// <param name="attackType">The attacker's type.</param>
    /// <param name="defenseType">The defender's type.</param>
    /// <returns>A float representing the effectiveness multiplier.</returns>
    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        // If no specific interaction exists, the default is 1x damage
        if (!chart.ContainsKey(attackType) || !chart[attackType].ContainsKey(defenseType))
        {
            return 1f;
        }

        return chart[attackType][defenseType];
    }
}
