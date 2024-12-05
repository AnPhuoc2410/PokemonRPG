using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;  
    private Vector2 input; 

    private Animator animator;

    public AudioSource footstepSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
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

                // Start moving towards the target position
                StartCoroutine(Move(targetPos));

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
    }
}
