using System.Collections;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }
    public void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = player.transform.position - transform.position;
        var moveVector = new Vector2(diff.x, diff.y).normalized;
        //moveVector = new Vector2(Mathf.Round(moveVector.x), Mathf.Round(moveVector.y));

        // Wait for the character to finish moving
        yield return character.Move(moveVector);
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
                {
                    Debug.Log("Starting Trainer View");
                }));
    }
    public void SetFovRotation(Direction dir)
    {
        float angle = dir switch
        {
            Direction.Down => 0f,
            Direction.Right => 90f,
            Direction.Up => 180f,
            Direction.Left => 270f,
            _ => 0f
        };
        fov.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
