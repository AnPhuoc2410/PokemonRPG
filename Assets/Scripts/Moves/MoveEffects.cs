using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> _boosts;

    public List<StatBoost> Boosts => _boosts;
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}


