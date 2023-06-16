using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store all information states need in the StateHandler rather than
/// passing all information to each state.
/// </summary>

public class PlayerStateHandler : StateHandler
{
    PlayerBaseState currentState;
    //reference to player controller
    public PlayerController pCon;

    //Define all states
    public IndyIdle idleState { get; private set; }
    public IndyRun runState { get; private set; }
    public IndyJump jumpState { get; private set; }
    public IndyFall fallState { get; private set; }
    public IndySlide slideState { get; private set; }

    //cooldowns
    public bool slideOnCooldown { get; private set; } = false;

    public void Initialize(PlayerController playerController)
    {
        pCon = playerController;
        idleState = new IndyIdle(pCon);
        runState = new IndyRun(pCon);
        jumpState = new IndyJump(pCon);
        fallState = new IndyFall(pCon);
        slideState = new IndySlide(pCon);
        currentState = idleState;
        currentState.EnterState(this);
    }


    void Update()
    {
        currentState.UpdateState(this);
        AnimationControl();
    }
    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
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
        currentState.Jump(this);
    }
    public void HandleSlide()
    {
        currentState.Slide(this);
    }

    public void HandleSlideCancel()
    {
        currentState.SlideCancel(this);
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
