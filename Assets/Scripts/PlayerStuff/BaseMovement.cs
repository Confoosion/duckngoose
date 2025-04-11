using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    // Basic Moving
    protected float speed = 5f;
    protected float horizontal;
    protected bool isFacingRight = true;

    protected Rigidbody2D rb;

    // Jumping
    protected bool canIJump = false;
    protected float jumpingPower = 11f;
    protected bool isJumping = false;
    private bool wasGroundedLastFrame = false;
    protected int AIRJUMPS = 0; // DEFAULT IS 0 JUMPS
    protected int jumpCount = 0; // Current jump count

    // Ground Checking
    protected GroundCheck groundChecker;

    // Coyote time variables
    protected float coyoteTime = 0.2f;
    protected float coyoteTimeCounter;


    // RECEIVING VARIABLES
    public void ReceiveVariables()
    {
        rb = GetComponent<Rigidbody2D>();

        Transform groundChild = transform.Find("GroundCheck");
        if(groundChild != null)
        {
            // Debug.Log("Ground Check connected");
            groundChecker = groundChild.GetComponent<GroundCheck>();
        }
    }

    // GETTING INPUT
    public virtual void ReceiveInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        PlayerMove();

        bool isGrounded = IsGrounded();
        if(isGrounded)
        {
            coyoteTimeCounter = coyoteTime;

            if(SwapCheck())
            {   // SWAPPING BETWEEN CHARACTERS
                POVSwapping.Swapping.SwapCharacter();
            }

            else if(canIJump)
            {   // Check if player can even jump
                if(!wasGroundedLastFrame && isJumping)
                {   // Check if player is on the ground after jumping
                    isJumping = false;
                    jumpCount = 0;
                }   
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        wasGroundedLastFrame = isGrounded;

        // Jumping checks
        if(canIJump)
        {
            if(Input.GetButtonDown("Jump") && (IsGrounded() && !isJumping))
            {   // Ground Jump
                Jump();
            }
            else if(Input.GetButtonDown("Jump") && (!IsGrounded() && jumpCount < AIRJUMPS))
            {
                // Midair Jump
                Jump(1);
            }
        }
        // if(Input.GetButtonDown("Jump") && (IsGrounded() && !isJumping))
        // {   // GROUND JUMP
        //     Jump();
        // }
        // else if(Input.GetButtonDown("Jump") && (!IsGrounded() && jumpCount + 1 < MAXJUMPS))
        // {   // MIDAIR JUMP
        //     Jump();
        // }
    }

    // PLAYER MOVEMENT
    public void PlayerMove()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        Flip();
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
        return(groundChecker.AmIOnGround());
    }

    public void Jump(int airJump = 0)
    {
        jumpCount += airJump;
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        coyoteTimeCounter = 0f; // Remove coyote time
        isJumping = true;
    }

    public bool SwapCheck()
    {
        if(Input.GetKeyDown(KeyCode.C) && IsGrounded())
        {
            return(true);
        }
        return(false);
    }

    // !! SETTERS !!

    public void SetJumps(int jumps)
    {
        canIJump = true;
        AIRJUMPS = jumps;
    }
}
