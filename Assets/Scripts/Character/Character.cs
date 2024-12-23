using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }
    private CharacterAnimator animator;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }
    public CharacterAnimator Animator => animator;
    public IEnumerator Move(Vector2 moveVector, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVector.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVector.y, -1f, 1f);         // Determine the target position
        var targetPos = transform.position + new Vector3(moveVector.x, moveVector.y);
        if (!IsWalkable(targetPos)) yield break;

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
    private void IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude);
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
