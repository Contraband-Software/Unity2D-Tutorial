using UnityEngine;

public interface IStateCollisionHandler<T> where T : StateHandler
{
    void OnTriggerEnter2D(T stateHandler, Collider2D collision);
}
