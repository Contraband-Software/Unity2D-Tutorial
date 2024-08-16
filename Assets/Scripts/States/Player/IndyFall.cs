using UnityEngine;

public class IndyFall : PlayerBaseState
{
    public IndyFall(PlayerStateHandler stateHandler) 
        : base(stateHandler){ }

    public override void EnterState()
    {
        Debug.Log("ENTER FALL");
        stateHandler.pCon.anim.Play("JumpFall");
    }
    public override void ExitState()
    {
    }

    public override void FixedUpdateState()
    {
        VerticalVelocity();
        HorizontalVelocity();
    }

    public override void UpdateState()
    {
        //TRANSITION to IDLE/RUNNING
        if (stateHandler.pCon.isGrounded)
        {
            if(stateHandler.pCon.holdingSlide && Mathf.Abs(stateHandler.pCon.rb.velocity.x) > 0f && !stateHandler.slideOnCooldown)
            {
                stateHandler.StartSlideCooldown();
                stateHandler.SwitchState(stateHandler.States[typeof(IndySlide)]);
            }
            else if (stateHandler.pCon.horizontal == 0)
            {
                stateHandler.SwitchState(stateHandler.States[typeof(IndyIdle)]);
            }
            else
            {
                stateHandler.SwitchState(stateHandler.States[typeof(IndyRun)]);
            }
        }
    }

    #region MOVEMENT_MANIPULATION
    private void VerticalVelocity()
    {
        //apply increased gravity
        float acceleration = (stateHandler.pCon.rb.velocity.y < 0) ? stateHandler.pCon.upwardDecel : stateHandler.pCon.downwardAccel;
        stateHandler.pCon.rb.velocity = new Vector2(stateHandler.pCon.rb.velocity.x, stateHandler.pCon.rb.velocity.y - acceleration);
    }

    private void HorizontalVelocity()
    {
        float targetVelocity = stateHandler.pCon.horizontal * stateHandler.pCon.maxSpeed;
        float acceleration;

        //apply airborne multiplier
        if(Mathf.Abs(stateHandler.pCon.horizontal) > 0)
        {
            acceleration = stateHandler.pCon.xAccel * stateHandler.pCon.xAirAccelMultiplier;
        }
        else
        {
            acceleration = stateHandler.pCon.xDecel* stateHandler.pCon.xAirDecelMultiplier;
        }

        //Calculate difference between current velocity and desired velocity
        float velocityDiff = targetVelocity - stateHandler.pCon.rb.velocity.x;
        //Calculate force along x-axis to apply to thr player
        float movement = velocityDiff * acceleration;

        //Convert this to a vector and apply to rigidbody
        stateHandler.pCon.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    #endregion

    #region COLLISION
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Rope"))
        {
            Debug.Log("hit ROPE");
            //check conditions for entering rope state
            GameObject ropeCollided = collider.transform.parent.gameObject;
            if (stateHandler.pCon.ropeLastAttached == null || ropeCollided != stateHandler.pCon.ropeLastAttached)
            {
                Rigidbody2D ropeSegmentRb = collider.transform.GetComponent<Rigidbody2D>();
                stateHandler.pCon.AttachToRopeSegment(ropeSegmentRb);

                stateHandler.SwitchState(stateHandler.States[typeof(IndyRope)]);
            }
        }
    }
    #endregion
}
