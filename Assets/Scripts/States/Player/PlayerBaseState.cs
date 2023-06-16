using UnityEngine;

public abstract class PlayerBaseState : BaseState<PlayerStateHandler>, IPlayerInputHandler
{
    protected PlayerController pCon;

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

    /// <summary>
    /// Allows handling a jump input event
    /// </summary>
    /// <param name="stateHandler"></param>
    public virtual void Jump(PlayerStateHandler stateHandler)
    {
    }

    public virtual void Slide(PlayerStateHandler stateHandler)
    {

    }

    public virtual void SlideCancel (PlayerStateHandler stateHandler)
    {

    }
}
