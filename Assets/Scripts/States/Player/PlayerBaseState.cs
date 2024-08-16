using UnityEngine;
using Software.Contraband.StateMachines;

public abstract class PlayerBaseState : BaseState,
    IPlayerInputHandler,
    IStateCollisionHandler
{
    protected PlayerStateHandler stateHandler;

    protected PlayerBaseState(PlayerStateHandler stateHandler)
    {
        this.stateHandler = stateHandler;
    }

    /// <summary>
    /// What you do in the state per frame
    /// </summary>
    public abstract void UpdateState();

    /// <summary>
    /// What you do in the state per fixed frame, 
    /// Reserved for physics essentially
    /// </summary>
    /// <param name="stateHandler"></param>
    public abstract void FixedUpdateState();

    /// <summary>
    /// Allows handling a jump input event
    /// </summary>
    /// <param name="stateHandler"></param>
    public virtual void Jump()
    {
    }

    public virtual void Slide()
    {

    }

    public virtual void SlideCancel ()
    {

    }

    public virtual void RopeDetach()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
