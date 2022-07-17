using System;
using UnityEngine;
using DataPipeline;

[Serializable]
public struct EntityMovementSettings : IData
{
    [HideInInspector] public Rigidbody RigidBody;
    public float WalkingSpeed;
    public float CrouchSpeed;
    public float DodgeSpeed;
    public float DodgeTime;
    public float DodgeCooldown;

    public Vector3 Gravity;

    public Transform GroundCheck;
    public LayerMask GroundMask;
    public float GroundDist;

    public void Clear()
    {

    }
}
