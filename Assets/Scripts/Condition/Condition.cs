using System;
using UnityEngine;

public class Condition
{
    public ConditionID ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Message { get; set; }
    public Action<Pokemon> OnStart { get; set; }
    public Func<Pokemon, bool> OnBeforeTurn { get; set; }
    public Action<Pokemon> OnAfterTurn { get; set; }
}
