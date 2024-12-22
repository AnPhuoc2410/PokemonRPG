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

    private bool preMove;

    private SpriteAnimator walkDownAnim;
    private SpriteAnimator walkUpAnim;
    private SpriteAnimator walkLeftAnim;
    private SpriteAnimator walkRightAnim;

    private SpriteAnimator currentAnim;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(spriteRenderer, walkDownSprites);
        walkUpAnim = new SpriteAnimator(spriteRenderer, walkUpSprites);
        walkLeftAnim = new SpriteAnimator(spriteRenderer, walkLeftSprites);
        walkRightAnim = new SpriteAnimator(spriteRenderer, walkRightSprites);

        currentAnim = walkDownAnim;
    }
    private void Update()
    {
        var prevAnim = currentAnim;
        if (MoveX == 1)
            currentAnim = walkRightAnim;
        else if (MoveX == -1)
            currentAnim = walkLeftAnim;
        else if (MoveY == 1)
            currentAnim = walkUpAnim;
        else if (MoveY == -1)
            currentAnim = walkDownAnim;

        if (currentAnim != prevAnim || IsMoving != preMove)
            currentAnim.Start();

        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            currentAnim.Reset();
        preMove = IsMoving;
    }
}
