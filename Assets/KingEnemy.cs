using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingEnemy : Enemy, DataPipeline.IGenerator<PlayerInteractionState>
{
    float pauseTime = 2f;
    float pauseTimer;
    float summonCooldown = 10f;
    float summonTimer = 10f;
    bool summoning;
    public GameObject summons;

    private void Start()
    {
        EnemyStart();
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddGenerator(this);
    }

    public void Write(ref PlayerInteractionState data)
    {
        if(gameObject != null)
        {
            if (health <= 0)
            {
                data.GameState.bossKilled = true;
                Instantiate(exp, transform.position, Quaternion.identity);
                for (int i = 0; i < (int)heldOrbs; ++i)
                {
                    Instantiate(exp, transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().RemoveGenerator(this);
    }

    public override void TakeDamage(float damage, StatusEffectType statusEffectType = StatusEffectType.NoEffect)
    {
        health -= damage;

        DamagePopupManager.OnDamage(transform.position, damage, statusEffectType);
    }


    public override void Movement(Vector3 direction, float deltaTime, float distance)
    {
        pauseTimer -= Time.deltaTime;
        summonTimer -= Time.deltaTime;

        if (summonTimer <= 0)
        {
            summonTimer = summonCooldown;
            pauseTimer = pauseTime;
            animator.Play("dice-enemy-charge");
            summoning = true;
        }
        else if (pauseTimer <= 0)
        {
            if (summoning)
            {
                summoning = false;
                Instantiate(summons, transform.position + direction, Quaternion.identity);
                Instantiate(summons, transform.position + direction, Quaternion.identity);
            }
            else
            {
                rb.MovePosition(transform.position + direction * 1.5f * walkingSpeed * deltaTime);
                animator.Play("dice-enemy-walk");
            }
        }


    }
}
