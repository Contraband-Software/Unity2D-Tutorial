using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Software.Contraband.StateMachines;

/// <summary>
/// Store all information states need in the StateHandler rather than
/// passing all information to each state.
/// </summary>
/// 
[Serializable]
public struct ColliderSize
{
    public Vector2 offset;
    public Vector2 size;
}

[
    RequireComponent(typeof(BoxCollider2D)),
    RequireComponent(typeof(PlayerController))
]
public class PlayerStateHandler : StateHandler<PlayerBaseState>
{
    //reference to player controller
    public PlayerController pCon;
    public BoxCollider2D col;

    //collider sizes for each state
    [Header("State Collider Sizes")]
    public ColliderSize idleCollider;
    public ColliderSize slideCollider;

    //cooldowns
    public bool slideOnCooldown { get; private set; } = false;

    protected override void Initialize()
    {
        pCon = GetComponent<PlayerController>();
        col = pCon.gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CurrentState.UpdateState();
        AnimationControl();
    }
    void FixedUpdate()
    {
        CurrentState.FixedUpdateState();
    }

    public override void SwitchState(PlayerBaseState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    private void AnimationControl()
    {
        if (CurrentState != States[typeof(IndySlide)])
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
        CurrentState.Jump();
    }
    public void HandleSlide()
    {
        CurrentState.Slide();
    }

    public void HandleSlideCancel()
    {
        CurrentState.SlideCancel();
    }

    public void HandleTriggerEnter(Collider2D collision)
    {
        CurrentState.OnTriggerEnter2D(collision);
    }

    public void HandleCollisionEnter(Collision2D collision)
    {
        CurrentState.OnCollisionEnter2D(collision);
    }

    public void HandleRopeDetach()
    {
        CurrentState.RopeDetach();
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
