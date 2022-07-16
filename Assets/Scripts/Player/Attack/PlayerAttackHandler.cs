using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : IHandler
{
    PlayerAttackSettings attackSettings;
    DiceSettings diceSettings;
    PlayerAttackPipeline.State currState;
    InputData inputData;

    public void Init(Dictionary<string, IInteractionData> data)
    {
        attackSettings = (PlayerAttackSettings)data["AttackSettings"];
        diceSettings = (DiceSettings)data["DiceSettings"];
        currState = (PlayerAttackPipeline.State)data["CurrState"];
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
