using UnityEngine;

public abstract class PlayerBaseState : IPlayerInputHandler
{
    protected PlayerController pCon;
    /// <summary>
    /// What you do when you first transition to this state
    /// </summary>
    /// <param name="stateHandler"></param>
    public abstract void EnterState(PlayerStateHandler stateHandler);

    /// <summary>
    /// What you do in the state per frame
    /// </summary>
    public abstract void UpdateState(PlayerStateHandler stateHandler);

    /// <summary>
    /// What you do in the state per fixed frame, 
    /// Reserved for physics essentially
    /// </summary>
    /// <param name="stateHandler"></param>
    public abstract void FixedUpdateState(PlayerStateHandler stateHandler);

    public virtual void Jump(PlayerStateHandler stateHandler)
    {
    }
}
