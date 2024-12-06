using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PokemonData
{
    public List<Stat> stats;
    public List<TypeElement> types;
    public Sprites sprites;
}

[System.Serializable]
public class Stat
{
    public int base_stat;
    public StatInfo stat;
}

[System.Serializable]
public class StatInfo
{
    public string name;
}

[System.Serializable]
public class TypeElement
{
    public TypeInfo type;
}

[System.Serializable]
public class TypeInfo
{
    public string name;
}

[System.Serializable]
public class Sprites
{
    public string front_default;
    public string back_default;
}
