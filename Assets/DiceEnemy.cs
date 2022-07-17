using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEnemy : Enemy
{
    bool quicker;
    float pauseTime = 2f;
    float pauseTimer;

    float boostVelocity = 2f;
    float currentBoost;

    public override void Movement(Vector3 direction, float deltaTime, float distance)
    {
        pauseTimer -= Time.deltaTime;

        if (pauseTimer <= 0)
        {
            if (currentBoost > 0) currentBoost -= Time.deltaTime * currentBoost;
            if (currentBoost < 0) currentBoost = 0;

            if (quicker)
            {
                rb.MovePosition(transform.position + direction * (4.5f + currentBoost) * walkingSpeed * deltaTime);
                animator.Play("dice-enemy-roll");
            }
            else
            {
                rb.MovePosition(transform.position + direction * 1.5f * walkingSpeed * deltaTime);
                animator.Play("dicebullywalk");

                if (distance <= 5)
                {
                    quicker = true;
                    pauseTimer = pauseTime;
                    animator.Play("dicebullycharge");
                    currentBoost = boostVelocity;
                }
            }
        } 


    }
}
