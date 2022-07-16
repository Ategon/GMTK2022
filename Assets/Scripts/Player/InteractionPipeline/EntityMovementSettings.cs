using System;
using UnityEngine;

[Serializable]
public struct EntityMovementSettings
{
    [HideInInspector] public CharacterController CharacterController;
    public float WalkingSpeed;
    public float CrouchSpeed;
    public float DodgeSpeed;
    public float DodgeTime;
    public float DodgeCooldown;

    public Vector3 Gravity;

    public Transform GroundCheck;
    public LayerMask GroundMask;
    public float GroundDist;
}