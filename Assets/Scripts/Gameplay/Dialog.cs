using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> dialogLines;

    public List<string> DialogLines => dialogLines;
}