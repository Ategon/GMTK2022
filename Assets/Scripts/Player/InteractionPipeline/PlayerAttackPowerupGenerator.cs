using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

/// <summary>
/// Trigger collider for the player to check if it is in any effect
/// </summary>
public class PlayerAttackPowerupGenerator : MonoBehaviour, IGenerator<PlayerInteractionState>
{
    float playerAttackSpeedMultiplier = 1f;
    PowerupGameObject currTimeEffect;

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
        PowerupGameObject powerup = other.GetComponent<PowerupGameObject>();
        if (powerup != null && powerup.PowerupSettings.powerupName == "Magic Circle")
        {
            playerAttackSpeedMultiplier = (1 / powerup.PowerupSettings.statusEffect.value) * powerup.PowerupSettings.floatMultiplier;
            currTimeEffect = powerup;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PowerupGameObject powerup = other.GetComponent<PowerupGameObject>();
        if (powerup != null && powerup.PowerupSettings.powerupName == "Magic Circle")
        {
            currTimeEffect = null;
            playerAttackSpeedMultiplier = 1f;
        }
    }
}