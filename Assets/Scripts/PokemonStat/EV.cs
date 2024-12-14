using UnityEngine;

[System.Serializable]
public class EV
{
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SpAttack { get; set; }
    public int SpDefense { get; set; }
    public int Speed { get; set; }
    public int HP { get; set; }

    public EV()
    {
        Attack = 0;
        Defense = 0;
        SpAttack = 0;
        SpDefense = 0;
        Speed = 0;
        HP = 0;
    }
}
