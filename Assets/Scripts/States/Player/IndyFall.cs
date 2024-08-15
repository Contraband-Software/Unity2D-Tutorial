using UnityEngine;

public class IndyFall : PlayerBaseState
{
    public IndyFall(
        PlayerController pCon, PlayerStateHandler stateHandler) 
        : base(pCon, stateHandler){ }

    public override void EnterState()
    {
        Debug.Log("ENTER FALL");
        pCon.anim.Play("JumpFall");
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
        if (pCon.isGrounded)
        {
            if(pCon.holdingSlide && Mathf.Abs(pCon.rb.velocity.x) > 0f && !stateHandler.slideOnCooldown)
            {
                stateHandler.StartSlideCooldown();
                stateHandler.SwitchState(stateHandler.slideState);
            }
            else if (pCon.horizontal == 0)
            {
                stateHandler.SwitchState(stateHandler.idleState);
            }
            else
            {
                stateHandler.SwitchState(stateHandler.runState);
            }
        }
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
        if(Mathf.Abs(pCon.horizontal) > 0)
        {
            acceleration = pCon.xAccel * pCon.xAirAccelMultiplier;
        }
        else
        {
            acceleration = pCon.xDecel* pCon.xAirDecelMultiplier;
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
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Rope"))
        {
            Debug.Log("hit ROPE");
            //check conditions for entering rope state
            GameObject ropeCollided = collider.transform.parent.gameObject;
            if (pCon.ropeLastAttached == null || ropeCollided != pCon.ropeLastAttached)
            {
                Rigidbody2D ropeSegmentRb = collider.transform.GetComponent<Rigidbody2D>();
                pCon.AttachToRopeSegment(ropeSegmentRb);

                stateHandler.SwitchState(stateHandler.ropeState);
            }
        }
    }
    #endregion
}
