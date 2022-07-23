using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    Fire,
    Ice,
    Poison,
    Lightning,
    MagicCircle,
    Gravity,

    NUM_POWERUPS
}

[CreateAssetMenu(fileName = "PowerupSettings", menuName = "ScriptableObjects/PowerupSettings")]
public class PowerupSettings : ScriptableObject
{
    public bool ifEnabled = false;

    public PowerupType powerupType;
    public string effectName;
    public string powerupDescription;
    public string powerupUpgradeDescription;
    public GameObject powerupPrefab;
    public Sprite powerupGlyph;
    public float damage;
    public float knockbackForce;
    public float duration;

    public bool ifRemoveEffectOnLeaveCollider;

    public int level;

    // TODO (GnoxNahte): Refactor
    // For Level scaling
    public float floatMultiplier = 1f;
    public int intValue;

    public StatusEffect statusEffect;
}
