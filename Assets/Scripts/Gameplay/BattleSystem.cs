using System;
using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleDialogBox dialogBox;
    [SerializeField] private PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    private BattleState state;
    private int currentAction;
    private int currentMove;
    private int currentPokemonIndex;

    private PokemonParty playerParty;
    private Pokemon wildPokemon;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetNotFaintedPokemon());
        enemyUnit.Setup(wildPokemon);
        partyScreen.Init();
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared!");
        FirstTurn();
    }

    private void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    private void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    private void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    private void EndBattle(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnBattleOver?.Invoke(won);
    }
    private void FirstTurn()
    {
        if (playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed)
        {
            ActionSelection();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator TryRun()
    {
        state = BattleState.Busy;
        bool canRun = UnityEngine.Random.value < 0.5f;

        if (canRun)
        {
            yield return dialogBox.TypeDialog("Got away safely!");
            EndBattle(true);
        }
        else
        {
            yield return dialogBox.TypeDialog("Can't escape!");
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return ExecuteMove(playerUnit, enemyUnit, move);

        if (state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return ExecuteMove(enemyUnit, playerUnit, move);

        if (state == BattleState.PerformMove)
        {
            ActionSelection();
        }
    }

    private IEnumerator ExecuteMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeTurn();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}!");

        // Play attack animation
        yield return sourceUnit.PlayAttackAnimation();

        // Handle moves with damage and/or status effects
        if (move.Base.Category == MoveCategory.Physical || move.Base.Category == MoveCategory.Special)
        {
            // Play hit animation
            yield return targetUnit.PlayHitAnimation();

            // Calculate damage and update HP
            var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
            yield return targetUnit.Hud.UpdateHubHP();

            // Show damage details
            yield return ShowDamageDetails(damageDetails);
        }

        yield return MoveEffect(move, sourceUnit.Pokemon, targetUnit.Pokemon);

        // Check if the target has fainted
        if (targetUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} fainted!");
            yield return targetUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
            CheckBattleOver(targetUnit);
        }

        // Handle post-turn effects like Poison or Burn
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.UpdateHubHP();

        // Check if the source Pokémon fainted due to post-turn effects
        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} fainted!");
            yield return sourceUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
            CheckBattleOver(sourceUnit);
        }
    }

    private IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }
    private IEnumerator MoveEffect(Move move, Pokemon source, Pokemon target)
    {
        var effect = move.Base.Effects;
        if (effect.Boosts != null)
        {
            if (move.Base.Target == MoveTarget.Self)
                source.ApplyBoosts(effect.Boosts);
            else
                target.ApplyBoosts(effect.Boosts);
        }
        if (effect.Status != ConditionID.none)
            target.SetStatus(effect.Status);
        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    private void CheckBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetNotFaintedPokemon();
            if (nextPokemon != null)
                OpenPartyScreen();
            else
                EndBattle(false);
        }
        else
        {
            EndBattle(true);
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
            yield return dialogBox.TypeDialog("It's not very effective...");
        }
        else if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("It's super effective!");
        }
    }

    public void HandleUpdate()
    {
        switch (state)
        {
            case BattleState.ActionSelection:
                HandleActionSelection();
                break;
            case BattleState.MoveSelection:
                HandleMoveSelection();
                break;
            case BattleState.PartyScreen:
                HandlePartySelection();
                break;
        }
    }

    private void HandleActionSelection()
    {
        HandleNavigation(ref currentAction, 4);
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (currentAction)
            {
                case 0:
                    MoveSelection();
                    break;
                case 1:
                    OpenPartyScreen();
                    break;
                case 2:
                    StartCoroutine(TryRun());
                    break;
                case 3:
                    // Handle Bag action
                    break;
            }
        }
    }

    private void HandleMoveSelection()
    {
        HandleNavigation(ref currentMove, playerUnit.Pokemon.Moves.Count);
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (playerUnit.Pokemon.Moves[currentMove].PP <= 0)
            {
                StartCoroutine(dialogBox.TypeDialog("No PP left for this move!"));
                return;
            }

            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }

    private void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentPokemonIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentPokemonIndex--;
        }

        if (currentPokemonIndex < 0)
            currentPokemonIndex = playerParty.Pokemons.Count - 1; // Wrap to last Pokémon
        else if (currentPokemonIndex >= playerParty.Pokemons.Count)
            currentPokemonIndex = 0; // Wrap to first Pokémon


        partyScreen.UpdateSelectedMember(currentPokemonIndex);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Pokemons[currentPokemonIndex];
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
            state = BattleState.ActionSelection;
            ActionSelection();
        }
    }
    private IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        bool currentPokemonFainted = playerUnit.Pokemon.HP <= 0;

        // Handle return animation if the current Pokémon is not fainted
        if (!currentPokemonFainted)
        {
            yield return dialogBox.TypeDialog($"Return {playerUnit.Pokemon.Base.Name}!");
            yield return playerUnit.PlayReturnAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}!");
        playerUnit.PlayEntryAnimation();

        yield return new WaitForSeconds(1.5f);

        // Determine the next action based on the situation
        if (currentPokemonFainted)
        {
            yield return new WaitForSeconds(1f);
            ActionSelection();
        }
        else
        {
            yield return EnemyMove();
        }
    }

    private void HandleNavigation(ref int currentIndex, int maxIndex)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentIndex++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentIndex--;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentIndex += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentIndex -= 2;

        currentIndex = Mathf.Clamp(currentIndex, 0, maxIndex - 1);
    }
}
