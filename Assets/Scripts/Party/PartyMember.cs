using System;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour
{
    [SerializeField] Text namePoke;
    [SerializeField] Text levelPoke;
    [SerializeField] Image imagePoke;

    [SerializeField] HPBar hpBar;
    [SerializeField] Text hp;
    private Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        imagePoke.sprite = pokemon.Base.FrontSprite;
        namePoke.text = pokemon.Base.Name;
        levelPoke.text = "Lvl " + pokemon.Level;
        hp.text = pokemon.HP + "/" + pokemon.MaxHP;
        hpBar.SetHealth((float)pokemon.HP / pokemon.MaxHP);
    }

    internal void SetSelected(bool selected)
    {
        if (selected)
        {
            namePoke.color = Color.red;
        }
        else
        {
            namePoke.color = Color.black;
        }
    }
}
