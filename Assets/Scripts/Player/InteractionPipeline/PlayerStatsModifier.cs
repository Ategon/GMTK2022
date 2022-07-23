using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// - If commented out, means it haven't been implemented
// - All the values should be in multipliers for the base value. 
//   E.g. FinalAttackSpeed = BaseAttackSpeed * PlayerAttackStatsModifier.AttackSpeed
[System.Serializable]
public struct PlayerMovementStatsModifier : DataPipeline.IData
{
    //public float WalkingSpeed;
    //public float CrouchSpeed;
    //public float DodgeSpeed;
    //public float DodgeTime;
    //public float DodgeCooldown;
    public void Clear()
    {
    }
}

[System.Serializable]
public struct PlayerAttackStatsModifier : DataPipeline.IData
{
    public float AttackSpeed;

    public void Clear()
    {
    }

    //[Header("Dice Modifiers")]
    //// Base dice stats without any modifiers
    //public float DiceAttackDamge;
    //public float DiceSpeed;
    //public float DiceLifetime;
    //public float DiceSize;
    //public float DiceCritChance;
    //// If Crit, DDiceamage = AttackDamage * CritDamageMultiplier
    //public float DiceCritDamageMultiplier;
    //
    //[Header("Powerup Modifiers")]
    //public float PowerupAttackDamage;
    //public float PowerupSize;
}
