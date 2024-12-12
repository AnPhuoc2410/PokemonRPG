using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

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

        StartCoroutine(PlayEntryAnimation());
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
                image.sprite = sprite;
            }
            else
            {
                Debug.LogError($"Failed to load sprite from URL: {url}. Error: {request.error}");
            }
        }
    }

    public IEnumerator PlayEntryAnimation()
    {
        float duration = 1.0f;
        Vector2 startPosition = rectTransform.anchoredPosition + new Vector2(500, 0);
        Vector2 endPosition = rectTransform.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = EaseInOutQuad(t);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
    }

    public IEnumerator PlayFaintAnimation()
    {
        float duration = 1.0f;
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition - new Vector2(0, 500);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = EaseInOutQuad(t);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
    }

    public IEnumerator PlayAttackAnimation()
    {
        float duration = 0.5f;
        Vector2 originalPosition = rectTransform.anchoredPosition;
        Vector2 attackPosition = originalPosition + (isPlayerUnit ? new Vector2(50, 0) : new Vector2(-50, 0)); //You can increase or decrease this for a stronger or subtler attack motion

        // Move forward
        float elapsed = 0f;
        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            t = EaseInOutQuad(t);
            rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, attackPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Move back
        elapsed = 0f;
        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            t = EaseInOutQuad(t);
            rectTransform.anchoredPosition = Vector2.Lerp(attackPosition, originalPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }

    public IEnumerator PlayHitAnimation()
    {
        float shakeIntensity = 5f; //Increase shakeIntensity for more dramatic shaking.
        float shakeDuration = 0.3f; //Adjust shakeDuration for a longer or shorter effect.
        Vector2 originalPosition = rectTransform.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xShake = Random.Range(-shakeIntensity, shakeIntensity);
            float yShake = Random.Range(-shakeIntensity, shakeIntensity);
            rectTransform.anchoredPosition = originalPosition + new Vector2(xShake, yShake);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;

        // Optional: Flash effect
        StartCoroutine(PlayFlashAnimation());
    }

    private IEnumerator PlayFlashAnimation()
    {
        float flashDuration = 0.3f;
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < flashDuration)
        {
            isVisible = !isVisible;
            image.enabled = isVisible; // Toggle visibility
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        image.enabled = true; // Ensure visibility is restored
    }

    private float EaseInOutQuad(float t)
    {
        if (t < 0.5f)
            return 2 * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }
}
