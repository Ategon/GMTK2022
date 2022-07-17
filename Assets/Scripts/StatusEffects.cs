using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    Fire,
    Poison,
    Slow,
}

public class StatusEffect
{
    public StatusEffectType type;

    public float value;
    public float duration;
    public float timeBetweenApplyEffect;

    public float lastTimeEffectWasApplied;

    public StatusEffect(StatusEffectType type, float value, float duration)
    {
        this.type = type;
        this.value = value;
        this.duration = duration;

        lastTimeEffectWasApplied = 0f;
    }
}

public class StatusEffects : MonoBehaviour
{
    List<StatusEffect> statusEffects;

    private Enemy enemy;
    private Rigidbody rb;

    float walkingSpeedMultiplier = 1f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        switch (statusEffect.type)
        {
            case StatusEffectType.Fire:
            case StatusEffectType.Poison:
                // For now, do nothing
                break;
            case StatusEffectType.Slow:
                walkingSpeedMultiplier *= statusEffect.value;
                break;
            default:
                Debug.LogError("StatusEffects.AddStatusEffect: Status effect not implemented");
                break;
        }

        statusEffects.Add(statusEffect);
    }

    private void Update()
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (Time.time - statusEffect.lastTimeEffectWasApplied > statusEffect.timeBetweenApplyEffect)
            {
                switch (statusEffect.type)
                {
                    case StatusEffectType.Fire:
                    case StatusEffectType.Poison:
                        enemy.TakeDamage(statusEffect.value);
                        break;
                    case StatusEffectType.Slow:
                        // For now, do nothing
                        break;
                    default:
                        Debug.LogError("StatusEffects.Update: Status effect not implemented");
                        break;
                }
            }
        }
    }
}
