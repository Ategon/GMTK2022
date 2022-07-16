using System;

[Serializable]
public struct PlayerInteractionState
{
    public PlayerState PlayerState;
    public EntityMovementSettings EntityMovementSettings;
    public PlayerCameraState PlayerCameraState;

    public PlayerAttackState PlayerAttackState;
    public PlayerAttackSettings PlayerAttackSettings;
    public DiceAttackSettings DiceAttackSettings;

    public SharedData sharedData;

    // General shared data, not specific to a handler / generator
    public struct SharedData
    {
        public UnityEngine.Transform PlayerTransform;
        public UnityEngine.Camera MainCamera;
    }
}