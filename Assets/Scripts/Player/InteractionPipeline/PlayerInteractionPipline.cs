using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class PlayerInteraction : MonoBehaviour
{
    // public float WalkingSpeed = 5;
    // public float CrouchSpeed = 3;
    // public float DodgeSpeed = 20;
    // public float DodgeTime = 1f;
    // public float DodgeCooldown = 0.5f;
    // public Vector3 Gravity = new Vector3(0f, -9.8f, 0f);

    [SerializeField]
    private InteractionPipeline<PlayerInteractionState> pipeline;

    public void Start()
    {
        pipeline = new InteractionPipeline<PlayerInteractionState>(new PlayerInteractionState());

        //Get InputReader GameObj
        //Add InputReader GameObj to pipeline => (pipeline.AddGenerator(inputreader))
        //Make GravityMovments and Add to pipeline => (pipeline.AddGenerator(gravityMovments))
        //Make PlayerMovement and Add to pipeline => (pipeline.AddGenerator(playerMovement))
        //Make PlayerController and Add to pipeline => (pipeline.AddHandler(playerController))
        //      PlayerController will read data and get unity to execute the right actions
    }

    public void Update()
    {
        pipeline.Execute();
    }
}