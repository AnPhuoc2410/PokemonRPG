using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour
{
    [SerializeField] Text namePoke;
    [SerializeField] Text levelPoke;
    [SerializeField] Image imagePoke;

    [SerializeField] HPBar hpBar;
    [SerializeField] Text hp;

    Pokemon _pokemon;
    public void Awake()
    {
        imagePoke = GetComponent<Image>();
    }

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        imagePoke.sprite = pokemon.Base.FrontSprite;
        namePoke.text = pokemon.Base.Name;
        levelPoke.text = "Lvl " + pokemon.Level;
        hp.text = pokemon.HP + "/" + pokemon.MaxHP;
        hpBar.SetHealth((float)pokemon.HP / pokemon.MaxHP);
    }
}