using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Component References")]
    public Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRend;
    public Animator anim;

    [Header("Player Settings")]
    public float maxSpeed;
    public float xAccel;
    public float xDecel;
    public float jumpingPower;
    public float downwardAccel;
    public float upwardDecel;
    public float xAirDecelMultiplier;
    public float xAirAccelMultiplier;
    public float horizontal { get; private set; }

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    public bool isGrounded { get; private set; } = false;

    [Header("State")]
    [SerializeField] private PlayerStateHandler stateHandler;

    private void Start()
    {
        stateHandler.Initialize(this);
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }
    private void Update()
    {
        AnimationControl();
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

    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion

    private void AnimationControl()
    {
        //flip sprite based on facing direction
        if (horizontal > 0)
        {
            spriteRend.flipX = false;
        }
        else if (horizontal < 0)
        {
            spriteRend.flipX = true;
        }
    }
}
