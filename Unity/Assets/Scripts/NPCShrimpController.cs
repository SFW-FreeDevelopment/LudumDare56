using System.Collections;
using UnityEngine;

public class NPCShrimpController : MonoBehaviour
{
    public float moveSpeed = 2f; // Movement speed
    public float jumpForce = 8f; // Jump force
    public float moveDirectionChangeInterval = 3f; // Max time between direction changes
    public float moveDuration = 1.5f; // How long the shrimp moves during each burst
    public float gravityScale = 0.5f; // Floaty effect for gravity
    public float fallMultiplier = 2.5f; // Falling speed modifier
    public float rotationSpeed = 50f; // Speed at which the shrimp rotates back to 0 on the Z axis
    public float maxRotationAngle = 20f; // Maximum rotation when "diving"
    public float rotationEaseSpeed = 5f; // How fast the rotation correction eases back
    public Sprite idleSprite; // Idle sprite
    public Sprite moveSprite; // Moving sprite
    public float animationSpeed = 0.3f; // Speed of sprite animation
    public Transform eyes; // Reference to eyes child GameObject
    public LayerMask tankBoundaryMask; // Layer mask for tank boundaries

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false; // Check if the shrimp is grounded
    private Vector2 randomDirection; // Random movement direction
    private bool isMoving = false; // Check if the shrimp is currently moving
    private bool canJump = true; // Controls whether the shrimp can jump
    private bool isAnimating = false; // For sprite animation control
    private float targetZRotation = 0f; // The desired Z rotation based on the movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = gravityScale; // Set floaty gravity

        // Start the random movement routine
        StartCoroutine(RandomMovement());
    }

    void Update()
    {
        // Apply random movement during bursts
        if (isMoving)
        {
            rb.velocity = new Vector2(randomDirection.x * moveSpeed, rb.velocity.y);

            // Rotate into the direction of movement for a "diving" effect
            RotateIntoMovement();
        }

        // Apply floaty effect when falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // If grounded and able to jump, randomly trigger a jump and retain movement direction
        if (isGrounded && canJump && Random.Range(0f, 1f) < 0.01f)
        {
            rb.velocity = new Vector2(randomDirection.x * moveSpeed, jumpForce); // Retain horizontal direction while jumping
            StartCoroutine(DisableJumpTemporarily());
        }

        // Rotate shrimp back to Z = 0 over time, with easing
        SmoothRotationToUpright();

        // Flip the sprite to face the correct direction and adjust eyes
        if (randomDirection.x < 0)
        {
            spriteRenderer.flipX = true; // Face left
            eyes.localPosition = new Vector3(-Mathf.Abs(eyes.localPosition.x), eyes.localPosition.y, eyes.localPosition.z); // Move eyes
        }
        else if (randomDirection.x > 0)
        {
            spriteRenderer.flipX = false; // Face right
            eyes.localPosition = new Vector3(Mathf.Abs(eyes.localPosition.x), eyes.localPosition.y, eyes.localPosition.z); // Move eyes
        }

        // Handle animation of shrimp moving and idle
        if (isMoving && !isAnimating)
        {
            StartCoroutine(AnimateSprite());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only interact with the tank boundaries
        if (((1 << collision.gameObject.layer) & tankBoundaryMask) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & tankBoundaryMask) != 0)
        {
            isGrounded = false;
        }
    }

    // Coroutine to enable random movement bursts and jumping
    IEnumerator RandomMovement()
    {
        while (true)
        {
            // Random wait time between movement bursts
            yield return new WaitForSeconds(Random.Range(1f, moveDirectionChangeInterval));

            // Begin movement burst
            isMoving = true;
            randomDirection = new Vector2(Random.Range(-1f, 1f), 0f).normalized;

            // Move for a random duration
            yield return new WaitForSeconds(moveDuration);

            // Stop moving
            isMoving = false;

            // Random wait before starting the next movement burst
            yield return new WaitForSeconds(Random.Range(1f, moveDirectionChangeInterval));
        }
    }

    // Temporarily disable jumping after a jump
    IEnumerator DisableJumpTemporarily()
    {
        canJump = false;
        yield return new WaitForSeconds(1f); // Wait a second before being able to jump again
        canJump = true;
    }

    // Coroutine to alternate between sprites like an animation when moving
    private IEnumerator AnimateSprite()
    {
        isAnimating = true;
        while (isMoving) // Keep animating while the shrimp is moving
        {
            spriteRenderer.sprite = moveSprite; // Switch to moving sprite
            yield return new WaitForSeconds(animationSpeed); // Wait before switching sprite
            spriteRenderer.sprite = idleSprite; // Switch back to idle sprite
            yield return new WaitForSeconds(animationSpeed);
        }
        isAnimating = false;
    }

    // Rotate into the direction of movement to simulate "diving"
    private void RotateIntoMovement()
    {
        if (randomDirection.x > 0)
        {
            // Rotating clockwise when moving right
            targetZRotation = -maxRotationAngle;
        }
        else if (randomDirection.x < 0)
        {
            // Rotating counterclockwise when moving left
            targetZRotation = maxRotationAngle;
        }

        // Apply gradual rotation to simulate the diving effect
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetZRotation), rotationSpeed * Time.deltaTime);
    }

    // Smoothly rotate the shrimp back to Z = 0 (upright position) over time with easing
    private void SmoothRotationToUpright()
    {
        if (!isMoving)
        {
            // Gradually rotate back to upright (Z = 0) after movement
            float currentZRotation = transform.eulerAngles.z;

            // Use Mathf.LerpAngle to ease the rotation back to Z = 0 more smoothly
            float targetRotation = 0f;
            float newRotationZ = Mathf.LerpAngle(currentZRotation, targetRotation, Time.deltaTime * rotationEaseSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newRotationZ);
        }
    }
}
