using UnityEngine;

[System.Serializable]
public class IV
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int SpAttack { get; private set; }
    public int SpDefense { get; private set; }
    public int Speed { get; private set; }
    public int HP { get; private set; }

    public void Initialize()
    {
        Attack = Random.Range(0, 32);
        Defense = Random.Range(0, 32);
        SpAttack = Random.Range(0, 32);
        SpDefense = Random.Range(0, 32);
        Speed = Random.Range(0, 32);
        HP = Random.Range(0, 32);
    }
}

