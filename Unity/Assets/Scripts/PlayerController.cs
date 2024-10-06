using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Drawing;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 10f; // Jump force for higher jump
    public float gravityScale = 1f; // Default gravity scale
    public float floatyFactor = 0.3f; // Floaty jump effect
    public float fallMultiplier = 2.5f; // Falling speed modifier for more control
    public int maxJumps = 2; // Max number of jumps (for double jump)
    public float rotationSpeed = 100f; // Rotation speed for flipping
    public Sprite idleSprite; // The sprite when shrimp is idle
    public Sprite moveSprite; // The sprite when shrimp is moving
    public float animationSpeed = 0.2f; // Speed of the sprite animation

    public Transform childSpriteTransform; // Reference to the child GameObject's Transform

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int jumpCount = 0; // Track number of jumps
    private bool isGrounded = true; // Check if shrimp is on the ground
    private bool isMoving = false; // Check if shrimp is moving
    private bool isAnimating = false; // Check if animation coroutine is running
    private bool isFacingRight = true; // Track the shrimp's facing direction

    private void OnEnable()
    {
        EventManager.OnColorChange += SetShrimpColor;
    }

    private void OnDisable()
    {
        EventManager.OnColorChange -= SetShrimpColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetShrimpColor();
        rb.gravityScale = gravityScale;
        spriteRenderer.sprite = idleSprite; // Start with the idle sprite
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement using A/D or left/right arrow keys
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Check if the shrimp is moving and flip the sprite if necessary
        if (move > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (move < 0 && isFacingRight)
        {
            Flip();
        }

        // Start sprite animation if the shrimp is moving
        if (Mathf.Abs(move) > 0.1f)
        {
            if (!isMoving)
            {
                isMoving = true;
                if (!isAnimating) // Start animation if it's not already running
                {
                    StartCoroutine(SwitchSprite());
                }
            }
        }
        else
        {
            isMoving = false;
            spriteRenderer.sprite = idleSprite; // Switch back to idle sprite when stopped
        }

        // Jump on spacebar press if grounded or if the shrimp has jumps left (double jump)
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpCount < maxJumps))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            isGrounded = false;
        }

        // Apply floaty effect when jumping and modify fall speed for better control
        if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (floatyFactor - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Rotation using Q and E keys
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
    }

    // Coroutine to alternate between sprites like an animation when moving
    private IEnumerator SwitchSprite()
    {
        isAnimating = true;
        while (isMoving) // Continue animation while the shrimp is moving
        {
            spriteRenderer.sprite = moveSprite;
            yield return new WaitForSeconds(animationSpeed); // Wait before switching sprite
            spriteRenderer.sprite = idleSprite;
            yield return new WaitForSeconds(animationSpeed);
        }
        isAnimating = false; // Stop animation when the shrimp stops moving
    }

    // Ground and obstacle collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true; // The shrimp is grounded when it touches the ground or an obstacle
            jumpCount = 0; // Reset the jump count when grounded or on an obstacle
        }
    }

    // Flip the shrimp sprite and adjust the child GameObject's position accordingly
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the sprite by inverting the local scale on the X axis
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Adjust the child sprite's position to maintain its relative orientation
        Vector3 childScale = childSpriteTransform.localScale;
        childScale.x *= -1; // Flip the child sprite's X scale too
        childSpriteTransform.localScale = childScale;
    }

    private void SetShrimpColor()
    {
        var hexValue = "";
        switch (SettingsManager.Instance.Settings.ShrimpColor)
        {
            case ShrimpColor.Pink:
                hexValue = "#FA7575";
                break;
            case ShrimpColor.Red:
                hexValue = "#FF3D3D"; // Neon Red
                break;
            case ShrimpColor.Blue:
                hexValue = "#3D9EFF"; // Neon Blue
                break;
            case ShrimpColor.Green:
                hexValue = "#3DFF8C"; // Neon Green
                break;
            case ShrimpColor.Yellow:
                hexValue = "#FFFF3D"; // Neon Yellow
                break;
            default:
                hexValue = "#FFFFFF"; // Fallback color (White)
                break;
        }

        SetColorFromHex(hexValue);
    }
    private void SetColorFromHex(string hex)
    {
        UnityEngine.Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            spriteRenderer.color = color;
        }
    }
}