using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseMovement : BaseMovement
{   
    void Awake()
    {
        // Setting variables for the BaseMovement inheriting
        ReceiveVariables();
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveInput();

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
        Flip();
    }

    private void FixedUpdate()
    {
        if(IsGrounded())
        {
            PlayerMove();
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
