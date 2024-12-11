using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleHub : MonoBehaviour
{
    [SerializeField] Text namePoke;
    [SerializeField] Text levelPoke;

    [SerializeField] HPBar hpBar;
    [SerializeField] Text hp;

    Pokemon _pokemon;
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        namePoke.text = pokemon.Base.Name;
        levelPoke.text = "Lvl " + pokemon.Level;
        hp.text = pokemon.HP + "/" + pokemon.MaxHP;
        hpBar.SetHealth((float)pokemon.HP / pokemon.MaxHP);
    }
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHealthSmooth((float)_pokemon.HP / _pokemon.MaxHP);
        yield return hp.text = _pokemon.HP + "/" + _pokemon.MaxHP;
    }
}
