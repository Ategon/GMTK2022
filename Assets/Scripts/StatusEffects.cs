using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    NoEffect,
    Burn,
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

    [SerializeField] Color slowTint;
    [SerializeField] Color burningTint;
    [SerializeField] Color poisonTint;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();

        statusEffects = new List<StatusEffect>();
    }

    public void AddStatusEffect(StatusEffect statusEffect, Vector3 dicePos)
    {
        switch (statusEffect.type)
        {
            case StatusEffectType.Slow:
                walkingSpeedMultiplier = statusEffect.value;
                break;
            case StatusEffectType.Knockback:
                rb.AddExplosionForce(statusEffect.explosionForce, dicePos, statusEffect.explosionRadius, 0, ForceMode.Impulse);
                return;
            case StatusEffectType.Burn:
            case StatusEffectType.Poison:
                break;
            case StatusEffectType.NoEffect:
                Debug.LogWarning("Status effect has type: NoEffect");
                return;
            default:
                Debug.LogError("StatusEffects.AddStatusEffect: Status effect not implemented");
                return;
        }

        RefreshAndAddStatusEffect(statusEffect);

        enemy.UpdateTint(GetTint());
    }

    private void Update()
    {
        for (int i = 0; i < statusEffects.Count; )
        {
            StatusEffect statusEffect = statusEffects[i];
            statusEffect.duration -= Time.deltaTime;
            if (statusEffect.duration < 0)
            {
                if (statusEffect.type == StatusEffectType.Slow)
                    walkingSpeedMultiplier = 1f;

                statusEffects.RemoveAt(i);

                enemy.UpdateTint(GetTint());

                continue;
            }
            if (Time.time - statusEffect.lastTimeEffectWasApplied > statusEffect.timeBetweenApplyEffect)
            {
                switch (statusEffect.type)
                {
                    case StatusEffectType.Burn:
                    case StatusEffectType.Poison:
                        enemy.TakeDamage(statusEffect.value);
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

    public Color GetTint()
    {
        Color baseColor = Color.black;

        if (GetStatusEffect(StatusEffectType.Slow) != -1)
            baseColor += slowTint;
        if (GetStatusEffect(StatusEffectType.Poison) != -1)
            baseColor += poisonTint;
        if (GetStatusEffect(StatusEffectType.Burn) != -1)
            baseColor += burningTint;

        if (baseColor == Color.black)
            return Color.white;
        else
            return baseColor / statusEffects.Count;
    }

    // Overwrites the status effect if there is already a status effect of that type
    public void RefreshAndAddStatusEffect(StatusEffect statusEffect)
    {
        int index = GetStatusEffect(statusEffect.type);
        if (index != -1)
            statusEffects.RemoveAt(index);

        statusEffects.Add(statusEffect);
    }


    // Returns the index of the status effect
    // Returns -1 if it can't find it
    public int GetStatusEffect(StatusEffectType type)
    {
        for (int i = 0; i < statusEffects.Count; i++)
            if (statusEffects[i].type == type)
                return i;

        return -1;
    }
}
