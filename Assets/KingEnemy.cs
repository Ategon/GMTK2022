using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingEnemy : Enemy
{
    float pauseTime = 2f;
    float pauseTimer;
    float summonCooldown = 10f;
    float summonTimer = 10f;
    bool summoning;
    public GameObject summons;

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
