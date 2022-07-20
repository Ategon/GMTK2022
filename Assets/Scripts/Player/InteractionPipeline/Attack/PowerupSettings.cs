using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupSettings", menuName = "ScriptableObjects/PowerupSettings")]
public class PowerupSettings : ScriptableObject
{
    public bool ifEnabled = false;

    public string effectName;
    public GameObject powerupPrefab;
    public Sprite powerupGlyph;
    public float damage;
    public float duration;

    public bool ifRemoveEffectOnLeaveCollider;

    // TODO (GnoxNahte): Refactor
    // For Level scaling
    public float floatMultiplier = 1f;
    public int intValue;

    public StatusEffect statusEffect;
}
