using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackSettingsOld : IInteractionData
{
    public GameObject DicePrefab;

    public DiceEffectSettings[] DiceEffects;
    // Index in DiceEffect for the Dice effect that the player starts with
    public int StartingDiceEffectIndex = 0;

    // Number of attacks per second
    public float AttackSpeed;

    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Camera mainCamera;
}

// Pass this data to the dice that is thrown
[System.Serializable]
public class DiceSettingsOld : IInteractionData
{
    // Base dice stats without any modifiers
    public float AttackDamge;
    public float Speed;
    public float Lifetime;
    public float Size = 1f;
    public float CritChance;
    // If Crit, Damage = AttackDamage * CritDamageMultiplier
    public float CritDamageMultiplier;

    public const int numOfSides = 6;
}
