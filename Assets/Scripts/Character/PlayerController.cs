using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 input; 

    public event Action OnEncountered;

    private Character character;

    public AudioSource footstepSound;

    private void Awake()
    {
        character = GetComponent<Character>();
    }
    public void HandleUpdate()
    {
        // Prevent overlapping movement
        if (!character.IsMoving)
        {
            // Get horizontal and vertical input
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Prevent diagonal movement
            if (input.x != 0) input.y = 0;

            // Check for movement input
            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, CheckForEncounter));

                // Start playing footstep sound if not already playing
                if (!footstepSound.isPlaying)
                {
                    footstepSound.Play();
                }
            }
            else
            {
                // Stop footstep sound if player is not moving
                if (footstepSound.isPlaying)
                {
                    footstepSound.Stop();
                }
            }
        }
        character.HandleUpdate();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }
    private void CheckForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.i.GrassLayer))
        {
            if (UnityEngine.Random.Range(1, 101) <= 10) // 10% chance for an encounter
            {
                // Stop any movement
                character.Animator.IsMoving = false;

                // Stop the footstep sound
                if (footstepSound.isPlaying)
                {
                    footstepSound.Stop();
                }
                OnEncountered();
            }
        }
    }
    private void Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

}

