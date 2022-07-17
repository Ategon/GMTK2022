using UnityEngine;
using UnityEngine.InputSystem;
using DataPipeline;

public class GravityMovments : IGenerator<PlayerInteractionState>
{
    public void Start()
    {

    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        HandleGravity(ref data);
    }

    private void HandleGravity(ref PlayerInteractionState data)
    {
        data.PlayerState.IsGrounded = Physics.CheckSphere(data.EntityMovementSettings.GroundCheck.position, data.EntityMovementSettings.GroundDist, data.EntityMovementSettings.GroundMask);

        if (data.PlayerState.IsGrounded && data.PlayerState.Velocity.y < 0f)
            data.PlayerState.Velocity.y = -2f; // Let the player fall some more since player is still slightly above the ground
        else
            data.PlayerState.Velocity += data.EntityMovementSettings.Gravity * Time.deltaTime;
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }
}