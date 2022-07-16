using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : IHandler
{
    MovementSettings moveSettings;
    MovementPipeline.State currState;
    InputData inputData;

    public void Init(Dictionary<string, IInteractionData> data)
    {
        moveSettings = (MovementSettings)data["MovementSettings"];
        currState = (MovementPipeline.State)data["CurrState"];
        inputData = (InputData)data["InputData"];
    }

    public void Handle()
    {
        HandleGravity();
        HandleStates();

        // Modify the move speed based on the move state
        float moveSpeed;
        switch (currState.moveState)
        {
            case MovementPipeline.MoveState.Dodging:
                // Lerp the dodge and walking speed based on the time left on the dodge
                moveSpeed = moveSettings.dodgeSpeed;
                break;
            case MovementPipeline.MoveState.Crouch:
                moveSpeed = moveSettings.crouchSpeed;
                break;
            case MovementPipeline.MoveState.Normal:
            default:
                moveSpeed = moveSettings.walkingSpeed;
                break;
        }

        currState.moveDirection = new Vector3(inputData.Move.x, 0, inputData.Move.y);

        moveSettings.characterController.Move(currState.moveDirection * moveSpeed * Time.deltaTime + currState.velocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        currState.isGrounded = Physics.CheckSphere(moveSettings.groundCheck.position, moveSettings.groundDist, moveSettings.groundMask);

        if (currState.isGrounded && currState.velocity.y < 0f)
            currState.velocity.y = -2f; // Let the player fall some more since player is still slightly above the ground
        else
            currState.velocity += moveSettings.gravity * Time.deltaTime;
    }

    private void HandleStates()
    {
        if (inputData.Dodge && Time.time - currState.lastDodgedTime >= moveSettings.dodgeCooldown)
        {
            currState.moveState = MovementPipeline.MoveState.Dodging;
            currState.timeLeftInCurrState = moveSettings.dodgeTime;
            currState.lastDodgedTime = Time.time;
        }
        else if (inputData.Crouch)
        {
            currState.moveState = MovementPipeline.MoveState.Crouch;
        }

        // Check if the time left for the current state has ended
        currState.timeLeftInCurrState -= Time.deltaTime;
        if (currState.timeLeftInCurrState <= 0f)
        {
            currState.moveState = MovementPipeline.MoveState.Normal;
        }
    }

}
