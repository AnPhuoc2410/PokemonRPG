using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleHub : MonoBehaviour
{
    [SerializeField] Text namePoke;
    [SerializeField] Text levelPoke;

    [SerializeField] HPBar hpBar;
    [SerializeField] Text hp;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        namePoke.text = pokemon.Base.Name;
        levelPoke.text = "Lvl " + pokemon.Level;
        hp.text = pokemon.HP + "/" + pokemon.MaxHP;
        hpBar.SetHealth((float)pokemon.HP / pokemon.MaxHP);
    }

    public IEnumerator UpdateHubHP()
    {
        float currentHP = _pokemon.HP;
        float maxHP = _pokemon.MaxHP;
        bool isHpChanged = _pokemon.isHpChanged;
        if (isHpChanged)
        {
            // Get the current displayed values for the bar and text
            float startHP = float.Parse(hp.text.Split('/')[0]); // Extract current displayed HP
            float startHealthBar = hpBar.GetCurrentHealth(); // New method in HPBar to get the current scale
            float targetHealthBar = currentHP / maxHP;

            float animationDuration = 0.5f; // Duration for both bar and text animation
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animationDuration;

                // Interpolate bar scale and HP text
                float interpolatedHealth = Mathf.Lerp(startHealthBar, targetHealthBar, t);
                float interpolatedHP = Mathf.Lerp(startHP, currentHP, t);

                // Update bar and text
                hpBar.SetHealth(interpolatedHealth);
                hp.text = Mathf.FloorToInt(interpolatedHP) + "/" + maxHP;

                yield return null;
            }

            // Ensure final values are set
            hpBar.SetHealth(targetHealthBar);
            hp.text = currentHP + "/" + maxHP;
            _pokemon.isHpChanged = false;
        }
    }
}
