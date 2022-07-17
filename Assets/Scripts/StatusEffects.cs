using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    NoEffect,
    Fire,
    Poison,
    Slow,
    Knockback,
}

[System.Serializable]
public class StatusEffect
{
    public StatusEffectType type;

    // Amount of damage over time / Slow effect
    public float value; 
    // Remaining duration the effect is active
    public float duration; 
    public float timeBetweenApplyEffect;

    //[HideInInspector]
    public float lastTimeEffectWasApplied;

    [Header("Knockback")]
    public float explosionForce;
    public float explosionRadius;

    public StatusEffect(StatusEffect copy)
    {
        type = copy.type;
        value = copy.value;
        duration = copy.duration;
        timeBetweenApplyEffect = copy.timeBetweenApplyEffect;
        explosionForce = copy.explosionForce;
        explosionRadius = copy.explosionRadius;

        lastTimeEffectWasApplied = 0f;
    }
}

public class StatusEffects : MonoBehaviour
{
    List<StatusEffect> statusEffects;

    private Enemy enemy;
    private Rigidbody rb;

    public float walkingSpeedMultiplier = 1f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();

        statusEffects = new List<StatusEffect>();
    }

    public void AddStatusEffect(StatusEffect statusEffect, Vector3 dicePos)
    {
        print("Added status effect: " + statusEffect.type);
        switch (statusEffect.type)
        {
            case StatusEffectType.Slow:
                walkingSpeedMultiplier = statusEffect.value;
                break;
            case StatusEffectType.Knockback:
                rb.AddExplosionForce(statusEffect.explosionForce, dicePos, statusEffect.explosionRadius, 0, ForceMode.Impulse);
                return;
            case StatusEffectType.NoEffect:
            case StatusEffectType.Fire:
            case StatusEffectType.Poison:
                break;
            default:
                Debug.LogError("StatusEffects.AddStatusEffect: Status effect not implemented");
                break;
        }


        RefreshAndAddStatusEffect(statusEffect);
    }

    private void Update()
    {
        for (int i = 0; i < statusEffects.Count; )
        {
            StatusEffect statusEffect = statusEffects[i];
            statusEffect.duration -= Time.deltaTime;
            if (statusEffect.duration < 0)
            {
                statusEffects.RemoveAt(i);
                continue;
            }
            if (Time.time - statusEffect.lastTimeEffectWasApplied > statusEffect.timeBetweenApplyEffect)
            {
                switch (statusEffect.type)
                {
                    case StatusEffectType.Fire:
                    case StatusEffectType.Poison:
                        enemy.TakeDamage(statusEffect.value);
                        print($"Enemy taking damage by {statusEffect.type}, Dmg: {statusEffect.value}, time: {statusEffect.lastTimeEffectWasApplied}");
                        statusEffect.lastTimeEffectWasApplied = Time.time;
                        break;
                    case StatusEffectType.NoEffect:
                    case StatusEffectType.Slow:
                    case StatusEffectType.Knockback:
                        // For now, do nothing
                        break;
                    default:
                        Debug.LogError("StatusEffects.Update: Status effect not implemented");
                        break;
                }
            }

            ++i;
        }
    }

    // Overwrites the status effect if there is already a status effect of that type
    public void RefreshAndAddStatusEffect(StatusEffect statusEffect)
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (statusEffects[i].type == statusEffect.type)
            {
                statusEffects.RemoveAt(i);
            }
        }

        statusEffects.Add(statusEffect);
    }
}
