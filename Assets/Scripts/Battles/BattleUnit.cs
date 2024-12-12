using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    public void Setup()
    {
        Pokemon = new Pokemon(_base, level);

        if (isPlayerUnit)
        {
            int random = Random.Range(1, 521);
            StartCoroutine(LoadSpriteFromURL($"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/{random}.png"));
        }
        else
        {
            int random = Random.Range(1, 521);
            StartCoroutine(LoadSpriteFromURL($"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{random}.png"));
        }
    }

    private IEnumerator LoadSpriteFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                GetComponent<Image>().sprite = sprite;
            }
            else
            {
                Debug.LogError($"Failed to load sprite from URL: {url}. Error: {request.error}");
            }
        }
    }

}
