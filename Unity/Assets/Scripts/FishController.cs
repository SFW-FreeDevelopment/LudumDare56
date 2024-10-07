using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour
{
    public float swimSpeed = 2f; // Horizontal swim speed
    public float verticalSpeed = 1f; // Vertical swimming speed
    public float animationSpeed = 0.3f; // Speed of frame animation
    public Sprite idleSprite; // Idle sprite (frame 1)
    public Sprite moveSprite; // Move sprite (frame 2)
    //public float smoothTurnChance = 0.75f; // Chance that the fish will smoothly continue in its current direction
    public float avoidanceDuration = 3f; // Duration to avoid upward or downward direction after hitting something
    public float stuckCheckTime = 2f; // Time interval to check for being stuck
    public float stuckPositionThreshold = 0.1f; // Threshold to detect if the fish is stuck (within this X range)

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool movingRight = true; // Check if the fish is moving right
    private bool avoidUp = false; // Should the fish avoid upward movement for a short time
    private bool avoidDown = false; // Should the fish avoid downward movement for a short time
    private float verticalDirection = 0f; // Vertical movement direction: -1 = down, 1 = up
    private bool isAnimating = false; // To control animation state

    private Vector3 lastPosition; // Last position to check if stuck
    private float stuckTimer = 0f; // Timer to count how long the fish has been stuck in the same spot

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position; // Initialize last position

        StartCoroutine(AnimateFish());
        StartCoroutine(RandomVerticalMovement()); // Start random up/down movement
        StartCoroutine(RandomDirectionChange()); // Start random direction changes
    }

    void FixedUpdate()
    {
        // Apply both horizontal and vertical movement
        MoveFish();
        CheckIfStuck();
    }

    void MoveFish()
    {
        // Determine horizontal velocity
        float horizontalVelocity = movingRight ? swimSpeed : -swimSpeed;

        // Apply vertical movement with avoidance
        if (avoidUp && verticalDirection > 0) // Avoid upward
        {
            verticalDirection = -1f;
        }
        else if (avoidDown && verticalDirection < 0) // Avoid downward
        {
            verticalDirection = 1f;
        }

        // Apply the combined velocity for horizontal and vertical movement
        rb.velocity = new Vector2(horizontalVelocity, verticalDirection * verticalSpeed);
    }

    void FlipSprite()
    {
        // Flip the fish's sprite and adjust Z rotation
        Vector3 localScale = transform.localScale;

        if (movingRight)
        {
            localScale.x = Mathf.Abs(localScale.x); // Ensure it's facing right
            transform.rotation = Quaternion.Euler(0, 0, 10f); // 10 degrees rotation when facing right
        }
        else
        {
            localScale.x = -Mathf.Abs(localScale.x); // Ensure it's facing left
            transform.rotation = Quaternion.Euler(0, 0, -10f); // -10 degrees when facing left
        }

        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect if the fish hit something above or below and adjust vertical direction
        if (collision.contacts[0].normal.y > 0.1f)
        {
            avoidDown = true; // Hit something below, avoid moving down
            StartCoroutine(AvoidVertical("down"));
        }
        else if (collision.contacts[0].normal.y < -0.1f)
        {
            avoidUp = true; // Hit something above, avoid moving up
            StartCoroutine(AvoidVertical("up"));
        }

        // Change direction horizontally with a chance for smooth continuation
        if (collision.contacts[0].normal.x != 0)
        {
            movingRight = !movingRight;
            FlipSprite();

            // Immediately reverse direction to prevent the fish from getting stuck
            rb.velocity = new Vector2(movingRight ? swimSpeed : -swimSpeed, rb.velocity.y);
        }
    }

    // Randomly change vertical direction over time, unless avoiding up or down
    private IEnumerator RandomVerticalMovement()
    {
        while (true)
        {
            // Randomly choose to move up or down
            verticalDirection = Random.Range(-1f, 1f);

            yield return new WaitForSeconds(Random.Range(2f, 4f)); // Change direction every few seconds
        }
    }

    // Randomly change horizontal direction occasionally
    private IEnumerator RandomDirectionChange()
    {
        while (true)
        {
            // Randomly decide to flip direction after a random interval
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            movingRight = !movingRight;
            FlipSprite();
        }
    }

    // Avoid moving up or down for a specified duration after a collision
    private IEnumerator AvoidVertical(string direction)
    {
        yield return new WaitForSeconds(avoidanceDuration);

        if (direction == "up")
        {
            avoidUp = false; // Stop avoiding upward movement
        }
        else if (direction == "down")
        {
            avoidDown = false; // Stop avoiding downward movement
        }
    }

    // Coroutine to animate the fish between two frames
    private IEnumerator AnimateFish()
    {
        isAnimating = true;
        while (true) // Loop for continuous animation
        {
            // Switch between idle and move sprites for the fish
            spriteRenderer.sprite = moveSprite;
            yield return new WaitForSeconds(animationSpeed);
            spriteRenderer.sprite = idleSprite;
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    // Check if the fish is stuck in the same horizontal position for too long
    private void CheckIfStuck()
    {
        // Check if the fish's X position has not changed significantly
        if (Mathf.Abs(transform.position.x - lastPosition.x) < stuckPositionThreshold)
        {
            stuckTimer += Time.fixedDeltaTime;

            // If the fish has been stuck for too long, reverse its direction
            if (stuckTimer > stuckCheckTime)
            {
                movingRight = !movingRight;
                FlipSprite();
                stuckTimer = 0f; // Reset the timer
            }
        }
        else
        {
            // Reset the timer and last position if the fish is moving
            stuckTimer = 0f;
            lastPosition = transform.position;
        }
    }
}
