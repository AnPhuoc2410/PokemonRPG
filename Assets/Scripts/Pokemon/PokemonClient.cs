using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PokemonClient", menuName = "Pokemon/Create new Pokemon")]
public class PokemonClient : ScriptableObject
{
    [SerializeField] string pokemonName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    private const string PokeApiUrl = "https://pokeapi.co/api/v2/pokemon/";

    public void FetchPokemonData(string pokemonName)
    {
        this.pokemonName = pokemonName;
        CoroutineRunner.StartCoroutine(GetPokemonData(pokemonName));
    }

    private IEnumerator GetPokemonData(string pokemonName)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PokeApiUrl + pokemonName.ToLower()))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                var json = webRequest.downloadHandler.text;
                var pokemonData = JsonUtility.FromJson<PokemonData>(json);

                // Populate fields with fetched data
                maxHP = pokemonData.stats.Find(stat => stat.stat.name == "hp").base_stat;
                attack = pokemonData.stats.Find(stat => stat.stat.name == "attack").base_stat;
                defense = pokemonData.stats.Find(stat => stat.stat.name == "defense").base_stat;
                spAttack = pokemonData.stats.Find(stat => stat.stat.name == "special-attack").base_stat;
                spDefense = pokemonData.stats.Find(stat => stat.stat.name == "special-defense").base_stat;
                speed = pokemonData.stats.Find(stat => stat.stat.name == "speed").base_stat;

                type1 = (PokemonType)System.Enum.Parse(typeof(PokemonType), pokemonData.types[0].type.name, true);
                if (pokemonData.types.Count > 1)
                {
                    type2 = (PokemonType)System.Enum.Parse(typeof(PokemonType), pokemonData.types[1].type.name, true);
                }
                else
                {
                    type2 = PokemonType.None;
                }

                // Fetch and set sprites
                CoroutineRunner.StartCoroutine(LoadSprite(pokemonData.sprites.front_default, sprite => frontSprite = sprite));
                CoroutineRunner.StartCoroutine(LoadSprite(pokemonData.sprites.back_default, sprite => backSprite = sprite));
            }
        }
    }

    private IEnumerator LoadSprite(string url, System.Action<Sprite> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(webRequest);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(sprite);
            }
        }
    }
}

