using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceEffectLoadoutHandler : IHandler
{
    PlayerAttackPipelineOld.LoadoutState loadoutState;

    DiceEffectSettings[] equippedEffects;

    public void Init(Dictionary<string, IInteractionData> data)
    {
        loadoutState = (PlayerAttackPipelineOld.LoadoutState)data["LoadoutState"];
        equippedEffects = loadoutState.equippedEffects;
    }

    public void Handle()
    {

    }

    public void AddEffectToLoadout(DiceEffectSettings diceEffect, int diceNumber)
    {
        if (loadoutState.equippedEffects[diceNumber] != null)
            Debug.LogError($"Dice Number: {diceNumber} is already taken up by {diceEffect.diceEffectPrefab.name}");
        else
            loadoutState.equippedEffects[diceNumber] = diceEffect;
    }
}
