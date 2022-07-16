using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPipelineOld : IPipelineBehaviour
{
    [SerializeField] PlayerAttackSettingsOld settings;
    [SerializeField] DiceSettingsOld diceSettings;

    private InputPipeline inputPipeline;

    private PlayerAttackHandlerOld attackHandler;
    private PlayerDiceEffectLoadoutHandler loadoutHandler;

    // Maybe will combine to 1 state
    private State currState;
    private LoadoutState loadoutState;

    // Attack state
    public class State : IInteractionData
    {
        public float lastAttackedTime = 0f; 

        public ObjectPool dicePool;
    }

    public class LoadoutState : IInteractionData
    {
        public DiceEffectSettings[] equippedEffects = new DiceEffectSettings[DiceSettingsOld.numOfSides];
    }

    private void Start()
    {
        attackHandler = new PlayerAttackHandlerOld();
        loadoutHandler = new PlayerDiceEffectLoadoutHandler();

        currState = new State();
        loadoutState = new LoadoutState();

        inputPipeline = GetComponent<InputPipeline>();

        settings.playerTransform = GetComponent<Transform>();
        settings.mainCamera = Camera.main;

        data.Add("AttackSettings", settings); 
        data.Add("DiceSettings", diceSettings); 
        data.Add("InputData", inputPipeline.InputData);
        data.Add("CurrState", currState);
        data.Add("LoadoutState", loadoutState);

        attackHandler.Init(data);
        loadoutHandler.Init(data);
    }

    private void Update()
    {
        attackHandler.Handle();
    }

    [ContextMenu("AddEffectTest")]
    private void AddEffectTest()
    {
        loadoutHandler.AddEffectToLoadout(settings.DiceEffects[1], 3);
    }
}
