using UnityEngine;

public class IndyRope : PlayerBaseState
{
    public IndyRope(PlayerController pCon) : base(pCon) { }
    private float preEntryMass;
    public override void EnterState(PlayerStateHandler stateHandler)
    {
        Debug.Log("ENTER ROPE");
        preEntryMass = pCon.rb.mass;
        pCon.rb.mass = pCon.simulatedMassOnRope;
        pCon.anim.Play("Rope");
    }

    public override void ExitState(PlayerStateHandler stateHandler)
    {
        pCon.rb.mass = preEntryMass;
    }

    public override void FixedUpdateState(PlayerStateHandler stateHandler)
    {
        VerticalVelocity();
        HorizontalVelocity();
    }

    public override void UpdateState(PlayerStateHandler stateHandler)
    {
    }

    #region MOVEMENT_MANIPULATION
    private void VerticalVelocity()
    {
        //apply increased gravity
        //float acceleration = (pCon.rb.velocity.y < 0) ? 0 : pCon.downwardAccel;
        //pCon.rb.velocity = new Vector2(pCon.rb.velocity.x, pCon.rb.velocity.y - acceleration);

        //apply simualted gravity
        pCon.rb.AddForce(pCon.rb.mass * 9.8f * Vector2.down, ForceMode2D.Force);
    }

    /// <summary>
    /// In this state, he has decreased acceleration
    /// </summary>
    private void HorizontalVelocity()
    {
        float maxAngle = 30f;
        float angle = Mathf.Abs(pCon.attachedRopeSegmentRb.transform.rotation.z);

        if(angle < maxAngle && pCon.rb.velocity.y >= 0)
        {
            float acceleration = 0f;
            //apply airborne multiplier
            if (Mathf.Abs(pCon.horizontal) > 0)
            {
                acceleration = pCon.xAccel * pCon.ropeSwingAccelMultiplier;
            }
            //Calculate force along x-axis to apply to the player
            float movement = pCon.horizontal * acceleration;

            //Convert this to a vector and apply to rigidbody
            pCon.rb.AddRelativeForce(movement * pCon.attachedRopeSegmentRb.transform.right, ForceMode2D.Force);
        }
    }

    #endregion

    #region DETACH
    public override void RopeDetach(PlayerStateHandler stateHandler)
    {
        stateHandler.SwitchState(stateHandler.jumpState);
    }
    #endregion
}
