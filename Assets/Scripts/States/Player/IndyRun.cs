using UnityEngine;

public class IndyRun : PlayerBaseState
{
    public IndyRun(PlayerStateHandler stateHandler)
        : base(stateHandler) { }

    public override void EnterState()
    {
        Debug.Log("ENTER RUN");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        stateHandler.pCon.anim.Play("Run");

        //TRANSITION TO FALLING
        if (!stateHandler.pCon.isGrounded && stateHandler.pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyFall)]);
        }

        //TRANSITION TO IDLE
        else if (stateHandler.pCon.horizontal == 0)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyIdle)]);
        }
    }

    public override void FixedUpdateState()
    {
        HorizontalVelocity();
    }

    #region MOVEMENT_MANIPULATION
    /// <summary>
    /// Indy will be running on the ground always in the Run State
    /// </summary>
    private void HorizontalVelocity()
    {
        float targetVelocity = stateHandler.pCon.horizontal * stateHandler.pCon.maxSpeed;
        float acceleration;

        //apply normal acceleration/deceleration
        acceleration = (Mathf.Abs(stateHandler.pCon.horizontal) > 0) ? stateHandler.pCon.xAccel : stateHandler.pCon.xDecel;

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - stateHandler.pCon.rb.velocity.x;
        //Calculate force along x-axis to apply to the player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        stateHandler.pCon.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    #endregion

    #region CONTROLS
    public override void Jump()
    {
        if (stateHandler.pCon.isGrounded)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyJump)]);
        }
    }

    public override void Slide()
    {
        if (stateHandler.pCon.isGrounded && Mathf.Abs(stateHandler.pCon.rb.velocity.x) > 4f && !stateHandler.slideOnCooldown)
        {
            stateHandler.StartSlideCooldown();
            stateHandler.SwitchState(stateHandler.States[typeof(IndySlide)]);
        }
    }
    #endregion
}
