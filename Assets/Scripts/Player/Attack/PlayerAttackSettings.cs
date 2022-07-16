using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackSettings : IInteractionData
{
    public GameObject DicePrefab;

    public DiceEffect[] DiceEffects;
    // Index in DiceEffect for the Dice effect that the player starts with
    public int StartingDiceEffectIndex = 0;

    // Number of attacks per second
    public float AttackSpeed; 
}

[System.Serializable]
public class DiceSettings : IInteractionData
{
    // Base dice stats without any modifiers
    public float AttackDamge;
    public float Lifetime;
    public float Size = 1f;
    public float CritChance;
    // If Crit, Damage = AttackDamage * CritDamageMultiplier
    public float CritDamageMultiplier; 
}
