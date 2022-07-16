using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class PlayerInteractionPipline : MonoBehaviour
{
    [SerializeField]
    PlayerInteractionState startPlayerState;

    [SerializeField]
    private InteractionPipeline<PlayerInteractionState> pipeline;

    private void Awake()
    {
        startPlayerState.EntityMovementSettings.CharacterController = GetComponent<CharacterController>();
    }

    public void Start()
    {
        pipeline = new InteractionPipeline<PlayerInteractionState>(startPlayerState);

        InputReader inputReader = GetComponent<InputReader>();

        pipeline.AddGenerator(inputReader);
        pipeline.AddGenerator(new GravityMovments());
        pipeline.AddGenerator(new PlayerMovementGenerator());

        pipeline.AddHandler(new PlayerMovementHandler());
    }

    public void Update()
    {
        pipeline.Execute();
    }
}