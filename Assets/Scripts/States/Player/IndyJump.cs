using System.Collections;
using UnityEngine;

public class IndyJump : PlayerBaseState
{
    public IndyJump(PlayerController pCon) : base(pCon) { }

    public override void EnterState(PlayerStateHandler stateHandler)
    {
        Debug.Log("ENTER JUMP");
        pCon.rb.velocity = new Vector2(pCon.rb.velocity.x, pCon.jumpingPower);
        pCon.anim.Play("Jump");
    }
    public override void ExitState(PlayerStateHandler stateHandler)
    {
    }

    public override void UpdateState(PlayerStateHandler stateHandler)
    {
        //TRANSITION TO FALLING
        if (pCon.rb.velocity.y < 0)
        {
            stateHandler.SwitchState(stateHandler.fallState);
        }

        //TRANSITION to IDLE/RUNNING
        else if(pCon.isGrounded && pCon.rb.velocity.y == 0)
        {
            if(pCon.horizontal == 0)
            {
                stateHandler.SwitchState(stateHandler.idleState);
            }
            else
            {
                stateHandler.SwitchState(stateHandler.runState);
            }
        }
    }

    public override void FixedUpdateState(PlayerStateHandler stateHandler)
    {
        VerticalVelocity();
        HorizontalVelocity();
    }

    #region MOVEMENT_MANIPULATION
    private void VerticalVelocity()
    {
        //apply increased gravity
        float acceleration = (pCon.rb.velocity.y < 0) ? pCon.upwardDecel : pCon.downwardAccel;
        pCon.rb.velocity = new Vector2(pCon.rb.velocity.x, pCon.rb.velocity.y - acceleration);
    }

    private void HorizontalVelocity()
    {
        float targetVelocity = pCon.horizontal * pCon.maxSpeed;
        float acceleration;

        //apply airborne multiplier
        if (Mathf.Abs(pCon.horizontal) > 0)
        {
            acceleration = pCon.xAccel * pCon.xAirAccelMultiplier;
        }
        else
        {
            acceleration = pCon.xDecel * pCon.xAirDecelMultiplier;
        }

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - pCon.rb.velocity.x;
        //Calculate force along x-axis to apply to thr player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        pCon.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    #endregion

    #region COLLISION
    public override void OnTriggerEnter2D(PlayerStateHandler stateHandler, Collider2D collider)
    {
        if (collider.CompareTag("Rope"))
        {
            //check conditions for entering rope state
            GameObject ropeCollided = collider.transform.parent.gameObject;
            if(pCon.ropeLastAttached == null || ropeCollided != pCon.ropeLastAttached)
            {
                Rigidbody2D ropeSegmentRb = collider.transform.GetComponent<Rigidbody2D>();
                pCon.AttachToRopeSegment(ropeSegmentRb);

                stateHandler.SwitchState(stateHandler.ropeState);
            }
        }
    }
    #endregion
}
