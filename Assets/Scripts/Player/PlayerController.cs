using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Component References")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRend;
    public Animator anim;

    [Header("Player Settings")]
    public float maxSpeed;
    public float xAccel;
    public float xDecel;
    public float jumpingPower;
    public float downwardAccel;
    public float upwardDecel;
    [Space(10)]
    public float xAirDecelMultiplier;
    public float xAirAccelMultiplier;
    [Space(10)]
    public float slidingDecel;
    public float horizontal { get; private set; }
    public bool holdingSlide { get; private set; }

    [Header("Cooldowns")]
    public float slideCooldown = 0.5f;

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    public bool isGrounded { get; private set; } = false;

    [Header("State")]
    [SerializeField] private PlayerStateHandler stateHandler;

    [Header("Rope Swinging")]
    public GameObject ropeLastAttached = null;
    bool attachedToRope = false;
    public HingeJoint2D playerHingeJoint;
    public HingeJoint2D playerFootHingeJoint;
    public Rigidbody2D attachedRopeSegmentRb;
    public Rigidbody2D attachedRopeSegmentRbFoot;
    public Rigidbody2D lowerRopeSegmentRb;
    public float ropeSwingAccelMultiplier;
    public float simulatedMassOnRope;

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }
    private void Update()
    {
        
    }

    #region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            stateHandler.HandleJump();
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() == 1f)
        {
            stateHandler.HandleSlide();
            holdingSlide = true;
        }
        if (context.canceled)
        {
            stateHandler.HandleSlideCancel();
            holdingSlide = false;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion

    #region COLLISIONS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateHandler.HandleTriggerEnter(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        stateHandler.HandleCollisionEnter(collision);
    }
    #endregion

    #region ROPE
    public void AttachToRopeSegment(Rigidbody2D ropeSegmentRb)
    {
        if (!attachedToRope)
        {
            attachedToRope = true;
            ropeLastAttached = ropeSegmentRb.transform.parent.gameObject;

            ropeLastAttached.GetComponent<RopeSwingable>().OnPlayerAttach(this);

            //attach hinge
            attachedRopeSegmentRb = ropeSegmentRb;
            playerHingeJoint.enabled = true;
            playerHingeJoint.connectedBody = ropeSegmentRb;

            //try attach foot hinge
            RopeLinePoint thisSeg = attachedRopeSegmentRb.gameObject.GetComponent<RopeLinePoint>();
            RopeLinePoint[] ropeLinePoints = ropeLastAttached.GetComponent<RopeLine>().ropeLinePoints;
            int ropeSegmentNumber = Array.IndexOf(ropeLinePoints, thisSeg);
            if(ropeSegmentNumber + 2 <= ropeLinePoints.Length - 1)
            {
                attachedRopeSegmentRbFoot = ropeLinePoints[ropeSegmentNumber + 2].gameObject.GetComponent<Rigidbody2D>();
                playerFootHingeJoint.enabled = true;
                playerFootHingeJoint.connectedBody = attachedRopeSegmentRbFoot;

                //re-position foot hinge
                playerFootHingeJoint.autoConfigureConnectedAnchor = false;
                playerFootHingeJoint.connectedAnchor = new Vector2(0f, 0f);
                playerFootHingeJoint.anchor = new Vector2(0f, -0.4f);
            }

            //allow rotation
            rb.freezeRotation = false;

            //re-position player hinges
            playerHingeJoint.autoConfigureConnectedAnchor = false;
            playerHingeJoint.connectedAnchor = new Vector2(0f, -0.8f);
            playerHingeJoint.anchor = new Vector2(0f, 0.33f);

        }
        
    }

    public void DetachFromRope(InputAction.CallbackContext context)
    {
        if (context.performed && attachedToRope)
        {
            attachedToRope = false;
            ropeLastAttached.GetComponent<RopeSwingable>().OnPlayerDetach();
            ropeLastAttached = null;

            //disable hinge
            attachedRopeSegmentRb = null;
            playerHingeJoint.enabled = false;
            playerHingeJoint.connectedBody = null;

            attachedRopeSegmentRbFoot = null;
            playerFootHingeJoint.enabled = false;
            playerFootHingeJoint.connectedBody = null;

            //reset rotation
            rb.transform.rotation = Quaternion.identity;
            rb.freezeRotation = true;

            //reset player hinge
            playerHingeJoint.autoConfigureConnectedAnchor = true;
            playerFootHingeJoint.autoConfigureConnectedAnchor = true;

            stateHandler.HandleRopeDetach();
        }
    }
    #endregion
}
