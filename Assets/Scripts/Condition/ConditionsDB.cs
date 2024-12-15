using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.Posion, new Condition
            {
                Name = "Posion",
                Description = "This is a posion condition",
                Message = "has been posioned"
            }
        },
        {
            ConditionID.Burn, new Condition
            {
                Name = "Burn",
                Description = "This is a burn condition",
                Message = "has been burned"
            }
        },
        {
            ConditionID.Sleep, new Condition
            {
                Name = "Sleep",
                Description = "This is a sleep condition",
                Message = "has been asleep"
            }
        },
        {
            ConditionID.Paralysis, new Condition
            {
                Name = "Paralysis",
                Description = "This is a paralysis condition",
                Message = "has been paralyzed"
            }
        },
        {
            ConditionID.Freeze, new Condition
            {
                Name = "Freeze",
                Description = "This is a freeze condition",
                Message = "has been frozen"
            }
        }
    };

}
