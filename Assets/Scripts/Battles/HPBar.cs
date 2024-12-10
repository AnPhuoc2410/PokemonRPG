using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject _healthBar;
    public void SetHealth(float health)
    {
        _healthBar.transform.localScale = new Vector3(health, 1f, 1f);
    }
}
