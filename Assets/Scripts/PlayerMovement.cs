using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isGrounded = false;
    public float thrust = 7.5f;
    public float speed = 5f;
    public string[] jumpableTags = { "Platform", "Invisible", "Breakable" };

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isJumping && isGrounded) Jump();

        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);
        isGrounded = false;
    }

    public void IncreaseJumpPower(float amount)
    {
        thrust += amount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (HasJumpableTag(collision.gameObject.tag))
        {
            isGrounded = true;
            isJumping = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }

    private bool HasJumpableTag(string tag)
    {
        foreach (string jumpableTag in jumpableTags)
        {
            if (tag == jumpableTag) return true;
        }
        return false;
    }
}
