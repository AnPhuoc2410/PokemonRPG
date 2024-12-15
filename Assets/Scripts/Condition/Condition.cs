using System;
using UnityEngine;

public class Condition
{
   public string Name { get; set; }
    public string Description { get; set; }
    public string Message { get; set; }
    public Action<Pokemon> OnAfterTurn { get; set; }
}