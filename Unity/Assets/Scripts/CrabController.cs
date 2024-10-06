using System.Collections;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Sprite idleSprite; // Idle sprite
    public Sprite moveSprite; // Moving sprite
    public float animationSpeed = 0.3f; // Speed of the sprite animation
    public float rightingSpeed = 2f; // Speed at which the crab rights itself
    public float uprightThreshold = 0.1f; // Threshold to consider the crab "upright"
    public float jumpForce = 5f; // Force applied when the crab jumps
    public float stuckThreshold = 1f; // Time threshold for determining if crab is stuck
    public float stuckDistanceThreshold = 0.1f; // Minimum movement to not be considered stuck

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isAnimating = false; // Check if the animation coroutine is running
    private bool isFacingRight = true; // Check the current facing direction
    private bool isUpright = true; // Check if the crab is upright
    private Vector2 lastPosition; // To track if the crab is stuck
    private float stuckTimer = 0f; // Timer to check how long crab has been stuck

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

        // Initialize last position for stuck detection
        lastPosition = transform.position;
    }

    void Update()
    {
        // Check if the crab is upright before moving
        if (!IsCrabUpright())
        {
            RightCrab(); // Rotate the crab back to the upright position
        }
        else
        {
            MoveCrab(); // Move the crab if it is upright

            // Start sprite animation if it's not already running
            if (!isAnimating)
            {
                StartCoroutine(SwitchSprite());
            }

            // Check if the crab is stuck and make it jump
            CheckIfStuck();
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

    // Check if the crab is upright
    private bool IsCrabUpright()
    {
        // Check if the crab's rotation is near zero (upright position)
        float zRotation = transform.eulerAngles.z;
        isUpright = Mathf.Abs(zRotation) < uprightThreshold || Mathf.Abs(zRotation - 360) < uprightThreshold;
        return isUpright;
    }

    // Rotate the crab back to the upright position
    private void RightCrab()
    {
        // Smoothly rotate the crab back to the upright position
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0); // Upright rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rightingSpeed);

        // Stop horizontal movement while the crab is righting itself
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    // Check if the crab is stuck (i.e., hasn't moved much in a certain amount of time)
    private void CheckIfStuck()
    {
        // Calculate the distance the crab has moved since the last frame
        float distanceMoved = Vector2.Distance(lastPosition, transform.position);

        // If the crab hasn't moved enough, start the stuck timer
        if (distanceMoved < stuckDistanceThreshold)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f; // Reset the timer if the crab is moving
        }

        // Update the last position
        lastPosition = transform.position;

        // If the stuck timer exceeds the threshold, make the crab jump
        if (stuckTimer > stuckThreshold)
        {
            Jump();
            stuckTimer = 0f; // Reset the timer after jumping
        }
    }

    // Make the crab jump over obstacles
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
