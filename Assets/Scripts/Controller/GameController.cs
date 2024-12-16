using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private BattleSystem battleController;
    [SerializeField]
    private Camera worldCamera;

    private GameState gameState;
    private bool isBattleActive = false;

    private void Awake()
    {
        ConditionsDB.Init();
    }

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned in GameController");
            return;
        }

        if (battleController == null)
        {
            Debug.LogError("BattleSystem is not assigned in GameController");
            return;
        }

        if (worldCamera == null)
        {
            Debug.LogError("WorldCamera is not assigned in GameController");
            return;
        }

        playerController.OnEncountered += StartBattle;
        battleController.OnBattleOver += EndBattle;
    }

    private void StartBattle()
    {
        if (isBattleActive) return; // Prevent duplicate battles
        isBattleActive = true;

        Debug.Log("Transitioning to battle state...");
        gameState = GameState.Battle;
        battleController.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        if (playerParty == null)
        {
            Debug.LogError("PlayerParty is not found on PlayerController");
            EndBattle(false);
            return;
        }

        var wildpokemon = FindFirstObjectByType<MapArea>().GetRandomWildPokemon();
        if (wildpokemon == null)
        {
            Debug.LogError("No wild Pokemon found");
            EndBattle(false);
            return;
        }

        if (playerParty.GetNotFaintedPokemon() == null)
        {
            Debug.LogError("No usable Pokémon! Returning to Pokémon Center...");
            EndBattle(false);
            return;
        }

        battleController.StartBattle(playerParty, wildpokemon);
    }

    private void EndBattle(bool isWin)
    {
        isBattleActive = false;
        gameState = GameState.FreeRoam;
        battleController.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (gameState == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (gameState == GameState.Battle)
        {
            battleController.HandleUpdate();
        }
    }
}
