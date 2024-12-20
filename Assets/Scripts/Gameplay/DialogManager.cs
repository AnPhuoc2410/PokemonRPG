using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    public event System.Action OnShowDialog;
    public event System.Action OnCloseDialog;

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    Dialog currentDialog;
    int currentLine = 0;
    bool isTyping;
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        currentDialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.DialogLines[0]));
    }
    public void HandleUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < currentDialog.DialogLines.Count)
            {
                dialogText.text = "";
                StartCoroutine(TypeDialog(currentDialog.DialogLines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialog(string dialog)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / 30);
        }
        yield return new WaitForSeconds(1f);
        isTyping = false;
    }
}
