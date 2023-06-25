using UnityEngine;

public abstract class BaseState
{

    /// <summary>
    /// What you do when you first transition to this state
    /// </summary>
    /// <param name="stateHandler"></param>
    public abstract void EnterState();
    /// <summary>
    /// What you do right before switching to another state
    /// </summary>
    /// <param name="stateHandler"></param>
    public abstract void ExitState();
}
