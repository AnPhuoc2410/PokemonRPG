using UnityEngine;
using System.Collections.Generic;

public static class TypeColor
{
    public static readonly Dictionary<PokemonType, Color> TypeColorMap = new Dictionary<PokemonType, Color>
    {
        { PokemonType.None, Color.gray },
        { PokemonType.Normal, new Color(0.658f, 0.658f, 0.470f) }, // Light Brown
        { PokemonType.Fire, new Color(1.0f, 0.506f, 0.0f) }, // Orange
        { PokemonType.Water, new Color(0.0f, 0.439f, 1.0f) }, // Blue
        { PokemonType.Electric, new Color(1.0f, 0.843f, 0.0f) }, // Yellow
        { PokemonType.Grass, new Color(0.470f, 0.784f, 0.313f) }, // Green
        { PokemonType.Ice, new Color(0.313f, 0.929f, 0.925f) }, // Light Cyan
        { PokemonType.Fighting, new Color(0.784f, 0.251f, 0.188f) }, // Red
        { PokemonType.Poison, new Color(0.635f, 0.251f, 0.635f) }, // Purple
        { PokemonType.Ground, new Color(0.784f, 0.470f, 0.188f) }, // Brown
        { PokemonType.Flying, new Color(0.658f, 0.564f, 1.0f) }, // Light Purple
        { PokemonType.Psychic, new Color(1.0f, 0.313f, 0.470f) }, // Pink
        { PokemonType.Bug, new Color(0.658f, 0.784f, 0.188f) }, // Greenish Yellow
        { PokemonType.Rock, new Color(0.658f, 0.564f, 0.313f) }, // Brownish
        { PokemonType.Ghost, new Color(0.439f, 0.313f, 0.658f) }, // Dark Purple
        { PokemonType.Dragon, new Color(0.439f, 0.188f, 1.0f) }, // Purple
        { PokemonType.Dark, new Color(0.439f, 0.313f, 0.251f) }, // Dark Brown
        { PokemonType.Steel, new Color(0.658f, 0.658f, 0.784f) }, // Light Gray
        { PokemonType.Fairy, new Color(0.929f, 0.564f, 0.925f) } // Light Pink
    };
}
