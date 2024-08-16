using UnityEngine;

public class IndySlide : PlayerBaseState
{
    public IndySlide(PlayerStateHandler stateHandler) 
        : base(stateHandler) {
    }

    public override void EnterState()
    {
        //play animation
        stateHandler.pCon.anim.Play("Slide");
        Debug.Log("ENTER SLIDE");

        //set smaller hitbox
        stateHandler.col.offset = stateHandler.slideCollider.offset;
        stateHandler.col.size = stateHandler.slideCollider.size;
    }

    public override void ExitState()
    {
        //reset hitbox to idle
        stateHandler.col.offset = stateHandler.idleCollider.offset;
        stateHandler.col.size = stateHandler.idleCollider.size;
    }

    public override void FixedUpdateState()
    {
        //no horizontal acceleration (cant change direction)
        //tending towards zero velocity
        float targetVelocity = 0;
        float acceleration = stateHandler.pCon.slidingDecel;

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - stateHandler.pCon.rb.velocity.x;
        //Calculate force along x-axis to apply to the player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        stateHandler.pCon.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public override void UpdateState()
    {
        //TRANSITION TO FALLING
        if (!stateHandler.pCon.isGrounded && stateHandler.pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyFall)]);
        }

        //TRANSITION TO IDLE WHEN VELOCITY REACHES NEAR ZERO
        else if(stateHandler.pCon.isGrounded && Mathf.Abs(stateHandler.pCon.rb.velocity.x) < 0.6f)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyIdle)]);
        }
    }

    #region CONTROLS
    /// <summary>
    /// On cancelling the slide, decide which state to transition to based
    /// on horizontal inputs
    /// </summary>
    /// <param name="stateHandler"></param>
    public override void SlideCancel()
    {
        if(Mathf.Abs(stateHandler.pCon.horizontal) != 0)
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyRun)]);
        }
        else
        {
            stateHandler.SwitchState(stateHandler.States[typeof(IndyIdle)]);
        }
    }

    /// <summary>
    /// On jump, slide gets cancelled implicitly
    /// </summary>
    /// <param name="stateHandler"></param>
    public override void Jump()
    {
        stateHandler.SwitchState(stateHandler.States[typeof(IndyJump)]);
    }
    #endregion
}
