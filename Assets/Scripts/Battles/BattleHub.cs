using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleHub : MonoBehaviour
{
    [SerializeField] Text namePoke;
    [SerializeField] Text levelPoke;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text hpText;
    [SerializeField] Text statusText;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        namePoke.text = pokemon.Base.Name;
        levelPoke.text = "Lvl " + pokemon.Level;
        hpText.text = pokemon.HP + "/" + pokemon.MaxHP;
        hpBar.SetHealth((float)pokemon.HP / pokemon.MaxHP);
        
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }
    public void SetStatusText()
    {
        if(_pokemon.Status == null)
        {
            statusText.text = "";
            statusText.color = ConditionColor.GetColor(ConditionID.none);
            return;
        }
        else
        {
            statusText.text = _pokemon.Status.ID.ToString().ToUpper();
            statusText.color = ConditionColor.GetColor(_pokemon.Status.ID);
        }
    }

    public IEnumerator UpdateHubHP()
    {
        float currentHP = _pokemon.HP;
        float maxHP = _pokemon.MaxHP;
        bool isHpChanged = _pokemon.isHpChanged;
        if (isHpChanged)
        {
            // Get the current displayed values for the bar and text
            float startHP = float.Parse(hpText.text.Split('/')[0]); 
            float startHealthBar = hpBar.GetCurrentHealth();
            float targetHealthBar = currentHP / maxHP;

            float animationDuration = 0.5f; 
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animationDuration;

                float interpolatedHealth = Mathf.Lerp(startHealthBar, targetHealthBar, t);
                float interpolatedHP = Mathf.Lerp(startHP, currentHP, t);

                // Update bar and text
                hpBar.SetHealth(interpolatedHealth);
                hpText.text = Mathf.FloorToInt(interpolatedHP) + "/" + maxHP;

                yield return null;
            }

            // Ensure final values are set
            hpBar.SetHealth(targetHealthBar);
            hpText.text = currentHP + "/" + maxHP;
            _pokemon.isHpChanged = false;
        }
    }
}
