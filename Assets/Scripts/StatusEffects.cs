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
    public float remainingDuration; 
    public float timeBetweenApplyEffect;

    public float lastTimeEffectWasApplied;

    [Header("Knockback")]
    public float explosionForce;
    public float explosionRadius;
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
                walkingSpeedMultiplier *= statusEffect.value;
                break;
            case StatusEffectType.Knockback:
                rb.AddExplosionForce(statusEffect.explosionForce, dicePos, statusEffect.explosionRadius, 0, ForceMode.Impulse);
                return;
            case StatusEffectType.NoEffect:
            case StatusEffectType.Fire:
            case StatusEffectType.Poison:
                // For now, do nothing
                break;
            default:
                Debug.LogError("StatusEffects.AddStatusEffect: Status effect not implemented");
                break;
        }

        statusEffects.Add(statusEffect);
    }

    private void Update()
    {
        for (int i = 0; i < statusEffects.Count; )
        {
            StatusEffect statusEffect = statusEffects[i];
            statusEffect.remainingDuration -= Time.deltaTime;
            if (statusEffect.remainingDuration < 0)
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
}
