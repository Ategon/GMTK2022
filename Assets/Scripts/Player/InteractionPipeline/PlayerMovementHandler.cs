using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;


// Needs to be worked on

public class PlayerMovementHandler : IGenerator<PlayerInteractionState>
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
        switch (data.PlayerState.MoveState)
        {
            case MoveState.Dodging:
                // Lerp the dodge and walking speed based on the time left on the dodge
                moveSpeed = data.EntityMovementSettings.DodgeSpeed;
                break;
            case MoveState.Crouch:
                moveSpeed = data.EntityMovementSettings.CrouchSpeed;
                break;
            case MoveState.Normal:
            default:
                moveSpeed = data.EntityMovementSettings.WalkingSpeed;
                break;
        }

        data.PlayerState.MoveDirection = new Vector3(data.PlayerState.Move.x, 0, data.PlayerState.Move.y);

        //Should be moved to a Handler
        data.EntityMovementSettings.CharacterController.Move(data.PlayerState.MoveDirection * moveSpeed * Time.deltaTime + data.PlayerState.Velocity * Time.deltaTime);
    }



    private void HandleStates(ref PlayerInteractionState data)
    {
        if (data.PlayerState.Dodge && Time.time - data.PlayerState.LastDodgedTime >= data.EntityMovementSettings.DodgeCooldown)
        {
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
