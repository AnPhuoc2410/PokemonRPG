using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;
    public AudioSource stepSound;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public LayerMask obstacleLayer;

    bool IsWalkable(Vector3 targetPos)
    {
        return !Physics2D.OverlapCircle(targetPos, 0.1f, obstacleLayer);
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            // Update Animator parameters
            animator.SetFloat("MoveX", input.x);
            animator.SetFloat("MoveY", input.y);
            animator.SetBool("IsWalking", input != Vector2.zero);

            if (input != Vector2.zero)
            {
                var targetPos = transform.position + new Vector3(input.x, input.y);
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }

            else
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // Move player towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Play step sound at intervals
            if (!stepSound.isPlaying)
            {
                stepSound.Play();
                yield return new WaitForSeconds(0.3f); // Adjust interval to match step rhythm
            }
            else
            {
                yield return null;
            }
        }

        transform.position = targetPos;
        isMoving = false;

        animator.SetBool("IsWalking", false);
    }

}
