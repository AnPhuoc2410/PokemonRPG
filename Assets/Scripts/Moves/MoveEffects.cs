using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> _boosts;
    [SerializeField] private ConditionID status;
    [SerializeField] private ConditionID volatileStatus;

    public List<StatBoost> Boosts => _boosts;
    public ConditionID Status => status;
    public ConditionID VolatileStatus => volatileStatus;
}
[System.Serializable]
public class SecondaryEffect : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;

    public int ChanceEffect => chance;
    public MoveTarget Target => target;
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}


