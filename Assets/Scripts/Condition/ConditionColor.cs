using System.Collections.Generic;
using UnityEngine;

public static class ConditionColor
{
    private static readonly Dictionary<ConditionID, Color> conditionColors = new Dictionary<ConditionID, Color>
    {
        { ConditionID.none, Color.clear },
        { ConditionID.psn, new Color(0.6f, 0.2f, 0.6f) }, // Purple
        { ConditionID.brn, new Color(1f, 0.4f, 0.2f) }, // Orange
        { ConditionID.slp, new Color(0.4f, 0.4f, 1f) }, // Blue
        { ConditionID.frz, new Color(0.6f, 0.8f, 1f) }, // Light Blue
        { ConditionID.par, new Color(1f, 1f, 0.2f) } // Yellow
    };

    public static Color GetColor(ConditionID conditionID)
    {
        if (conditionColors.TryGetValue(conditionID, out Color color))
        {
            return color;
        }
        return Color.clear; // Default color if conditionID is not found
    }
}

