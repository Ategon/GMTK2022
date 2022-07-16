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
        // TOOD (GnoxNahte?): quick fixfor now, might do something different.
        // maybe for some variables just assign directly in inspector
        initialPlayerState.EntityMovementSettings.CharacterController = GetComponent<CharacterController>();

        initialPlayerState.PlayerCameraState.VirtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        initialPlayerState.PlayerCameraState.CameraFollow = initialPlayerState.PlayerCameraState.VirtualCamera.Follow;
        initialPlayerState.PlayerCameraState.ScreenSize = new Vector2(Screen.width, Screen.height);

        initialPlayerState.PlayerAttackState.dicePool = new ObjectPool();
        initialPlayerState.PlayerAttackState.dicePool.InitPool("DicePool", initialPlayerState.PlayerAttackSettings.DicePrefab, 30);

        initialPlayerState.PlayerAttackState.equippedEffects = new DiceEffectSettings[DiceSettingsOld.numOfSides];
        initialPlayerState.PlayerAttackState.equippedEffects[0] =
            initialPlayerState.PlayerAttackSettings.DiceEffects[initialPlayerState.PlayerAttackSettings.StartingDiceEffectIndex];


        initialPlayerState.sharedData.PlayerTransform = transform;
        initialPlayerState.sharedData.MainCamera = Camera.main;
    }

    public void Start()
    {
        pipeline = new InteractionPipeline<PlayerInteractionState>(initialPlayerState);

        InputReader inputReader = GetComponent<InputReader>();

        pipeline.AddGenerator(inputReader);
        pipeline.AddGenerator(new GravityMovments());
        pipeline.AddGenerator(new PlayerMovementGenerator());
        pipeline.AddGenerator(new PlayerAttackGenerator());

        pipeline.AddHandler(new PlayerMovementHandler());
        pipeline.AddHandler(new PlayerCameraHandler());
        pipeline.AddHandler(new PlayerAttackHandler());
    }

    public void Update()
    {
        pipeline.Execute();
    }
}