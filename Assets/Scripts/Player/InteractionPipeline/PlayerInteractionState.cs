using System;
using DataPipeline;
using UnityEngine;

[Serializable]
public struct PlayerInteractionState : IData
{
    public double deltaTime;
    public float stunBoostTime;
    [ReadOnly] public PlayerState PlayerState;
    public EntityMovementSettings EntityMovementSettings;
    [ReadOnly] public PlayerMovementStatsModifier PlayerMovementStatsModifier;

    public PlayerCameraState PlayerCameraState;

    public PlayerAttackState PlayerAttackState;
    public PlayerAttackSettings PlayerAttackSettings;
    public DiceAttackSettings DiceAttackSettings;
    public PlayerAttackStatsModifier PlayerAttackStatsModifier;

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
        public GameAudio GameAudio;
        public double deltaTime;
    }
}