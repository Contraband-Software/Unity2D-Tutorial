using UnityEngine;

public abstract class PlayerBaseState : BaseState,
    IPlayerInputHandler,
    IStateCollisionHandler
{
    protected PlayerController pCon;

    protected PlayerBaseState(PlayerController pCon)
    {
        this.pCon = pCon;
    }

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

    public virtual void RopeDetach(PlayerStateHandler stateHandler)
    {

    }

    public virtual void OnTriggerEnter2D(PlayerStateHandler stateHandler, Collider2D collision)
    {
    }

    public virtual void OnCollisionEnter2D(PlayerStateHandler stateHandler, Collision2D collision)
    {
    }
}
