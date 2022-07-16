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
    private Camera mainCamera;
    public InputData inputData;

    private Vector2 screenSize;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraFollow = virtualCamera.Follow;
        mainCamera = Camera.main;

        inputData = inputPipeline.InputData;

        screenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        // Remap range from [ScreenWidth, ScreenHeight] to [-aimLookPercentage, aimLookAmt]
        Vector2 cursorPosRemap = (inputData.CursorPos / screenSize * 2f - Vector2.one) * aimLookAmt;

        print("CursorPosRemap: " + cursorPosRemap);
        cameraFollow.position = new Vector3(
            playerTransform.position.x + cursorPosRemap.x,
            playerTransform.position.y,
            playerTransform.position.z + cursorPosRemap.y);

        //// Convert world space to screen space
        //Vector3 cameraFollowPos = mainCamera.WorldToScreenPoint(playerTransform.position);
        //
        //// Modify the camera position based on the cursor position
        //cameraFollowPos.x += (cursorPos.x - Screen.width / 2) * aimLookPercentage;
        //cameraFollowPos.y += (cursorPos.y - Screen.height / 2) * aimLookPercentage;
        //
        //// Convert back from screen space to world space
        //cameraFollow.position = mainCamera.ScreenToWorldPoint(cameraFollowPos);
    }
}
