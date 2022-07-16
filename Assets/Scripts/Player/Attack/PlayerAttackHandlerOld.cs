using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandlerOld : IHandler
{
    PlayerAttackSettingsOld attackSettings;
    DiceSettingsOld diceSettings;
    PlayerAttackPipelineOld.State currState;
    InputData inputData;

    public void Init(Dictionary<string, IInteractionData> data)
    {
        attackSettings = (PlayerAttackSettingsOld)data["AttackSettings"];
        diceSettings = (DiceSettingsOld)data["DiceSettings"];
        currState = (PlayerAttackPipelineOld.State)data["CurrState"];
        inputData = (InputData)data["InputData"];
    }

    public void Handle()
    {
        float timeBetweenAttacks = 1 / attackSettings.AttackSpeed;
        if (inputData.Fire && Time.time - currState.lastAttackedTime >= timeBetweenAttacks)
        {
            GameObject die = GameObject.Instantiate(attackSettings.DicePrefab);
            currState.lastAttackedTime = Time.time;
        }
    }
}
