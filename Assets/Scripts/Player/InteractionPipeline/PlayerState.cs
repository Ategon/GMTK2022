using System;
using UnityEngine;
using DataPipeline;

[Serializable]
public struct PlayerState : IData
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

    public float MoveSpeed;

    public void SetDifference(in PlayerState state)
    {
        if (Move != state.Move) Move = state.Move;
        if (Aim != state.Aim) Aim = state.Aim;
        if (CursorPos != state.CursorPos) CursorPos = state.CursorPos;
        if (Dodge != state.Dodge) Dodge = state.Dodge;
        if (Reload != state.Reload) Reload = state.Reload;
        if (Crouch != state.Crouch) Crouch = state.Crouch;
        if (Fire != state.Fire) Fire = state.Fire;
        if (Pause != state.Pause) Pause = state.Pause;
        if (IsGrounded != state.IsGrounded) IsGrounded = state.IsGrounded;
        if (MoveDirection != state.MoveDirection) MoveDirection = state.MoveDirection;
        if (MoveState != state.MoveState) MoveState = state.MoveState;
        if (TimeLeftInCurrState != state.TimeLeftInCurrState) TimeLeftInCurrState = state.TimeLeftInCurrState;
        if (LastDodgedTime != state.LastDodgedTime) LastDodgedTime = state.LastDodgedTime;
        if (Velocity != state.Velocity) Velocity = state.Velocity;
        if (MoveSpeed != state.MoveSpeed) MoveSpeed = state.MoveSpeed;
    }

    public void Clear()
    {

    }
}

public enum MoveState
{
    Normal,
    Crouch,
    Dodging,
}