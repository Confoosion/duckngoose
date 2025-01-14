using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpingPower = 11f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Coyote time variables
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Double jump
    private int MAXJUMPS = 1;
    private int jumpCount = 0;


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            jumpCount = 0;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump logic with coyote time
        if (Input.GetButtonDown("Jump") && (coyoteTimeCounter > 0f || jumpCount < MAXJUMPS))
        {
            jumpCount++;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            coyoteTimeCounter = 0f; // Reset coyote time after jump
        }

        // Jump cut logic (Variable jumps)
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
