using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; // Movement speed
    private bool isMoving;  // Whether the player is currently moving
    private Vector2 input;  // Input vector for movement

    public AudioSource footstepSound; // Footstep sound source

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
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        // Move the player towards the target position
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the target position
        transform.position = targetPos;

        isMoving = false;
    }
}
