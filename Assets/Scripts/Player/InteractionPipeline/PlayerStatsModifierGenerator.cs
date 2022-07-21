using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

using System;
using System.Reflection;

/// <summary>
/// Trigger collider for the player to check if it is in any effect
/// </summary>
public class PlayerStatsModifierGenerator : MonoBehaviour, IGenerator<PlayerInteractionState>
{
    [System.Serializable]
    public class PowerupPair
    {
        public PowerupGameObject powerupGameObj;
        public float modifierAmt;
        public PowerupType powerupType { get { return powerupGameObj.PowerupSettings.powerupType; } }
    }

    // Tuple.Item1 = PowerupGameObject
    // Tuple.Item2 = modifierAmt
    List<PowerupPair> powerupModifierData;

    PlayerAttackStatsModifier attackStatsModifier;

    private void Awake()
    {
        powerupModifierData = new List<PowerupPair>();

        // Using reflection to set all values to 1f
        Type type = typeof(PlayerAttackStatsModifier);
        object attackStatsModifierObj = attackStatsModifier;
        MemberInfo[] props = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        for (int i = 1; i < props.Length; ++i)
        {
            FieldInfo field = (FieldInfo)props[i];
            if (field == null)
                continue;

            if (field.FieldType == typeof(float))
                field.SetValue(attackStatsModifierObj, 1f);
            else if (field.FieldType == typeof(int))
                field.SetValue(attackStatsModifierObj, 1);
            else
                Debug.LogError($"Type: {field.FieldType} \n is not implemented when calculating Player's final stats in PlayerStatsManager");
        }
        attackStatsModifier = (PlayerAttackStatsModifier)attackStatsModifierObj;
    }

    public void Start()
    {
    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        data.PlayerAttackStatsModifier.AttackSpeed = attackStatsModifier.AttackSpeed;
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }

    public void AddPowerup(PowerupGameObject powerupGameObject)
    {
        PowerupSettings powerupSettings = powerupGameObject.PowerupSettings;

        float modifierAmt = 0f;
        switch (powerupGameObject.PowerupSettings.powerupType)
        {
            case PowerupType.MagicCircle:
                modifierAmt = (1 / powerupSettings.statusEffect.value) * powerupSettings.floatMultiplier;
                break;
            default:
                // Do nothing for now
                break;
        }

        int powerupIndex = GetPowerupIndex(powerupGameObject.PowerupSettings.powerupType);

        // If can't find the powerup
        if (powerupIndex == -1)
        {
            powerupModifierData.Add(new PowerupPair { powerupGameObj = powerupGameObject, modifierAmt = modifierAmt });
            powerupIndex = powerupModifierData.Count - 1;
            attackStatsModifier.AttackSpeed += modifierAmt;
        }
        else
        {
            attackStatsModifier.AttackSpeed -= powerupModifierData[powerupIndex].modifierAmt;
            powerupModifierData[powerupIndex].powerupGameObj = powerupGameObject;
            powerupModifierData[powerupIndex].modifierAmt = modifierAmt;
            attackStatsModifier.AttackSpeed += modifierAmt;
        }


    }

    public void RemovePowerup(PowerupGameObject powerupGameObject)
    {
        int powerupIndex = GetPowerupIndex(powerupGameObject);

        if (powerupIndex == -1)
            return;

        switch (powerupGameObject.PowerupSettings.powerupType)
        {
            case PowerupType.MagicCircle:
                attackStatsModifier.AttackSpeed -= powerupModifierData[powerupIndex].modifierAmt;
                break;
            default:
                // Do nothing for now
                break;
        }

        powerupModifierData.RemoveAt(powerupIndex);
    }

    // Returns -1 if can't find it
    public int GetPowerupIndex(PowerupType powerupType)
    {
        for (int i = 0; i < powerupModifierData.Count; i++)
        {
            if (powerupModifierData[i].powerupType == powerupType)
                return i;
        }

        return -1;
    }

    // Returns -1 if can't find it
    public int GetPowerupIndex(PowerupGameObject powerupGameObject)
    {
        for (int i = 0; i < powerupModifierData.Count; i++)
        {
            if (powerupModifierData[i].powerupGameObj == powerupGameObject)
                return i;
        }

        return -1;
    }
}