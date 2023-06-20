using UnityEngine;

public class IndySlide : PlayerBaseState
{
    private enum HorizontalInput { ONENTRY, BLANK, RETAPPED}
    public IndySlide(PlayerController pCon)
    {
        this.pCon = pCon;
    }

    public override void EnterState(PlayerStateHandler stateHandler)
    {
        //play animation
        pCon.anim.Play("Slide");
        Debug.Log("ENTER SLIDE");

        //set smaller hitbox
        stateHandler.col.offset = stateHandler.slideCollider.offset;
        stateHandler.col.size = stateHandler.slideCollider.size;
    }

    public override void ExitState(PlayerStateHandler stateHandler)
    {
        //reset hitbox to idle
        stateHandler.col.offset = stateHandler.idleCollider.offset;
        stateHandler.col.size = stateHandler.idleCollider.size;
    }

    public override void FixedUpdateState(PlayerStateHandler stateHandler)
    {
        //no horizontal acceleration (cant change direction)
        //tending towards zero velocity
        float targetVelocity = 0;
        float acceleration = pCon.slidingDecel;

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - pCon.rb.velocity.x;
        //Calculate force along x-axis to apply to the player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        pCon.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public override void UpdateState(PlayerStateHandler stateHandler)
    {
        //TRANSITION TO FALLING
        if (!pCon.isGrounded && pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.fallState);
        }

        //TRANSITION TO IDLE WHEN VELOCITY REACHES NEAR ZERO
        else if(pCon.isGrounded && Mathf.Abs(pCon.rb.velocity.x) < 0.6f)
        {
            stateHandler.SwitchState(stateHandler.idleState);
        }
    }

    #region CONTROLS
    /// <summary>
    /// On cancelling the slide, decide which state to transition to based
    /// on horizontal inputs
    /// </summary>
    /// <param name="stateHandler"></param>
    public override void SlideCancel(PlayerStateHandler stateHandler)
    {
        if(Mathf.Abs(pCon.horizontal) != 0)
        {
            stateHandler.SwitchState(stateHandler.runState);
        }
        else
        {
            stateHandler.SwitchState(stateHandler.idleState);
        }
    }

    /// <summary>
    /// On jump, slide gets cancelled implicitly
    /// </summary>
    /// <param name="stateHandler"></param>
    public override void Jump(PlayerStateHandler stateHandler)
    {
        stateHandler.SwitchState(stateHandler.jumpState);
    }
    #endregion
}
