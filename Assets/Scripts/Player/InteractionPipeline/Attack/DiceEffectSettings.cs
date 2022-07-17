using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceEffect", menuName = "ScriptableObjects/DiceEffect")]
public class DiceEffectSettings : ScriptableObject
{
    public GameObject diceEffectPrefab;
    public Sprite diceEffectGlyph;
    public float damage;
    public float effectDuration;

    public bool ifRemoveEffectOnLeaveCollider;

    // TODO (GnoxNahte): Refactor
    public int numOfLightningStrikes; 

    public StatusEffect statusEffect;
}
