using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Component References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRend;
    [SerializeField] Animator anim;

    [Header("Player Settings")]
    [SerializeField] float maxSpeed;
    [SerializeField] float xAccel;
    [SerializeField] float xDecel;
    [SerializeField] float jumpingPower;
    [Space(10)]
    [SerializeField] float downwardAccel;
    [SerializeField] float upwardDecel;
    [Space(10)]
    [SerializeField] float xAirDecelMultiplier;
    [SerializeField] float xAirAccelMultiplier;
    private float horizontal;

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    private bool isGrounded = false;


    //State
    public enum State { IDLE, RUN, JUMP, FALLING}
    private State state = State.IDLE;

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();

        HorizontalVelocity();
        VerticalVelocity();
    }
    private void Update()
    {
        StateControl();
        AnimationControl();
    }

    #region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            state = State.JUMP;
            anim.Play("Jump");
        }
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion

    #region MOVEMENT_MANIPULATION
    /// <summary>
    /// Find target velocity based off of direction and max speed
    /// Move towards that velocity by finding difference between
    /// current velocity and target.
    /// </summary>
    private void HorizontalVelocity()
    {
        //apply acceleration in direction, or decelerate if AD keys not being pressed

        float targetVelocity = horizontal * maxSpeed;
        float acceleration;
        //check if airborne or not
        if (isGrounded)
        {
            //apply normal acceleration/deceleration
            acceleration = (Mathf.Abs(horizontal) > 0) ? xAccel : xDecel;
        }
        else
        {
            //apply airborne multiplier
            acceleration = (Mathf.Abs(horizontal) > 0) ? xAccel * xAirAccelMultiplier : xDecel * xAirDecelMultiplier;
        }

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - rb.velocity.x;
        //Calculate force along x-axis to apply to thr player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    /// <summary>
    /// Controls the vertical speed behaviour
    /// </summary>
    private void VerticalVelocity()
    {
        float acceleration = (rb.velocity.y < 0) ? upwardDecel : downwardAccel;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - acceleration);
    }

    #endregion

    #region STATE
    private void StateControl()
    {
        if(horizontal != 0 && isGrounded)
        {
            state = State.RUN;
        }
        if (!isGrounded && state != State.FALLING)
        {
            state = State.JUMP;
        }
        if(horizontal == 0 && isGrounded)
        {
            state = State.IDLE;
        }
    }


    private void AnimationControl()
    {
        //flip sprite based on facing direction
        if (horizontal > 0)
        {
            spriteRend.flipX = false;
        }
        else if (horizontal < 0)
        {
            spriteRend.flipX = true;
        }

        switch(state){
            case State.RUN:
                if (Mathf.Abs(rb.velocity.x) > 0.2f)
                {
                    anim.Play("Run");
                }
                else if (Mathf.Abs(rb.velocity.x) < 1f)
                {
                    anim.Play("Idle");
                }
                break;

            case State.JUMP:
                if(rb.velocity.y < 0)
                {
                    anim.Play("JumpFall");
                }
                if(rb.velocity.y > 0)
                {
                    anim.Play("Jump");
                }
                break;

            case State.IDLE:
                anim.Play("Idle");
                break;

            default:
                break;
        }

    }
    #endregion
}
