using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerCameraHandler : IHandler<PlayerInteractionState>
{
    public void Handle(in PlayerInteractionState data)
    {
        // Remap range from [ScreenWidth, ScreenHeight] to [-aimLookPercentage, aimLookAmt]
        Vector2 cursorPosRemap = (data.PlayerState.CursorPos / data.PlayerCameraState.ScreenSize * 2f - Vector2.one) * data.PlayerCameraState.AimLookAmt;

        data.PlayerCameraState.CameraFollow.position = new Vector3(
            data.sharedData.PlayerTransform.position.x + cursorPosRemap.x,
            data.sharedData.PlayerTransform.position.y,
            data.sharedData.PlayerTransform.position.z + cursorPosRemap.y);
    }
}
