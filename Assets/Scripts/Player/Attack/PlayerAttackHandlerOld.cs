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

        currState.dicePool = new ObjectPool();
        currState.dicePool.InitPool("DicePool", attackSettings.DicePrefab, 30);

        loadoutState.equippedEffects[0] = attackSettings.DiceEffects[attackSettings.StartingDiceEffectIndex];
    }

    public void Handle()
    {
        float timeBetweenAttacks = 1 / attackSettings.AttackSpeed;
        if (inputData.Fire && Time.time - currState.lastAttackedTime >= timeBetweenAttacks)
        {
            RaycastHit hitInfo;
            // Calculate the direction to throw the dice
            Ray ray = attackSettings.mainCamera.ScreenPointToRay(inputData.CursorPos);
            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 direction = (hitInfo.point - attackSettings.playerTransform.position).normalized;

                GameObject diceObj = currState.dicePool.Get();
                diceObj.SetActive(true);
                diceObj.transform.position = attackSettings.playerTransform.position + direction + Vector3.up * 0.6f;
                diceObj.transform.rotation = attackSettings.playerTransform.rotation;
                Dice dice = diceObj.GetComponent<Dice>();
                dice.Init(diceSettings, loadoutState.equippedEffects, currState.dicePool, direction);
                currState.lastAttackedTime = Time.time;
            }
        }
    }
}
