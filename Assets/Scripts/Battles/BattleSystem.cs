using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHub playerHub;
    [SerializeField] BattleHub enemyHub;

    private void Start()
    {
        //Setup Battle
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHub.SetData(playerUnit.Pokemon);
        enemyHub.SetData(enemyUnit.Pokemon);
    }
}
