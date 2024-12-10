using UnityEngine;
using UnityEngine.UI;

public class BattleHub : MonoBehaviour
{
    [SerializeField] Text namePoke;
    [SerializeField] Text levelPoke;

    [SerializeField] HPBar hpBar;

    public void SetData(Pokemon pokemon)
    {
        namePoke.text = pokemon.Base.Name;
        levelPoke.text = "Lvl " + pokemon.Level;
        hpBar.SetHealth((float)pokemon.HP / pokemon.MaxHP);
    }
}
