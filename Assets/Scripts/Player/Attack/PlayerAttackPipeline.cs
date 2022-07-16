using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPipeline : IPipelineBehaviour
{
    [SerializeField] PlayerAttackSettings settings;
    [SerializeField] DiceSettings diceSettings;

    private InputPipeline inputPipeline;

    private PlayerAttackHandler attackHandler;
    private State currState;

    public class State : IInteractionData
    {
        public DiceEffect[] effects = new DiceEffect[6];
        public float lastAttackedTime = 0f;
    }

    private void Start()
    {
        attackHandler = new PlayerAttackHandler();
        currState = new State();

        inputPipeline = GetComponent<InputPipeline>();

        data.Add("AttackSettings", settings); 
        data.Add("DiceSettings", diceSettings); 
        data.Add("InputData", inputPipeline.InputData);
        data.Add("CurrState", currState);

        attackHandler.Init(data);
    }

    private void Update()
    {
        attackHandler.Handle();
    }
}
