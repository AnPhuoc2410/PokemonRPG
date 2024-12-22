using UnityEngine;

public class SpriteAnimator 
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] frames;

    private int currentFrame;
    private float timePerFrame;
    private float timer;

    public SpriteAnimator(SpriteRenderer spriteRenderer, Sprite[] frames, float timePerFrame = 0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.timePerFrame = timePerFrame;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= timePerFrame)
        {
           currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= timePerFrame;
        }
    }

    public void Reset()
    {
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = frames[currentFrame];
    }


}
