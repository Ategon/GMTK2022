using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using Cinemachine;

public class PlayerInteractionPipline : MonoBehaviour
{
    [SerializeField]
    PlayerInteractionState initialPlayerState;

    [SerializeField]
    private InteractionPipeline<PlayerInteractionState> pipeline;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        // TOOD (GnoxNahte?): quick fixfor now, might do something different.
        // maybe for some variables just assign directly in inspector
        initialPlayerState.EntityMovementSettings.RigidBody = GetComponent<Rigidbody>();

        initialPlayerState.PlayerCameraState.VirtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        initialPlayerState.PlayerCameraState.CameraFollow = initialPlayerState.PlayerCameraState.VirtualCamera.Follow;
        initialPlayerState.PlayerCameraState.ScreenSize = new Vector2(Screen.width, Screen.height);

        initialPlayerState.PlayerAttackState.dicePool = new ObjectPool();
        initialPlayerState.PlayerAttackState.dicePool.InitPool("DicePool", initialPlayerState.PlayerAttackSettings.DicePrefab, 30);

        //initialPlayerState.PlayerAttackState.equippedPowerups = new PowerupSettings[DiceAttackSettings.numOfSides];
        //initialPlayerState.PlayerAttackState.equippedPowerups[0] =
        //    initialPlayerState.PlayerAttackSettings.Powerups[initialPlayerState.PlayerAttackSettings.StartingPowerupIndex];

        foreach (PowerupSettings powerupSetting in initialPlayerState.PlayerAttackState.equippedPowerups)
            powerupSetting.ifEnabled = false;

        initialPlayerState.sharedData.PlayerTransform = transform;
        initialPlayerState.sharedData.MainCamera = Camera.main;
        initialPlayerState.sharedData.VirtualCamera = virtualCamera;
    }

    public void Start()
    {
        pipeline = new InteractionPipeline<PlayerInteractionState>(initialPlayerState);

        InputReader inputReader = GetComponent<InputReader>();
        PlayerStatsModifierGenerator playerStatsModifierGenerator = GetComponent<PlayerStatsModifierGenerator>();
        PlayerDiceBuilderGenerator playerDiceBuilderGenerator = GetComponent<PlayerDiceBuilderGenerator>();

        PlayerVisuals playerVisuals = transform.Find("Visuals").GetComponent<PlayerVisuals>();

        pipeline.AddGenerator(inputReader);
        pipeline.AddGenerator(new GravityMovments());
        pipeline.AddGenerator(new PlayerMovementGenerator());
        pipeline.AddGenerator(new PlayerAttackGenerator());
        pipeline.AddGenerator(playerDiceBuilderGenerator);
        pipeline.AddGenerator(playerStatsModifierGenerator);

        pipeline.AddHandler(new PlayerMovementHandler());
        pipeline.AddHandler(new PlayerCameraHandler());
        pipeline.AddHandler(new PlayerAttackHandler());
        pipeline.AddHandler(playerVisuals.getHandler());
    }

    public void FixedUpdate()
    {
        pipeline.Execute();
    }
}