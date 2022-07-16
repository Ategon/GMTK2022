using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class PlayerInteractionPipline : MonoBehaviour
{
    [SerializeField]
    PlayerInteractionState initialPlayerState;

    [SerializeField]
    private InteractionPipeline<PlayerInteractionState> pipeline;

    private void Awake()
    {
        initialPlayerState.EntityMovementSettings.CharacterController = GetComponent<CharacterController>();

        // TOOD (GnoxNahte): quick fix, might do something diff
        initialPlayerState.PlayerCameraState.VirtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        initialPlayerState.PlayerCameraState.CameraFollow = initialPlayerState.PlayerCameraState.VirtualCamera.Follow;
        initialPlayerState.PlayerCameraState.ScreenSize = new Vector2(Screen.width, Screen.height);
    }

    public void Start()
    {
        pipeline = new InteractionPipeline<PlayerInteractionState>(initialPlayerState);

        InputReader inputReader = GetComponent<InputReader>();

        pipeline.AddGenerator(inputReader);
        pipeline.AddGenerator(new GravityMovments());
        pipeline.AddGenerator(new PlayerMovementGenerator());

        pipeline.AddHandler(new PlayerMovementHandler());
        pipeline.AddHandler(new PlayerCameraHandler());
    }

    public void Update()
    {
        pipeline.Execute();
    }
}