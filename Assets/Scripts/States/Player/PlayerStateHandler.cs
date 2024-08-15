using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store all information states need in the StateHandler rather than
/// passing all information to each state.
/// </summary>
/// 
[System.Serializable]
public struct ColliderSize
{
    public Vector2 offset;
    public Vector2 size;
}
public class PlayerStateHandler : StateHandler<PlayerBaseState>
{
    //reference to player controller
    public PlayerController pCon;
    public BoxCollider2D col;

    //Define all states
    public IndyIdle idleState { get; private set; }
    public IndyRun runState { get; private set; }
    public IndyJump jumpState { get; private set; }
    public IndyFall fallState { get; private set; }
    public IndySlide slideState { get; private set; }
    public IndyRope ropeState { get; private set; }

    //collider sizes for each state
    [Header("State Collider Sizes")]
    public ColliderSize idleCollider;
    public ColliderSize slideCollider;

    //cooldowns
    public bool slideOnCooldown { get; private set; } = false;

    public void Initialize(PlayerController playerController)
    {
        pCon = playerController;
        col = playerController.gameObject.GetComponent<BoxCollider2D>();
        idleState = new IndyIdle(pCon, this);
        runState = new IndyRun(pCon, this);
        jumpState = new IndyJump(pCon, this);
        fallState = new IndyFall(pCon, this);
        slideState = new IndySlide(pCon, this);
        ropeState = new IndyRope(pCon, this);
        currentState = idleState;
        currentState.EnterState();
    }


    void Update()
    {
        currentState.UpdateState();
        AnimationControl();
    }
    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public override void SwitchState(PlayerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    private void AnimationControl()
    {
        if (currentState != slideState)
        {
            //flip sprite based on facing direction
            if (pCon.horizontal > 0)
            {
                pCon.spriteRend.flipX = false;
            }
            else if (pCon.horizontal < 0)
            {
                pCon.spriteRend.flipX = true;
            }
        }
    }

    #region PLAYER_CONTROL_INTERFACE
    public void HandleJump()
    {
        currentState.Jump();
    }
    public void HandleSlide()
    {
        currentState.Slide();
    }

    public void HandleSlideCancel()
    {
        currentState.SlideCancel();
    }

    public void HandleTriggerEnter(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(collision);
    }

    public void HandleCollisionEnter(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }

    public void HandleRopeDetach()
    {
        currentState.RopeDetach();
    }
    #endregion

    #region COOLDOWNS
    public void StartSlideCooldown()
    {
        StartCoroutine(SlideCooldown(pCon.slideCooldown));
    }

    private IEnumerator SlideCooldown(float cooldownTime)
    {
        slideOnCooldown = true;

        yield return new WaitForSeconds(cooldownTime);

        slideOnCooldown = false;
    }
    #endregion
}
