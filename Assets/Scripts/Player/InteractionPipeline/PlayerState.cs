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

    public Vector3 dodgeDirection;
    public float LastDodgedTime;

    public Vector3 Velocity;

    public Vector3 position;

    public float MoveSpeed;

    public void SetDirty(in PlayerState state, in Dirty dirty)
    {
        if (dirty.Move) Move = state.Move;
        if (dirty.Aim) Aim = state.Aim;
        if (dirty.CursorPos) CursorPos = state.CursorPos;
        if (dirty.Dodge) Dodge = state.Dodge;
        if (dirty.Reload) Reload = state.Reload;
        if (dirty.Crouch) Crouch = state.Crouch;
        if (dirty.Fire) Fire = state.Fire;
        if (dirty.Pause) Pause = state.Pause;
        if (dirty.IsGrounded) IsGrounded = state.IsGrounded;
        if (dirty.MoveDirection) MoveDirection = state.MoveDirection;
        if (dirty.MoveState) MoveState = state.MoveState;
        if (dirty.TimeLeftInCurrState) TimeLeftInCurrState = state.TimeLeftInCurrState;
        if (dirty.LastDodgedTime) LastDodgedTime = state.LastDodgedTime;
        if (dirty.Velocity) Velocity = state.Velocity;
        if (dirty.MoveSpeed) MoveSpeed = state.MoveSpeed;
    }

    public void Clear()
    {

    }

    public struct Dirty
    {
        public bool Fire;
        public bool Reload;
        public bool Crouch;
        public bool Dodge;
        public bool Move;
        public bool Aim;
        public bool CursorPos;
        public bool Pause;
        public bool IsGrounded;
        public bool MoveDirection;
        public bool MoveState;
        public bool TimeLeftInCurrState;
        public bool LastDodgedTime;
        public bool Velocity;
        public bool MoveSpeed;

        public void Reset()
        {
            Fire = false;
            Reload = false;
            Crouch = false;
            Dodge = false;
            Move = false;
            Aim = false;
            CursorPos = false;
            Pause = false;
            IsGrounded = false;
            MoveDirection = false;
            MoveState = false;
            TimeLeftInCurrState = false;
            LastDodgedTime = false;
            Velocity = false;
            MoveSpeed = false;
        }
    }
}

public enum MoveState
{
    Undefined,
    Normal,
    Crouch,
    Dodging,
}