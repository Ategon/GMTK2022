using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceBuilderGenerator : MonoBehaviour, DataPipeline.IGenerator<PlayerInteractionState>
{
    DiceBuilder diceBuilder;

    private void Awake()
    {
        diceBuilder = GameObject.FindObjectOfType<DiceBuilder>();
    }

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
        data.PlayerAttackState.equippedPowerups = diceBuilder.selectedDice.equippedPowerups;
    }
}
