using System;

[Serializable]
public struct PlayerInteractionState
{
    public PlayerState PlayerState;
    public EntityMovementSettings EntityMovementSettings;
    public PlayerCameraState PlayerCameraState;

    // Other general stuff, not specific to a handler / generator
    public UnityEngine.Transform PlayerTransform;
}