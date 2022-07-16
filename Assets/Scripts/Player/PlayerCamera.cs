using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] InputPipeline inputPipeline;
    [SerializeField] Transform playerTransform;
    [Tooltip("Percentage of the screen that the camera will move based on the mouse position")]
    [SerializeField] float aimLookAmt = 0.2f;

    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraFollow;
    private InputData inputData;

    private Vector2 screenSize;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraFollow = virtualCamera.Follow;

        inputData = inputPipeline.InputData;

        screenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        // Remap range from [ScreenWidth, ScreenHeight] to [-aimLookPercentage, aimLookAmt]
        Vector2 cursorPosRemap = (inputData.CursorPos / screenSize * 2f - Vector2.one) * aimLookAmt;

        cameraFollow.position = new Vector3(
            playerTransform.position.x + cursorPosRemap.x,
            playerTransform.position.y,
            playerTransform.position.z + cursorPosRemap.y);
    }
}
