using System;
using UnityEngine;

[Serializable]
public struct PlayerState
{
    public Vector2 Move;
    public Vector2 Aim;
    public Vector2 CursorPos;
    public bool Dodge;
    public bool Reload;
    public bool Crouch;
    public bool Fire;
    public bool Pause;
    public bool IsGrounded;

    public Vector3 MoveDirection;

    public MoveState MoveState;
    public float TimeLeftInCurrState;

    public float LastDodgedTime;

    public Vector3 Velocity;
}

public enum MoveState
{
    Normal,
    Crouch,
    Dodging,
}