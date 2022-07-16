using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;


// Needs to be worked on

public class PlayerMovementGenerator : IGenerator<PlayerInteractionState>
{

    public void Start()
    {

    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        Handle(ref data);
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }

    public void Handle(ref PlayerInteractionState data)
    {
        HandleStates(ref data);

        // Modify the move speed based on the move state
        float moveSpeed;
        Vector3 moveDirection;
        switch (data.PlayerState.MoveState)
        {
            case MoveState.Dodging:
                moveSpeed = data.EntityMovementSettings.DodgeSpeed;
                moveDirection = data.PlayerState.dodgeDirection;
                break;
            case MoveState.Crouch:
                moveSpeed = data.EntityMovementSettings.CrouchSpeed;
                moveDirection = new Vector3(data.PlayerState.Move.x, 0, data.PlayerState.Move.y);
                break;
            case MoveState.Normal:
            default:
                moveSpeed = data.EntityMovementSettings.WalkingSpeed;
                moveDirection = new Vector3(data.PlayerState.Move.x, 0, data.PlayerState.Move.y);
                break;
        }

        data.PlayerState.MoveDirection = moveDirection;
        data.PlayerState.MoveSpeed = moveSpeed;
    }

    private void HandleStates(ref PlayerInteractionState data)
    {
        if (data.PlayerState.Dodge && Time.time - data.PlayerState.LastDodgedTime >= data.EntityMovementSettings.DodgeCooldown)
        {
            data.PlayerState.dodgeDirection = new Vector3(data.PlayerState.Move.x, 0, data.PlayerState.Move.y);
            data.PlayerState.MoveState = MoveState.Dodging;
            data.PlayerState.TimeLeftInCurrState = data.EntityMovementSettings.DodgeTime;
            data.PlayerState.LastDodgedTime = Time.time;
        }
        else if (data.PlayerState.Crouch)
        {
            data.PlayerState.MoveState = MoveState.Crouch;
        }

        // Check if the time left for the current state has ended
        data.PlayerState.TimeLeftInCurrState -= Time.deltaTime;
        if (data.PlayerState.TimeLeftInCurrState <= 0f)
        {
            data.PlayerState.MoveState = MoveState.Normal;
        }
    }
}
