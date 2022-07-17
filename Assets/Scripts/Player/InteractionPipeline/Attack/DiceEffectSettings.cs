using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceEffect", menuName = "ScriptableObjects/DiceEffect")]
public class DiceEffectSettings : ScriptableObject
{
    public bool ifEnabled = false;

    public string effectName;
    public GameObject diceEffectPrefab;
    public Sprite diceEffectGlyph;
    public float damage;
    public float effectDuration;

    public bool ifRemoveEffectOnLeaveCollider;

    // TODO (GnoxNahte): Refactor
    // For Level scaling
    public float floatMultiplier = 1f;
    public int intValue;

    public StatusEffect statusEffect;
}
