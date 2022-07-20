using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

/// <summary>
/// Trigger collider for the player to check if it is in any effect
/// </summary>
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
        if (diceEffect != null && diceEffect.EffectSettings.effectName == "Magic Circle")
        {
            playerAttackSpeedMultiplier = (1 / diceEffect.EffectSettings.statusEffect.value) * diceEffect.EffectSettings.floatMultiplier;
            currTimeEffect = diceEffect;
            print("1 playerAttackSpeedMultiplier: " + playerAttackSpeedMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DiceEffect diceEffect = other.GetComponent<DiceEffect>();
        if (diceEffect != null && diceEffect.EffectSettings.effectName == "Magic Circle")
        {
            currTimeEffect = null;
            playerAttackSpeedMultiplier = 1f;
        }
    }
}