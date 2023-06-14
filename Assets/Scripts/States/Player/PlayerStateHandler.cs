using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store all information states need in the StateHandler rather than
/// passing all information to each state.
/// </summary>

public class PlayerStateHandler : MonoBehaviour
{
    PlayerBaseState currentState;

    //reference to player controller
    public PlayerController pCon;

    //Define all states
    public IndyIdle idleState { get; private set; }
    public IndyRun runState { get; private set; }
    public IndyJump jumpState { get; private set; }
    public IndyFall fallState { get; private set; }

    public void Initialize(PlayerController playerController)
    {
        pCon = playerController;
        idleState = new IndyIdle(pCon);
        runState = new IndyRun(pCon);
        jumpState = new IndyJump(pCon);
        fallState = new IndyFall(pCon);
        currentState = idleState;
        currentState.EnterState(this);
    }


    void Update()
    {
        currentState.UpdateState(this);
    }
    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    #region PLAYER_CONTROL_INTERFACE
    public void HandleJump()
    {
        currentState.Jump(this);
    }
    #endregion
}
