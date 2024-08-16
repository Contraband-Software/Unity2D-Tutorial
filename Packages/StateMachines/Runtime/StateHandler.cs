using UnityEngine;
using System;
using Assembly = System.Reflection.Assembly;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Software.Contraband.StateMachines
{
    /// <summary>
    /// A StateHandler that handles states of a certain type inherited from BaseState
    /// </summary>
    public abstract class StateHandler<T> : MonoBehaviour where T : BaseState
    {
        public Dictionary<Type, T> States { get; protected set; } = new();
        protected T CurrentState { get; set; }

        protected void Awake()
        {
            var states = Assembly.GetAssembly(this.GetType()).GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(T)));
            
            foreach (var type in states)
            {
                T s = Activator.CreateInstance(type, new object[] { this }) as T;
                States.Add(s.GetType(), s);
            }
        
            // Starting state
            CurrentState = States.Values.First(s => s.GetStateInfo.HasFlag(StateType.Default));
            CurrentState.EnterState();
            
            Initialize();
        }

        protected abstract void Initialize();
        public abstract void SwitchState(T newState);
    }
}


