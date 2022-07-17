using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiceEffectBase : MonoBehaviour
{
    DiceEffectSettings effectSettings;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(effectSettings.damage);
        }
    }
}
