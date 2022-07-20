using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGameObject : MonoBehaviour
{
    PowerupSettings effectSettings;
    public PowerupSettings EffectSettings { get { return effectSettings; } }
    
    private List<GameObject> enemies = new List<GameObject>();

    public void Init(PowerupSettings powerupSettings)
    {
        effectSettings = powerupSettings;

        LightningEffect lightningEffect = GetComponent<LightningEffect>();
        if (lightningEffect != null)
        {
            lightningEffect.Damage = effectSettings.damage * effectSettings.floatMultiplier;
            lightningEffect.NumOfStrikes = effectSettings.intValue;
        }
        IceEffect iceEffect = GetComponent<IceEffect>();
        if (iceEffect != null)
        {
            iceEffect.damage = effectSettings.damage * effectSettings.floatMultiplier;
        }

        StartCoroutine(EndPowerup());
    }

    IEnumerator EndPowerup()
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
            enemy.TakeDamage(effectSettings.damage * effectSettings.floatMultiplier);

            StatusEffects statusEffects = other.GetComponent<StatusEffects>();
            if (statusEffects != null)
            {
                statusEffects.AddStatusEffect(new StatusEffect(effectSettings.statusEffect, effectSettings.floatMultiplier), transform.position);

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
