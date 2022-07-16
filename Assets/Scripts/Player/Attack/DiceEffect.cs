using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceEffect", menuName = "ScriptableObjects/DiceEffect")]
public class DiceEffect : ScriptableObject
{
    public Sprite sprite;
    public float damage;
}
