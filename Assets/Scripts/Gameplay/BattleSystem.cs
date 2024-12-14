using System;
using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHub playerHub;
    [SerializeField] BattleHub enemyHub;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    private BattleState state;
    private int currentAction;
    private int currentMove;
    private int currentPokemon;

    private PokemonParty playerParty;
    private Pokemon wildPokemon;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        //Setup Battle
        playerUnit.Setup(playerParty.GetNotFaintedPokemon());
        enemyUnit.Setup(wildPokemon);
        playerHub.SetData(playerUnit.Pokemon);
        enemyHub.SetData(enemyUnit.Pokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared!");
        PlayerAction();
    }
    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }
    private void PLayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }
    private void OpenParty()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }
    private IEnumerator TryRun()
    {
        state = BattleState.Busy;

        bool canRun = UnityEngine.Random.value < 0.5f;

        if (canRun)
        {
            yield return dialogBox.TypeDialog("Got away safely");
            OnBattleOver(true);
        }
        else
        {
            yield return dialogBox.TypeDialog("Can't escape");
            state = BattleState.EnemyMove;
            StartCoroutine(EnemyMove());
        }
    }
    private IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        var move = playerUnit.Pokemon.Moves[currentMove];

        //if (move.PP <= 0)
        //{
        //    yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}'s {move.Base.Name} has no PP left!");
        //    PlayerAction(); // Return to action selection
        //    yield break;
        //}

        move.PP--;
        //Show Move

        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        //Play Animation
        StartCoroutine(playerUnit.PlayAttackAnimation());
        StartCoroutine(enemyUnit.PlayHitAnimation());
        //Calculate Damage
        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        //Update HP
        yield return enemyHub.UpdateHP();
        //Show Damage 
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} fainted");
            yield return new WaitForSeconds(1f);
            StartCoroutine(enemyUnit.PlayFaintAnimation());
            //End Battle
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }
    private IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");

        StartCoroutine(enemyUnit.PlayAttackAnimation());
        StartCoroutine(playerUnit.PlayHitAnimation());

        var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);

        yield return playerHub.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} fainted");
            yield return new WaitForSeconds(1f);
            StartCoroutine(playerUnit.PlayFaintAnimation());
            //End Battle
            yield return new WaitForSeconds(2f);
            Pokemon nextPokemon = playerParty.GetNotFaintedPokemon();
            if (nextPokemon != null)
            {
                OpenParty();
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            PlayerAction();
        }
    }
    private IEnumerator ShowDamageDetails(DamageDetail damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
        }

        if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog("It's not very effective");
        }
        else if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("It's super effective");
        }
    }
    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    private void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentPokemon;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentPokemon;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentPokemon += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentPokemon -= 2;

        currentPokemon = Mathf.Clamp(currentPokemon, 0, playerParty.Pokemons.Count - 1);
        partyScreen.UpdateSelectedMember(currentPokemon);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Pokemons[currentPokemon];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted Pokemon");
                return;
            }
            if (selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You can't switch to the same Pokemon");
                return;
            }
            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            dialogBox.EnableDialogText(true);
            state = BattleState.PlayerAction;
            PlayerAction();
        }
    }

    private IEnumerator SwitchPokemon(Pokemon selectedMember)
    {
        if (playerUnit.Pokemon.HP > 0)
        {
            yield return dialogBox.TypeDialog($"Return {playerUnit.Pokemon.Base.Name}!");
            playerUnit.PlayReturnAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(selectedMember);
        playerHub.SetData(selectedMember);
        dialogBox.SetMoveNames(selectedMember.Moves);

        yield return dialogBox.TypeDialog($"Go {selectedMember.Base.Name}!");
        playerUnit.PlayEntryAnimation();
        StartCoroutine(EnemyMove());
    }

    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PLayerMove();
            }
            else if (currentAction == 1)
            {
                OpenParty();
            }
            else if (currentAction == 2)
            {
                TryRun();
            }
            else if (currentAction == 3)
            {
                //Bag
            }
        }
    }
    private void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Check if move has PP
            if (playerUnit.Pokemon.Moves[currentMove].PP <= 0)
            {
                StartCoroutine(dialogBox.TypeDialog("No PP left for this move!"));
                return;
            }

            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            state = BattleState.PlayerAction;
            PlayerAction();
        }
    }

}
