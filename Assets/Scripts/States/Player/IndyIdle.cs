using UnityEngine;

public class IndyIdle : PlayerBaseState
{
    public IndyIdle(PlayerController pCon) : base(pCon) { }

    public override void EnterState(PlayerStateHandler stateHandler)
    {
        Debug.Log("ENTER IDLE");
    }
    public override void ExitState(PlayerStateHandler stateHandler)
    {
    }

    public override void UpdateState(PlayerStateHandler stateHandler)
    {
        stateHandler.pCon.anim.Play("Idle");

        //TRANSITION TO FALLING
        if (!pCon.isGrounded && pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.fallState);
        }

        //TRANSITION TO RUNNING
        else if(pCon.isGrounded && pCon.horizontal != 0)
        {
            stateHandler.SwitchState(stateHandler.runState);
        }
    }

    public override void FixedUpdateState(PlayerStateHandler stateHandler)
    {
        HorizontalVelocity();
    }

    public override void Jump(PlayerStateHandler stateHandler)
    {
        if (stateHandler.pCon.isGrounded)
        {
            stateHandler.SwitchState(stateHandler.jumpState);
        }
    }

    #region MOVEMENT_MANIPULATION
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
}
