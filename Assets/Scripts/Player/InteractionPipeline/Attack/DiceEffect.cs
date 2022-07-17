using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEffect : MonoBehaviour
{
    DiceEffectSettings effectSettings;

    private List<GameObject> enemies = new List<GameObject>();

    public void Init(DiceEffectSettings diceEffectSettings)
    {
        effectSettings = diceEffectSettings;

        LightningEffect lightningEffect = GetComponent<LightningEffect>();
        if (lightningEffect != null)
        {
            lightningEffect.Damage = effectSettings.damage;
            lightningEffect.NumOfStrikes = effectSettings.numOfLightningStrikes;
        }
        IceEffect iceEffect = GetComponent<IceEffect>();
        if (iceEffect != null)
        {
            iceEffect.damage = effectSettings.damage;
        }

        StartCoroutine(EndDiceEffect());
    }

    IEnumerator EndDiceEffect()
    {
        yield return new WaitForSeconds(effectSettings.effectDuration);

        if (effectSettings.ifRemoveEffectOnLeaveCollider)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null)
                    continue;

                StatusEffects statusEffects = enemy.GetComponent<StatusEffects>();
                if (statusEffects != null)
                    statusEffects.RemoveStatusEffect(effectSettings.statusEffect.type);
            }
        }

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

                enemies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!effectSettings.ifRemoveEffectOnLeaveCollider)
            return;

        StatusEffects statusEffects = other.GetComponent<StatusEffects>();
        if (statusEffects != null)
            statusEffects.RemoveStatusEffect(effectSettings.statusEffect.type);

        enemies.Remove(other.gameObject);
    }
}
