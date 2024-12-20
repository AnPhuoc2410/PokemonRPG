using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        DialogManager.Instance.ShowDialog(dialog);
    }
}
