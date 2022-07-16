using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPipelineOld : IPipelineBehaviour
{
    [SerializeField] PlayerAttackSettingsOld settings;
    [SerializeField] DiceSettingsOld diceSettings;

    private InputPipeline inputPipeline;

    private PlayerAttackHandlerOld attackHandler;
    private State currState;
    private LoadoutState loadoutState;

    // Attack state
    public class State : IInteractionData
    {
        public float lastAttackedTime = 0f;
    }

    public class LoadoutState : IInteractionData
    {
        public DiceEffectSettings[] equippedEffects = new DiceEffectSettings[DiceSettingsOld.numOfSides];
    }

    private void Start()
    {
        attackHandler = new PlayerAttackHandlerOld();
        currState = new State();

        inputPipeline = GetComponent<InputPipeline>();

        settings.playerTransform = GetComponent<Transform>();

        data.Add("AttackSettings", settings); 
        data.Add("DiceSettings", diceSettings); 
        data.Add("InputData", inputPipeline.InputData);
        data.Add("CurrState", currState);
        data.Add("LoadoutState", loadoutState);

        attackHandler.Init(data);
    }

    private void Update()
    {
        attackHandler.Handle();
    }
}
