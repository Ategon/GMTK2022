using System;
using UnityEngine;

[Serializable]
public struct PlayerAttackState
{
    public float lastAttackedTime;
    public Vector3 ShootDir;

    public ObjectPool dicePool;

    public bool throwTriggered;

    public DiceEffectSettings[] equippedEffects;
}

[Serializable]
public struct PlayerAttackSettings
{
    public GameObject DicePrefab;
    public DiceEffectSettings[] DiceEffects;

    public int StartingDiceEffectIndex;

    // Number of attacks per second
    public float AttackSpeed;

    public LayerMask MouseRaycastLayerMask;
}

[Serializable]
public struct DiceAttackSettings
{
    // Base dice stats without any modifiers
    public float AttackDamge;
    public float Speed;
    public float Lifetime;
    public float Size;
    public float CritChance;
    // If Crit, Damage = AttackDamage * CritDamageMultiplier
    public float CritDamageMultiplier;

    public Vector2 SpinSpeedRange;
    public float SpinSlowDown;

    public const int numOfSides = 6;
}