using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] private List<Pokemon> pokemons;

    public void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    public Pokemon GetNotFaintedPokemon()
    {
       return pokemons.Find(p => p.HP > 0);
    }
}
