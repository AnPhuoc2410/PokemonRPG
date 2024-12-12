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
    [SerializeField] private MoveCategory _category;

    public string Name => _name;
    public string Description => _description;
    public PokemonType Type => _type;
    public int Power => _power;
    public int Accuracy => _accuracy;
    public int PP => _pp;
    public MoveCategory Category => _category;
}
