using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerAttackState
{
    public float lastAttackedTime;
    public Vector3 ShootDir;

    // Format: NumOfFaces, ObjectPool
    public Dictionary<int, ObjectPool> dicePools;

    public bool throwTriggered;

    public PowerupSettings[] equippedPowerups;
}

[Serializable]
public struct PlayerAttackSettings
{
    public GameObject DicePrefab;
    public PowerupSettings[] Powerups;

    public int StartingPowerupIndex;

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
    public float KnockbackForce;
    public float CritChance;
    // If Crit, Damage = AttackDamage * CritDamageMultiplier
    public float CritDamageMultiplier;

    public Vector2 SpinSpeedRange;
    public float SpinSlowDown;

    public const int numOfSides = 6;
}