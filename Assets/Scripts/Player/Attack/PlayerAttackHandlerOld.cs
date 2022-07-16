using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandlerOld : IHandler
{
    PlayerAttackSettingsOld attackSettings;
    DiceSettingsOld diceSettings;
    PlayerAttackPipelineOld.State currState;
    PlayerAttackPipelineOld.LoadoutState loadoutState;
    InputData inputData;

    public void Init(Dictionary<string, IInteractionData> data)
    {
        attackSettings = (PlayerAttackSettingsOld)data["AttackSettings"];
        diceSettings = (DiceSettingsOld)data["DiceSettings"];
        currState = (PlayerAttackPipelineOld.State)data["CurrState"];
        loadoutState = (PlayerAttackPipelineOld.LoadoutState)data["LoadoutState"];
        inputData = (InputData)data["InputData"];

        loadoutState.equippedEffects[0] = attackSettings.DiceEffects[attackSettings.StartingDiceEffectIndex];
    }

    public void Handle()
    {
        float timeBetweenAttacks = 1 / attackSettings.AttackSpeed;
        if (inputData.Fire && Time.time - currState.lastAttackedTime >= timeBetweenAttacks)
        {
            // TODO (GnoxNahte): Replace with pool
            GameObject diceObj = GameObject.Instantiate(attackSettings.DicePrefab, attackSettings.playerTransform.position, Quaternion.identity);
            Dice dice = diceObj.GetComponent<Dice>();
            dice.Init(diceSettings, loadoutState.equippedEffects);
            currState.lastAttackedTime = Time.time;
        }
    }
}
