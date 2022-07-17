using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerMovementHandler : IHandler<PlayerInteractionState>
{
    public void Handle(in PlayerInteractionState data)
    {
        data.EntityMovementSettings.RigidBody.MovePosition(data.sharedData.PlayerTransform.position + data.PlayerState.MoveDirection * data.PlayerState.MoveSpeed * Time.deltaTime + data.PlayerState.Velocity * Time.deltaTime);
    } 
}
