using UnityEngine;

namespace Software.Contraband.StateMachines
{
    /// <summary>
    /// A StateHandler that handles states of a certain type inherited from BaseState
    /// </summary>
    public abstract class StateHandler<T> : MonoBehaviour where T : BaseState
    {
        protected T currentState;

        public abstract void SwitchState(T newState);
    }
}
