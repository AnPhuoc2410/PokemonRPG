using UnityEngine;

[CreateAssetMenu(fileName = "MoveBase", menuName = "Pokemons/Create new moves")]
public class MoveBase : ScriptableObject
{
    [SerializeField] private string _name;

    [TextArea]
    [SerializeField] private string _description;

    [SerializeField] private PokemonType _type;
    [SerializeField] private int _power;
    [SerializeField] private int _accuracy;
    [SerializeField] private int _pp;

}
