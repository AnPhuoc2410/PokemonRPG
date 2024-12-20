using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;
    public LayerMask interactableLayer;
    private bool isMoving;  
    private Vector2 input; 

    public event Action OnEncountered;

    private Animator animator;

    public AudioSource footstepSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void HandleUpdate()
    {
        // Prevent overlapping movement
        if (!isMoving)
        {
            // Get horizontal and vertical input
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Prevent diagonal movement
            if (input.x != 0) input.y = 0;

            // Check for movement input
            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                // Determine the target position
                var targetPos = transform.position + new Vector3(input.x, input.y);

                if (IsWalkable(targetPos))
                {
                    // Start moving towards the target position
                    StartCoroutine(Move(targetPos));
                }
             
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
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

   
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounter();
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
    private void CheckForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, grassLayer))
        {
            if (UnityEngine.Random.Range(1, 101) <= 50) // 10% chance for an encounter
            {
                // Stop any movement
                isMoving = false;
                animator.SetBool("isMoving", false);

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
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.1f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

}

