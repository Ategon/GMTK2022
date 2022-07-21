using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGameObject : MonoBehaviour
{
    PowerupSettings powerupSettings;
    public PowerupSettings PowerupSettings { get { return powerupSettings; } }
    
    private List<GameObject> enemies = new List<GameObject>();

    private PlayerStatsModifierGenerator playerStatsModifier;

    public void Init(PowerupSettings powerupSettings)
    {
        this.powerupSettings = powerupSettings;

        LightningPowerup lightningPowerup = GetComponent<LightningPowerup>();
        if (lightningPowerup != null)
        {
            lightningPowerup.Damage = this.powerupSettings.damage * this.powerupSettings.floatMultiplier;
            lightningPowerup.NumOfStrikes = this.powerupSettings.intValue;
        }
        IcePowerup icePowerup = GetComponent<IcePowerup>();
        if (icePowerup != null)
        {
            icePowerup.powerupSettings = powerupSettings;
        }

        StartCoroutine(EndPowerup());
    }

    IEnumerator EndPowerup()
    {
        yield return new WaitForSeconds(powerupSettings.duration);

        if (powerupSettings.ifRemoveEffectOnLeaveCollider)
        {
            foreach (GameObject obj in enemies)
            {
                if (obj == null)
                    continue;

                StatusEffects statusEffects = obj.GetComponent<StatusEffects>();
                if (statusEffects != null)
                {
                    statusEffects.RemoveStatusEffect(powerupSettings.statusEffect.type);
                    continue;
                }
            }
        }

        if (playerStatsModifier != null)
            playerStatsModifier.RemovePowerup(this);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(powerupSettings.damage * powerupSettings.floatMultiplier);

            StatusEffects statusEffects = other.GetComponent<StatusEffects>();
            if (statusEffects != null)
            {
                statusEffects.AddStatusEffect(new StatusEffect(powerupSettings.statusEffect, powerupSettings.floatMultiplier), transform.position);

                enemies.Add(other.gameObject);
                return;
            }
        }

        PlayerStatsModifierGenerator playerStatsModifierGenerator = other.GetComponent<PlayerStatsModifierGenerator>(); 
        if (playerStatsModifierGenerator != null)
        {
            playerStatsModifier = playerStatsModifierGenerator;
            playerStatsModifierGenerator.AddPowerup(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerStatsModifier != null)
            playerStatsModifier.RemovePowerup(this);

        if (!powerupSettings.ifRemoveEffectOnLeaveCollider)
            return;

        StatusEffects statusEffects = other.GetComponent<StatusEffects>();
        if (statusEffects != null)
            statusEffects.RemoveStatusEffect(powerupSettings.statusEffect.type);

        enemies.Remove(other.gameObject);
    }
}
