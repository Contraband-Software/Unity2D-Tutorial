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

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    private float horizontal;

    private void FixedUpdate()
    {
        AnimationControl();
        HorizontalVelocity();
        VerticalVelocity();
    }

    #region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
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
        if (IsGrounded())
        {
            //apply normal acceleration/deceleration
            acceleration = (Mathf.Abs(horizontal) == 1) ? xAccel : xDecel;
        }
        else
        {
            //apply airborne multiplier
            acceleration = (Mathf.Abs(horizontal) == 1) ? xAccel * xAirAccelMultiplier : xDecel * xAirDecelMultiplier;
        }

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - rb.velocity.x;
        //Calculate force along x-axis to apply to thr player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    private void AnimationControl()
    {
        //flip sprite based on facing direction
        if(horizontal == 1)
        {
            spriteRend.flipX = false;
        }
        else if(horizontal == -1)
        {
            spriteRend.flipX = true;
        }

        //play run animation if x velocity is above threshold
        if(Mathf.Abs(rb.velocity.x) < 1f && IsGrounded())
        {
            anim.Play("Idle");
        }

        //otherwise do idle animation if grounded
        else if (Mathf.Abs(rb.velocity.x) > 0.2f && IsGrounded())
        {
            anim.Play("Run");
        }
        else if (!IsGrounded())
        {
            //jump
            anim.StopPlayback();
        }

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
}
