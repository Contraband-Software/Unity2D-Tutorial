using UnityEngine;
public interface IPlayerInputHandler
{
    void Jump(PlayerStateHandler stateHandler);

    void Slide(PlayerStateHandler stateHandler);

    void SlideCancel(PlayerStateHandler stateHandler);

    void RopeDetach(PlayerStateHandler stateHandler);
}
