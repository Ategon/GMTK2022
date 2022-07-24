using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceBuilderGenerator : MonoBehaviour, DataPipeline.IGenerator<PlayerInteractionState>
{
    [SerializeField] DiceBuilder diceBuilder;

    public void Start()
    {

    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        Handle(ref data);
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }

    public void Handle(ref PlayerInteractionState data)
    {
        data.PlayerAttackSettings.DicePrefab = diceBuilder.selectedDice.dicePrefab;
        data.PlayerAttackState.equippedPowerups = diceBuilder.selectedDice.equippedPowerups;

        // Should be in handler
        if (data.PlayerState.DiceBuilder)
        {
            diceBuilder.OpenDiceBuilder();
        }
    }
}
