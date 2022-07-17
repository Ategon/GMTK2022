using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEffect : MonoBehaviour
{
    DiceEffectSettings effectSettings;
    
    public void Init(DiceEffectSettings diceEffectSettings)
    {
        effectSettings = diceEffectSettings;

        StartCoroutine(EndDiceEffect());
    }

    IEnumerator EndDiceEffect()
    {
        yield return new WaitForSeconds(effectSettings.effectDuration);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(effectSettings.damage);

            StatusEffects statusEffects = other.GetComponent<StatusEffects>();
            if (statusEffects != null)
            {
                statusEffects.AddStatusEffect(new StatusEffect(effectSettings.statusEffect), transform.position);
            }
        }
    }
}
