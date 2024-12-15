using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> _boosts;
    [SerializeField] private ConditionID status;

    public List<StatBoost> Boosts => _boosts;
    public ConditionID Status => status;
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}


