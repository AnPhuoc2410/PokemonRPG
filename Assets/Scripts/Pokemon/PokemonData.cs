using System.Collections.Generic;
using SimpleJSON;

[System.Serializable]
public class PokemonData
{
    public string name;
    public List<Stat> stats;
    public List<Type> types;
    public Sprites sprites;

    public static PokemonData FromJson(JSONNode json)
    {
        var pokemonData = new PokemonData
        {
            name = json["name"],

            stats = new List<Stat>(),
            types = new List<Type>(),
            sprites = new Sprites
            {
                front_default = json["sprites"]["front_default"].Value,
                back_default = json["sprites"]["back_default"].Value
            }
        };

        // Parse stats
        foreach (JSONNode stat in json["stats"].AsArray)
        {
            pokemonData.stats.Add(new Stat
            {
                base_stat = stat["base_stat"].AsInt,
                stat = new StatInfo
                {
                    name = stat["stat"]["name"].Value
                }
            });
        }

        // Parse types
        foreach (JSONNode type in json["types"].AsArray)
        {
            pokemonData.types.Add(new Type
            {
                type = new TypeInfo
                {
                    name = type["type"]["name"].Value
                }
            });
        }

        return pokemonData;
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
    public class Type
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
}
