
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] Text[] actionTexts;
    [SerializeField] Text[] moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;


    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);//1f30
        }
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    } 
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Length; i++)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = Color.red;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }
}
