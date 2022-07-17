using System;
using UnityEngine;
using DataPipeline;

using Cinemachine;

[Serializable]
public struct PlayerCameraState : IData
{
    [Tooltip("Percentage of the screen that the camera will move based on the mouse position")]
    public float AimLookAmt;

    public CinemachineVirtualCamera VirtualCamera;
    public Transform CameraFollow;

    public Vector2 ScreenSize;

    public void Clear()
    {

    }
}