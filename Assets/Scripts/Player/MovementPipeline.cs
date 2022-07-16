using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPipeline : IPipelineBehaviour
{
    public MovementSettings settings;

    private InputPipeline inputPipeline;

    private MovementHandler movementHandler;
    private State currState;

    public enum MoveState
    {
        Normal,
        Crouch,
        Dodging,
    }

    // TODO: Not really sure what to call this class
    // Values that will change 
    public class State : IInteractionData
    {
        public Vector3 moveDirection = Vector3.zero;
         
        public MoveState moveState = MoveState.Normal;
        public float timeLeftInCurrState = 0f;
         
        public float lastDodgedTime = 0f;
         
        public Vector3 velocity = Vector3.zero;
        public bool isGrounded = true;
    }

    private void Start()
    {
        movementHandler = new MovementHandler();
        currState = new State();

        inputPipeline = GetComponent<InputPipeline>();

        settings.characterController = GetComponent<CharacterController>();

        data.Add("MovementSettings", settings);
        data.Add("InputData", inputPipeline.InputData);
        data.Add("CurrState", currState);

        movementHandler.Init(data);
    }

    private void Update()
    {
        movementHandler.Handle();
    }
}
