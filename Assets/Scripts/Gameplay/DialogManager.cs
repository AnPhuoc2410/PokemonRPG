using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void ShowDialog(Dialog dialog)
    {
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.DialogLines[0]));
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / 30);
        }
        yield return new WaitForSeconds(1f);
    }
}
