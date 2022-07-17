using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerAttackDiceEffectGenerator : MonoBehaviour, IGenerator<PlayerInteractionState>
{
    float playerAttackSpeedMultiplier = 1f;
    DiceEffect currTimeEffect;

    public void Start()
    {
    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        if (currTimeEffect == null)
            data.PlayerAttackState.playerAttackSpeedMultiplier = 1f;
        else
            data.PlayerAttackState.playerAttackSpeedMultiplier = playerAttackSpeedMultiplier;
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        DiceEffect diceEffect = other.GetComponent<DiceEffect>();
        if (diceEffect != null && diceEffect.EffectSettings.effectName == "Time")
        {
            playerAttackSpeedMultiplier =  diceEffect.EffectSettings.floatMultiplier + 0.5f;
            currTimeEffect = diceEffect;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DiceEffect diceEffect = other.GetComponent<DiceEffect>();
        if (diceEffect != null && diceEffect.EffectSettings.effectName == "Time")
        {
            currTimeEffect = null;
            playerAttackSpeedMultiplier = 1f;
        }
    }
}
