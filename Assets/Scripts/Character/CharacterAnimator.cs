using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] walkDownSprites;
    [SerializeField] private Sprite[] walkUpSprites;
    [SerializeField] private Sprite[] walkLeftSprites;
    [SerializeField] private Sprite[] walkRightSprites;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    private bool wasMoving;

    private SpriteAnimator walkDownAnim;
    private SpriteAnimator walkUpAnim;
    private SpriteAnimator walkLeftAnim;
    private SpriteAnimator walkRightAnim;

    private SpriteAnimator currentAnim;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize animations
        walkDownAnim = new SpriteAnimator(spriteRenderer, walkDownSprites);
        walkUpAnim = new SpriteAnimator(spriteRenderer, walkUpSprites);
        walkLeftAnim = new SpriteAnimator(spriteRenderer, walkLeftSprites);
        walkRightAnim = new SpriteAnimator(spriteRenderer, walkRightSprites);

        currentAnim = walkDownAnim; // Default animation
    }

    private void Update()
    {
        // Snap small movement values to zero to avoid jittery animation
        MoveX = Mathf.Abs(MoveX) > 0.1f ? Mathf.Sign(MoveX) : 0;
        MoveY = Mathf.Abs(MoveY) > 0.1f ? Mathf.Sign(MoveY) : 0;

        // Determine the current animation based on movement direction
        SpriteAnimator newAnim = GetAnimatorForDirection();

        // If the animation or movement state changes, restart the animation
        if (currentAnim != newAnim || IsMoving != wasMoving)
        {
            currentAnim = newAnim;
            currentAnim.Start();
        }

        // Update animation if moving, otherwise reset to the first frame
        if (IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            currentAnim.Reset();
        }

        wasMoving = IsMoving;
    }

    private SpriteAnimator GetAnimatorForDirection()
    {
        // Prioritize horizontal movement over vertical for smoother transitions
        if (MoveX > 0) return walkRightAnim;
        if (MoveX < 0) return walkLeftAnim;
        if (MoveY > 0) return walkUpAnim;
        if (MoveY < 0) return walkDownAnim;

        return currentAnim; // Default to the last used animation if no direction
    }
}
