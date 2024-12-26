using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private List<Vector2> movementPattern;
    [SerializeField] private float timeBetweenPattern;

    private Character character;
    private NPCState state;
    private float idleTimer = 0f;
    private int currentPattern = 0;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Talking;
            character.LookTowards(initiator.position);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => {
                idleTimer = 0f;
                state = NPCState.Idle; }));

        }
    }

    private void Update()
    { 
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }
            }
        }

        character.HandleUpdate();
    }

    private IEnumerator Walk()
    {
        state = NPCState.Walking;
        var oldPosition = transform.position;

        // Enforce alignment with the grid by snapping the position to whole numbers
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            transform.position.z
        );

        // Move to the next position in the movement pattern
        Vector2 nextMove = movementPattern[currentPattern];
        yield return character.Move(nextMove);

        // Update the current movement pattern index
        if(transform.position != oldPosition)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }

}
