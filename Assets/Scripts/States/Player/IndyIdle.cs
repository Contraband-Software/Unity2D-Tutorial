using Software.Contraband.StateMachines;
using UnityEngine;

public class IndyIdle : PlayerBaseState
{
    protected override StateType GetStateInfo => StateType.Default;
    
    public IndyIdle(PlayerStateHandler stateHandler)
        : base(stateHandler) { }

    public override void EnterState()
    {
        Debug.Log("ENTER IDLE");
    }
    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        stateHandler.pCon.anim.Play("Idle");

        //TRANSITION TO FALLING
        if (!stateHandler.pCon.isGrounded && stateHandler.pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyFall)]);
        }

        //TRANSITION TO RUNNING
        else if(stateHandler.pCon.isGrounded && stateHandler.pCon.horizontal != 0)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyRun)]);
        }
    }

    public override void FixedUpdateState()
    {
        HorizontalVelocity();
    }

    public override void Jump()
    {
        if (stateHandler.pCon.isGrounded)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyJump)]);
        }
    }

    #region MOVEMENT_MANIPULATION
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
}
