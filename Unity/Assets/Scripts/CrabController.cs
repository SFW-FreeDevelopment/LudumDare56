using System.Collections;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Sprite idleSprite; // Idle sprite
    public Sprite moveSprite; // Moving sprite
    public float animationSpeed = 0.3f; // Speed of the sprite animation

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isAnimating = false; // Check if the animation coroutine is running
    private bool isFacingRight = true; // Check the current facing direction

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Randomly choose a starting direction (left or right)
        if (Random.value > 0.5f)
        {
            Flip(); // Start facing left
        }

        // Start the sprite with the idle sprite
        spriteRenderer.sprite = idleSprite;
    }

    void Update()
    {
        // Move the crab in its current direction
        MoveCrab();

        // Start sprite animation if it's not already running
        if (!isAnimating)
        {
            StartCoroutine(SwitchSprite());
        }
    }

    void MoveCrab()
    {
        // Move the crab left or right based on its facing direction
        float moveDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }

    // Coroutine to alternate between sprites like an animation when moving
    private IEnumerator SwitchSprite()
    {
        isAnimating = true;
        while (true) // Keep animating while the crab is moving
        {
            spriteRenderer.sprite = moveSprite; // Set to moving sprite
            yield return new WaitForSeconds(animationSpeed); // Wait before switching sprite
            spriteRenderer.sprite = idleSprite; // Switch back to idle sprite
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    // Detect collisions and flip the crab's direction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the crab hits something, flip its direction
        if (collision.contacts[0].normal.x != 0) // Check for collisions on the sides
        {
            Flip();
        }
    }

    // Flip the crab sprite when changing direction
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the sprite by inverting the local scale on the X axis
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}