using System.Collections;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject _healthBar;

    public void SetHealth(float health)
    {
        _healthBar.transform.localScale = new Vector3(health, 1f, 1f);
    }

    public IEnumerator SetHealthSmooth(float newHP)
    {
        float currentHP = _healthBar.transform.localScale.x;
        float changeAmt = currentHP - newHP;

        while(currentHP - newHP > Mathf.Epsilon)
        {
            currentHP -= changeAmt * Time.deltaTime;
            _healthBar.transform.localScale = new Vector3(currentHP, 1f, 1f);
            yield return null;
        }
        _healthBar.transform.localScale = new Vector3(newHP, 1f, 1f);
    }
}
