using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private BattleSystem battleController;
    [SerializeField]
    private Camera worldCamera;

    GameState gameState;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleController.OnBattleOver += EndBattle;
    }

    private void StartBattle()
    {
        gameState = GameState.Battle;
        battleController.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        battleController.StartBattle();
    }
    private void EndBattle(bool isWin)
    {
        gameState = GameState.FreeRoam;
        battleController.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }


    private void Update()
    {
        if(gameState == GameState.FreeRoam)
        {
           playerController.HandleUpdate();
        }
        else if (gameState == GameState.Battle)
        {
            battleController.HandleUpdate();
        }
    }
}
