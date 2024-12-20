using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] List<string> dialogLines;

    public List<string> DialogLines => dialogLines;
}