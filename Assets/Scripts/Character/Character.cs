using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }
    private CharacterAnimator animator;

    private const float PathCheckBoxSize = 0.2f;
    private const float WalkableRadius = 0.1f;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public CharacterAnimator Animator => animator;

    public IEnumerator Move(Vector2 moveVector, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVector.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVector.y, -1f, 1f);

        var targetPos = transform.position + new Vector3(moveVector.x, moveVector.y);

        if (!IsPathClear(targetPos)) yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var direction = (targetPos - transform.position).normalized;
        var distance = Vector3.Distance(transform.position, targetPos);

        return !Physics2D.BoxCast(
            transform.position + direction,
            new Vector2(PathCheckBoxSize, PathCheckBoxSize),
            0f,
            direction,
            distance - 1,
            GameLayers.i.SolidObjectsLayer |
            GameLayers.i.InteractableLayer |
            GameLayers.i.PlayerLayer
        );
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xDiff == 0 || yDiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
        }
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }
}
