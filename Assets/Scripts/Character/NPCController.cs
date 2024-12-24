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

        //// Randomize the direction, ensuring it's straight (horizontal or vertical only)
        //Vector2 direction = Random.value > 0.5f
        //    ? new Vector2(Random.Range(0, 2) * 2 - 1, 0) * 3 // Horizontal: -1 or 1
        //    : new Vector2(0, Random.Range(0, 2) * 2 - 1) * 3; // Vertical: -1 or 1

        //yield return character.Move(direction);
        yield return character.Move(movementPattern[currentPattern]);
        if(transform.position != oldPosition)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
}
