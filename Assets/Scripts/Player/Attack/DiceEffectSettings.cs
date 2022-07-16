using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceEffect", menuName = "ScriptableObjects/DiceEffect")]
public class DiceEffectSettings : ScriptableObject
{
    public GameObject diceEffectPrefab;
    public float damage;
}
