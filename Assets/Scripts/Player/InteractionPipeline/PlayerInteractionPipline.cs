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

        playerState.PlayerAttackState.dicePools = new Dictionary<int, ObjectPool>();
        DiceBuilder diceBuilder = FindObjectOfType<DiceBuilder>();
        foreach (DiceBuilder.DiceData dice in diceBuilder.diceSelection)
        {
            ObjectPool objectPool = new ObjectPool();
            objectPool.InitPool("DicePool_D" + dice.numSides, dice.dicePrefab, 30);
            playerState.PlayerAttackState.dicePools.Add(dice.numSides, objectPool);
        }

        foreach (PowerupSettings powerupSetting in playerState.PlayerAttackState.equippedPowerups)
            powerupSetting.ifEnabled = false;

        playerState.sharedData.PlayerTransform = transform;
        playerState.sharedData.MainCamera = Camera.main;
        playerState.sharedData.VirtualCamera = virtualCamera;
        playerState.sharedData.GameAudio = gameAudio;

        pipeline = new InteractionPipeline<PlayerInteractionState>(); // moved from start
    }

    public void Start()
    {
        

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
        playerState.deltaTime = Time.deltaTime;
        pipeline.HandleData(in playerState);
        pipeline.WriteData(ref playerState);
    }

    public void AddGenerator(IGenerator<PlayerInteractionState> generator)
    {
        pipeline.AddGenerator(generator);
    }

    public void RemoveGenerator(IGenerator<PlayerInteractionState> generator)
    {
        pipeline.RemoveGenerator(generator);
    }

    public void AddHandler(IHandler<PlayerInteractionState> handler)
    {
        pipeline.AddHandler(handler);
    }
}