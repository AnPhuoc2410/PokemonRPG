using Assets.Scripts.Battles;
using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHub playerHub;
    [SerializeField] BattleHub enemyHub;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        //Setup Battle
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHub.SetData(playerUnit.Pokemon);
        enemyHub.SetData(enemyUnit.Pokemon);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }
    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }
    private void PLayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }
    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
    }
    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 2)
            {
                currentAction += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction >= 2)
            {
                currentAction -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction % 2 == 0)
            {
                currentAction += 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction % 2 == 1)
            {
                currentAction -= 1;
            }
        }
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PLayerMove();
            }
            else if (currentAction == 1)
            {
                //Pokemon
            }
            else if (currentAction == 2)
            {
                //Run
            }
            else if (currentAction == 3)
            {
                //Bag
            }
        }
    }

}
