using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "PokemonClient", menuName = "Pokemons/Don't topuch this one a Pokemon")]
public class PokemonClient : ScriptableObject
{
    [SerializeField] private string pokemonName;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private PokemonType type1;
    [SerializeField] private PokemonType type2;
    [SerializeField] private int maxHP, attack, defense, spAttack, spDefense, speed;

    private const string PokeApiUrl = "https://pokeapi.co/api/v2/pokemon/";

    public string PokemonName => pokemonName;
    public Sprite FrontSprite => frontSprite;
    public Sprite BackSprite => backSprite;
    public PokemonType Type1 => type1;
    public PokemonType Type2 => type2;
    public int MaxHP => maxHP;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public void FetchPokemonData(string name)
    {
        pokemonName = name;
        CoroutineRunner.StartCoroutine(GetPokemonData(name));
    }

    private IEnumerator GetPokemonData(string name)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PokeApiUrl + name.ToLower()))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                var json = webRequest.downloadHandler.text;

                // Parse JSON using SimpleJSON
                JSONNode data = JSON.Parse(json);

                // Extract data
                maxHP = data["stats"][0]["base_stat"].AsInt;
                attack = data["stats"][1]["base_stat"].AsInt;
                defense = data["stats"][2]["base_stat"].AsInt;
                spAttack = data["stats"][3]["base_stat"].AsInt;
                spDefense = data["stats"][4]["base_stat"].AsInt;
                speed = data["stats"][5]["base_stat"].AsInt;

                type1 = ParsePokemonType(data["types"][0]["type"]["name"]);
                type2 = data["types"].Count > 1 ? ParsePokemonType(data["types"][1]["type"]["name"]) : PokemonType.None;

                // Fetch and set sprites
                CoroutineRunner.StartCoroutine(LoadSprite(data["sprites"]["front_default"], sprite => frontSprite = sprite));
                CoroutineRunner.StartCoroutine(LoadSprite(data["sprites"]["back_default"], sprite => backSprite = sprite));
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
                Debug.LogError("Error fetching sprite: " + webRequest.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(webRequest);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(sprite);
            }
        }
    }

    private PokemonType ParsePokemonType(string typeName)
    {
        return System.Enum.TryParse(typeName, true, out PokemonType result) ? result : PokemonType.None;
    }

    public static class CoroutineRunner
    {
        private class CoroutineHolder : MonoBehaviour { }

        private static CoroutineHolder _runner;

        public static void StartCoroutine(IEnumerator coroutine)
        {
            if (_runner == null)
            {
                var obj = new GameObject("CoroutineRunner");
                _runner = obj.AddComponent<CoroutineHolder>();
                Object.DontDestroyOnLoad(obj);
            }
            _runner.StartCoroutine(coroutine);
        }
    }
}
