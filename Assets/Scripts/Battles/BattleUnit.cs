using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHub hud;
    public Pokemon Pokemon { get; set; }
    public bool IsPlayerUnit => isPlayerUnit;
    public BattleHub Hud => hud;

    private RectTransform rectTransform;
    private Image image;
    private Vector2 originPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        originPosition = rectTransform.anchoredPosition;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        // Reset to the original position
        rectTransform.anchoredPosition = originPosition;
        image.enabled = true; // Ensure the image is visible

        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
            //int random = Random.Range(1, 521);
            //StartCoroutine(LoadSpriteFromURL($"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/{random}.png"));
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
            //int random = Random.Range(1, 521);
            //StartCoroutine(LoadSpriteFromURL($"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{random}.png"));
        }
        hud.SetData(Pokemon);
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
        image.enabled = false; // Hide the image
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
    public IEnumerator PlayReturnAnimation()
    {
        float duration = 1.0f;
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(800, 0);

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

    private float EaseInOutQuad(float t)
    {
        if (t < 0.5f)
            return 2 * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }
}
