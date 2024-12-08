using UnityEngine;
using UnityEngine.UI;

public class PokemonManager : MonoBehaviour
{
    [SerializeField] private PokemonClient pokemonClient;
    [SerializeField] private InputField pokemonNameInput;
    [SerializeField] private Button fetchButton;
    [SerializeField] private Text displayText;
    [SerializeField] private Image frontSpriteImage;

    private void Start()
    {
        // Add a listener to the button click event
        fetchButton.onClick.AddListener(OnFetchButtonClicked);
    }

    private void OnFetchButtonClicked()
    {
        string pokemonName = pokemonNameInput.text;
        if (!string.IsNullOrEmpty(pokemonName))
        {
            // Fetch the Pokémon data
            pokemonClient.FetchPokemonData(pokemonName);

            // Wait for coroutine to complete (you can improve this by showing loading text)
            Invoke(nameof(UpdateUI), 2f); // Add delay for fetch to complete
        }
        else
        {
            displayText.text = "Please enter a Pokémon name.";
        }
    }

    private void UpdateUI()
    {
        // Update the UI with Pokémon data
        displayText.text = $"Name: {pokemonClient.PokemonName}\n" +
                           $"HP: {pokemonClient.MaxHP}\n" +
                           $"Attack: {pokemonClient.Attack}\n" +
                           $"Defense: {pokemonClient.Defense}\n" +
                           $"Sp. Attack: {pokemonClient.SpAttack}\n" +
                           $"Sp. Defense: {pokemonClient.SpDefense}\n" +
                           $"Speed: {pokemonClient.Speed}\n" +
                           $"Type1: {pokemonClient.Type1}\n" +
                           $"Type2: {pokemonClient.Type2}";

        // Update the sprite image
        frontSpriteImage.sprite = pokemonClient.FrontSprite;
    }
}
