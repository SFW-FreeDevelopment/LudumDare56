using System.Collections;
using UnityEngine;

public class SnailController : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Sprite idleSprite; // Idle sprite
    public Sprite moveSprite; // Moving sprite
    public float animationSpeed = 0.3f; // Speed of the sprite animation
    public float stuckThreshold = 1f; // Time threshold for determining if snail is stuck
    public float stuckDistanceThreshold = 0.1f; // Minimum movement to not be considered stuck
    public bool isOnLeftSide = true; // Whether the snail starts on the left side
    public float directionChangeDelay = 2f; // Delay between direction changes
    public float wallXPosition = -5f; // X position to clamp the snail to the wall (adjust based on your scene)

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider; // Reference to the PolygonCollider2D
    private Vector2[] originalColliderPoints; // Store the original points of the PolygonCollider2D
    private bool isAnimating = false; // Check if the animation coroutine is running
    private bool isMovingUp = false; // Start moving down first
    private Vector2 lastPosition; // To track if the snail is stuck
    private float stuckTimer = 0f; // Timer to check how long the snail has been stuck
    private float directionChangeTimer = 0f; // Timer for controlling direction changes

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>(); // Get the PolygonCollider2D component

        // Store the original points of the PolygonCollider2D
        originalColliderPoints = polygonCollider.points;

        // Start the sprite with the idle sprite
        spriteRenderer.sprite = idleSprite;

        // Reset any manual Y-axis rotation on the GameObject
        transform.rotation = Quaternion.identity;

        // Set the initial rotation based on which side of the tank the snail is on
        if (isOnLeftSide)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); // Face downward on the left side (initial)
            wallXPosition = transform.position.x; // Set the wall position for clamping
        }
        else
        {
            // Ensure the X rotation is set to 180 degrees for the right side
            transform.rotation = Quaternion.Euler(180, 0, 90); // Face downward on the right side (initial)
            wallXPosition = transform.position.x; // Set the wall position for clamping
        }

        // Initialize last position for stuck detection
        lastPosition = transform.position;
    }

    void Update()
    {
        // Clamp the snail's position to the wall to prevent it from drifting off
        ClampToWall();

        // Move the snail up or down
        MoveSnail();

        // Start sprite animation if it's not already running
        if (!isAnimating)
        {
            StartCoroutine(SwitchSprite());
        }

        // Check if the snail is stuck and flip direction
        CheckIfStuck();
    }

    void MoveSnail()
    {
        // Move the snail up or down based on its current direction
        float moveDirection = isMovingUp ? 1 : -1;
        rb.velocity = new Vector2(rb.velocity.x, moveDirection * speed);
    }

    // Coroutine to alternate between sprites like an animation when moving
    private IEnumerator SwitchSprite()
    {
        isAnimating = true;
        while (true) // Keep animating while the snail is moving
        {
            spriteRenderer.sprite = moveSprite; // Set to moving sprite
            yield return new WaitForSeconds(animationSpeed); // Wait before switching sprite
            spriteRenderer.sprite = idleSprite; // Switch back to idle sprite
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    // Detect collisions and flip the snail's direction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Flip direction if the snail hits something above or below, but add a delay
        if (collision.contacts[0].normal.y != 0 && directionChangeTimer >= directionChangeDelay)
        {
            Flip();
            directionChangeTimer = 0f; // Reset the timer after a direction change
        }
    }

    // Flip the snail sprite when changing direction and flip the collider's axes when needed
    private void Flip()
    {
        isMovingUp = !isMovingUp; // Toggle movement direction

        // Handle the flipping logic based on which side the snail is on
        if (isOnLeftSide)
        {
            if (isMovingUp)
            {
                // Moving up on the left side: Rotate to face upward and flip sprite horizontally
                transform.rotation = Quaternion.Euler(0, 0, -90); // Face upward
                spriteRenderer.flipX = true; // Flip horizontally
                FlipPolygonCollider(true, false); // Flip the x-axis of the collider
            }
            else
            {
                // Moving down on the left side: Rotate to face downward and reset flip
                transform.rotation = Quaternion.Euler(0, 0, -90); // Face downward
                spriteRenderer.flipX = false; // Don't flip
                FlipPolygonCollider(false, false); // Restore the original collider points
            }
        }
        else
        {
            // On the right side, ensure X rotation is always set to 180 degrees
            if (isMovingUp)
            {
                // Moving up on the right side: Rotate to face upward, flip sprite and collider
                transform.rotation = Quaternion.Euler(180, 0, 90); // Face upward
                spriteRenderer.flipX = true; // Flip the sprite
                FlipPolygonCollider(true, false); // Flip the x-axis of the collider
            }
            else
            {
                // Moving down on the right side: Rotate to face downward, maintain X rotation
                transform.rotation = Quaternion.Euler(180, 0, 90); // Face downward
                spriteRenderer.flipX = false; // Reset the sprite flip
                FlipPolygonCollider(false, false); // Restore collider points
            }
        }
    }

    // Flip the polygon collider's x-axis based on movement direction
    private void FlipPolygonCollider(bool flipUp, bool flipY)
    {
        Vector2[] flippedPoints = new Vector2[originalColliderPoints.Length];

        for (int i = 0; i < originalColliderPoints.Length; i++)
        {
            // Flip the x-axis and/or y-axis as needed
            flippedPoints[i] = new Vector2(
                flipUp ? -originalColliderPoints[i].x : originalColliderPoints[i].x,
                flipY ? -originalColliderPoints[i].y : originalColliderPoints[i].y
            );
        }

        polygonCollider.points = flippedPoints; // Apply the modified points to the collider
    }

    // Check if the snail is stuck (i.e., hasn't moved much in a certain amount of time)
    private void CheckIfStuck()
    {
        // Calculate the distance the snail has moved since the last frame
        float distanceMoved = Vector2.Distance(lastPosition, transform.position);

        // If the snail hasn't moved enough, start the stuck timer
        if (distanceMoved < stuckDistanceThreshold)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f; // Reset the timer if the snail is moving
        }

        // Update the last position
        lastPosition = transform.position;

        // If the stuck timer exceeds the threshold, flip direction
        if (stuckTimer > stuckThreshold && directionChangeTimer >= directionChangeDelay)
        {
            Flip();
            stuckTimer = 0f; // Reset the timer after flipping
            directionChangeTimer = 0f; // Reset the direction change timer
        }

        directionChangeTimer += Time.deltaTime; // Update the timer for direction changes
    }

    // Keep the snail clamped to the wall to prevent it from drifting off
    private void ClampToWall()
    {
        // Clamp the snail's position on the x-axis to the wall
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = wallXPosition;
        transform.position = clampedPosition;
    }
}
