using UnityEngine;

public interface IStateCollisionHandler<T> where T : StateHandler
{
    void OnTriggerEnter2D(T stateHandler, Collider2D collision);

    void OnCollisionEnter2D(T stateHandler, Collision2D collision);
}
