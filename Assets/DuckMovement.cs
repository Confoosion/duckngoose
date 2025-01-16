using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    // Follow Mechanic
    [SerializeField] bool isFollowing = false;
    [SerializeField] Transform followPartner;
    [SerializeField] float followDistance = 1.5f;

    private float horizontal;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpingPower = 11f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;

    // Ground Checking
    private Vector2 groundCheck_box_vector = new Vector2(0.69f, 0.1f);
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Coyote time variables
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Double Jump
    private int MAXJUMPS = 1;
    private int jumpCount = 0;

    // Wall Jump
    private Vector2 wallJump_box_vector = new Vector2(0.2f, 0.725f);
    private Vector2 wallJump_box_offsets = new Vector2(0.3f, 0f);
    private bool wallJumpLock = false;
    private float WALLJUMPTIME = 0.1f;
    private float wallJumpTimer = 0f;


    void Update()
    {
        Debug.Log(IsWallTouching());
        if(!isFollowing)
        {
            if(!wallJumpLock)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
            }

            if (IsGrounded())
            {
                jumpCount = 0;
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if(wallJumpTimer > 0f)
            {
                wallJumpTimer -= Time.deltaTime;
            }
            else
            {
                wallJumpLock = false;
            }

            // Wall Jump logic
            if(Input.GetButtonDown("Jump") && rb.velocity.x == 0f && !IsGrounded() && IsWallTouching())
            {
                // Debug.Log("Wall Jump");
                WallJump();
            }

            // Normal Jump logic with coyote time
            else if (Input.GetButtonDown("Jump") && (coyoteTimeCounter > 0f || jumpCount < MAXJUMPS) && !IsWallTouching())
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
        }
        else
        {
            Follow();
        }
        Flip();
        // Debug.Log(rb.velocity);
    }

    private void FixedUpdate()
    {
        if(!isFollowing && !wallJumpLock)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
    
    private bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(0.69f, 0.1f);
        return Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);
        // return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    private bool IsWallTouching()
    {
        Vector2 boxPosition = new Vector2(transform.position.x + horizontal * wallJump_box_offsets.x, transform.position.y - wallJump_box_offsets.y);

        // Perform the overlap box check
        return Physics2D.OverlapBox(boxPosition, wallJump_box_vector, 0f, groundLayer);
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

    private void WallJump()
    {
        // Debug.Log(speed);
        rb.velocity = new Vector2(horizontal * -1f * speed, jumpingPower);
        wallJumpLock = true;
        wallJumpTimer = WALLJUMPTIME;
    }

    private void Follow()
    {
        float distance = followPartner.position.x - transform.position.x;
        // Debug.Log(distance);

        if(distance < -followDistance)
        {
            // Follow Left
            horizontal = -1f;
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else if(distance > followDistance)
        {
            // Follow Right
            horizontal = 1f;
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            // Ground Checking
            // Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
            Vector2 boxSize = new Vector2(0.69f, 0.1f); // Same as in IsGrounded
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, boxSize);

            // Wall Checking
            Vector2 boxPosition = new Vector2(transform.position.x + horizontal * wallJump_box_offsets.x, transform.position.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxPosition, wallJump_box_vector);
        }
    }
}
