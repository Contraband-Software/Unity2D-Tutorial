using UnityEngine;

public interface IStateCollisionHandler
{
    void OnTriggerEnter2D(PlayerStateHandler stateHandler, Collider2D collision);

    void OnCollisionEnter2D(PlayerStateHandler stateHandler, Collision2D collision);
}
