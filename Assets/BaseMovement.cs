using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    // Basic Moving
    protected float horizontal;
    protected float speed = 5f;
    protected float jumpingPower = 11f;
    protected bool isFacingRight = true;

    protected Rigidbody2D rb;

    // Ground Checking
    private Vector2 groundCheck_box_vector = new Vector2(0.69f, 0.1f);
    protected Transform groundCheck;
    protected LayerMask groundLayer;

    // Coyote time variables
    protected float coyoteTime = 0.2f;
    protected float coyoteTimeCounter;

    // RECEIVING VARIABLES
    public void ReceiveVariables()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
        groundLayer = (4 | 8);
    }

    // GETTING INPUT
    public virtual void ReceiveInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    // PLAYER MOVEMENT
    public void PlayerMove()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // FLIP CHARACTER SPRITE (For moving left and right)
    public void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // CHECK IF PLAYER IS ON GROUND
    public bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(0.69f, 0.1f);
        return Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);
    }
}
