using UnityEngine;

public class IndyRun : PlayerBaseState
{
    public IndyRun(PlayerController pCon) : base(pCon) { }

    public override void EnterState(PlayerStateHandler stateHandler)
    {
        Debug.Log("ENTER RUN");
    }

    public override void ExitState(PlayerStateHandler stateHandler)
    {
    }

    public override void UpdateState(PlayerStateHandler stateHandler)
    {
        pCon.anim.Play("Run");

        //TRANSITION TO FALLING
        if (!pCon.isGrounded && pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.fallState);
        }

        //TRANSITION TO IDLE
        else if (pCon.horizontal == 0)
        {
            stateHandler.SwitchState(stateHandler.idleState);
        }
    }

    public override void FixedUpdateState(PlayerStateHandler stateHandler)
    {
        HorizontalVelocity();
    }

    #region MOVEMENT_MANIPULATION
    /// <summary>
    /// Indy will be running on the ground always in the Run State
    /// </summary>
    private void HorizontalVelocity()
    {
        float targetVelocity = pCon.horizontal * pCon.maxSpeed;
        float acceleration;

        //apply normal acceleration/deceleration
        acceleration = (Mathf.Abs(pCon.horizontal) > 0) ? pCon.xAccel : pCon.xDecel;

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - pCon.rb.velocity.x;
        //Calculate force along x-axis to apply to the player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        pCon.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    #endregion

    #region CONTROLS
    public override void Jump(PlayerStateHandler stateHandler)
    {
        if (stateHandler.pCon.isGrounded)
        {
            stateHandler.SwitchState(stateHandler.jumpState);
        }
    }

    public override void Slide(PlayerStateHandler stateHandler)
    {
        if (pCon.isGrounded && Mathf.Abs(pCon.rb.velocity.x) > 4f && !stateHandler.slideOnCooldown)
        {
            stateHandler.StartSlideCooldown();
            stateHandler.SwitchState(stateHandler.slideState);
        }
    }
    #endregion
}
