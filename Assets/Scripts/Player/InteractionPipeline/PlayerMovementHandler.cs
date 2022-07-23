using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerMovementHandler : IHandler<PlayerInteractionState>
{
    float footstepsDiff = 0.5f;
    float timer = 0;

    public void Handle(in PlayerInteractionState data)
    {
        timer += (float) data.deltaTime;

        if(timer > 0.5f && data.PlayerState.MoveDirection.x != 0 && data.PlayerState.MoveDirection.z != 0)
        {
            data.sharedData.GameAudio.PlaySound("Footsteps", AudioTrackType.Footsteps);
            timer = 0;
        }

        data.EntityMovementSettings.RigidBody.MovePosition(data.sharedData.PlayerTransform.position + data.PlayerState.MoveDirection * data.PlayerState.MoveSpeed * Time.deltaTime * (1 + data.stunBoostTime) + data.PlayerState.Velocity * Time.deltaTime);
    } 
}
