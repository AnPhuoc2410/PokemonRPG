using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject _healthBar;

    private void Start()
    {
        _healthBar.transform.localScale = new Vector3(0.5f, 0.5f);
    }
}
