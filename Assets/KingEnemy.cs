using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KingEnemy : Enemy, DataPipeline.IGenerator<PlayerInteractionState>
{
    float pauseTime = 2f;
    float pauseTimer;
    float summonCooldown = 10f;
    float summonTimer = 10f;
    bool summoning;
    public GameObject summons;

    private GameObject bossBar;

    private void Start()
    {
        EnemyStart();
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddGenerator(this);
        bossBar = GameObject.Find("Boss Bar").transform.Find("Image").gameObject;
    }

    private void FixedUpdate()
    {
        if(bossBar) bossBar.transform.localScale = new Vector3(health / maxHealth, 1, 1);

        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        float distance = (float)Math.Sqrt(Math.Pow(player.transform.position.x - transform.position.x, 2) + Math.Pow(player.transform.position.z - transform.position.z, 2));

        direction.Normalize();

        FlipDirection(direction);
        Movement(direction, Time.deltaTime, distance);
        LoopMap();
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
