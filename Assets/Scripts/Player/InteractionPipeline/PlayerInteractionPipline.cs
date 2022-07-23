using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using Cinemachine;

public class PlayerInteractionPipline : MonoBehaviour
{
    [SerializeField]
    PlayerInteractionState playerState;

    [SerializeField]
    private InteractionPipeline<PlayerInteractionState> pipeline;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private GameAudio gameAudio;

    private void Awake()
    {
        // TOOD (GnoxNahte?): quick fixfor now, might do something different.
        // maybe for some variables just assign directly in inspector
        playerState.EntityMovementSettings.RigidBody = GetComponent<Rigidbody>();

        playerState.PlayerCameraState.VirtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        playerState.PlayerCameraState.CameraFollow = playerState.PlayerCameraState.VirtualCamera.Follow;
        playerState.PlayerCameraState.ScreenSize = new Vector2(Screen.width, Screen.height);

        playerState.PlayerAttackState.dicePool = new ObjectPool();
        playerState.PlayerAttackState.dicePool.InitPool("DicePool", playerState.PlayerAttackSettings.DicePrefab, 30);

        //initialPlayerState.PlayerAttackState.equippedPowerups = new PowerupSettings[DiceAttackSettings.numOfSides];
        //initialPlayerState.PlayerAttackState.equippedPowerups[0] =
        //    initialPlayerState.PlayerAttackSettings.Powerups[initialPlayerState.PlayerAttackSettings.StartingPowerupIndex];

        foreach (PowerupSettings powerupSetting in playerState.PlayerAttackState.equippedPowerups)
            powerupSetting.ifEnabled = false;

        playerState.sharedData.PlayerTransform = transform;
        playerState.sharedData.MainCamera = Camera.main;
        playerState.sharedData.VirtualCamera = virtualCamera;
        playerState.sharedData.GameAudio = gameAudio;
    }

    public void Start()
    {
        pipeline = new InteractionPipeline<PlayerInteractionState>();

        InputReader inputReader = GetComponent<InputReader>();
        PlayerStatsModifierGenerator playerAttackPowerupGenerator = GetComponentInChildren<PlayerStatsModifierGenerator>();

        PlayerVisuals playerVisuals = transform.Find("Visuals").GetComponent<PlayerVisuals>();

        pipeline.AddGenerator(inputReader);
        pipeline.AddGenerator(new GravityMovments());
        pipeline.AddGenerator(new PlayerMovementGenerator());
        pipeline.AddGenerator(new PlayerAttackGenerator());
        pipeline.AddGenerator(playerAttackPowerupGenerator);

        pipeline.AddHandler(new PlayerMovementHandler());
        pipeline.AddHandler(new PlayerCameraHandler());
        pipeline.AddHandler(new PlayerAttackHandler());
        pipeline.AddHandler(playerVisuals.getHandler());
    }

    public void FixedUpdate()
    {
        playerState.deltaTime = Time.deltaTime;
        pipeline.HandleData(in playerState);
        pipeline.WriteData(ref playerState);
    }
}