using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteEnemy : Enemy
{
    float pauseTime = 5f;
    float pauseTimer;
    float summonCooldown = 10f;
    float summonTimer = 10f;
    public bool summoning;
    public GameObject summons;

    public override void Movement(Vector3 direction, float deltaTime, float distance)
    {
        pauseTimer -= Time.deltaTime;
        summonTimer -= Time.deltaTime;

        if (summonTimer <= 0)
        {
            summonTimer = summonCooldown;
            pauseTimer = pauseTime;
            animator.Play("roulette-spin");
            summoning = true;
        }
        else if (pauseTimer <= 0)
        {
            if (summoning)
            {
                summoning = false;
            }
            else
            {
                rb.MovePosition(transform.position + direction * 1.5f * walkingSpeed * deltaTime);
                animator.Play("roulette-walk");
            }
        }


    }
}
