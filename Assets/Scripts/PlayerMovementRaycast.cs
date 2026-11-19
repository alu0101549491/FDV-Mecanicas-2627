using UnityEngine;

public class PlayerMovementRaycast : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isJumping = false;
    private bool isGrounded = false;
    public float thrust = 7.5f;
    public float speed = 5f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        if (isJumping && isGrounded) Jump();

        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            isJumping = true;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);
        isGrounded = false;
        isJumping = false;
    }

    public void IncreaseJumpPower(float amount)
    {
        thrust += amount;
    }

    private bool CheckGrounded()
    {
        Vector2 origin;

        if (groundCheck != null)
        {
            origin = groundCheck.position;
        }
        else if (playerCollider != null)
        {
            origin = new Vector2(
                playerCollider.bounds.center.x,
                playerCollider.bounds.min.y
            );
        }
        else
        {
            origin = transform.position;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }
}
