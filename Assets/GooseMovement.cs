using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseMovement : MonoBehaviour
{   
    // Follow Mechanic
    [SerializeField] bool isFollowing = true;
    [SerializeField] Transform followPartner;
    [SerializeField] float followDistance = 1.5f;

    private float horizontal;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpingPower = 10f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Coyote time variables
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Update is called once per frame
    void Update()
    {
        if(!isFollowing)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            // Jump logic with coyote time
            if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
            {
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
    }

    private void FixedUpdate()
    {
        if(!isFollowing && IsGrounded())
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
            Vector2 boxSize = new Vector2(0.75f, 0.1f); // Same as in IsGrounded
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, boxSize);
        }
    }
}
