using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 10f; // Jump force for higher jump
    public float gravityScale = 1f; // Default gravity scale
    public float floatyFactor = 0.3f; // Floaty jump effect
    public float fallMultiplier = 2.5f; // Falling speed modifier for more control
    public int maxJumps = 2; // Max number of jumps (for double jump)
    public float rotationSpeed = 100f; // Rotation speed for flipping

    private Rigidbody2D rb;
    private int jumpCount = 0; // Track number of jumps
    private bool isGrounded = true; // Check if shrimp is on the ground

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement using A/D or left/right arrow keys
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

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

    // Ground and obstacle collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true; // The shrimp is grounded when it touches the ground or an obstacle
            jumpCount = 0; // Reset the jump count when grounded or on an obstacle
        }
    }
}