using System;
using DataPipeline;

[Serializable]
public struct PlayerInteractionState : IData
{
    [ReadOnly]
    public PlayerState PlayerState;
    public EntityMovementSettings EntityMovementSettings;
    public PlayerCameraState PlayerCameraState;

    [ReadOnly]
    public PlayerAttackState PlayerAttackState;
    public PlayerAttackSettings PlayerAttackSettings;
    public DiceAttackSettings DiceAttackSettings;

    public SharedData sharedData;

    public void Clear()
    {

    }

    // General shared data, not specific to a handler / generator
    public struct SharedData
    {
        public UnityEngine.Transform PlayerTransform;
        public UnityEngine.Camera MainCamera;
        public Cinemachine.CinemachineVirtualCamera VirtualCamera;
    }
}