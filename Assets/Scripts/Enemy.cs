using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    GameObject player;
    protected Rigidbody rb;
    SpriteRenderer sr;
    //protected CharacterController controller;
    protected Animator animator;
    protected StatusEffects statusEffects;

    [SerializeField] protected float walkingSpeed = 1f;
    [SerializeField] private float maxHealth = 100;
    public float expAmount = 20;

    public float heldOrbs = 0;

    protected float moveSpeed { get { return walkingSpeed * statusEffects.walkingSpeedMultiplier; } }

    protected float health;

    [SerializeField] public GameObject exp;

    // Start is called before the first frame update
    void Start()
    {
        EnemyStart();
    }

    protected void EnemyStart()
    {
        player = GameObject.Find("Player");
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        health = maxHealth;
        animator = transform.Find("Sprite").GetComponent<Animator>();
        statusEffects = GetComponent<StatusEffects>();
    }

    void FlipDirection(Vector3 direction)
    {
        if (direction.x < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    public virtual void Movement(Vector3 direction, float deltaTime, float distance)
    {
        rb.MovePosition(transform.position + direction * 2f * moveSpeed * deltaTime);
    }

    void LoopMap()
    {
        if (player.transform.position.x - transform.position.x > 40)
        {
            rb.MovePosition(transform.position + Vector3.right * 80f);
        }
        if (player.transform.position.x - transform.position.x < -40)
        {
            rb.MovePosition(transform.position + Vector3.right * -80f);
        }
        if (player.transform.position.z - transform.position.z > 40)
        {
            rb.MovePosition(transform.position + Vector3.forward * 80f);
        }
        if (player.transform.position.z - transform.position.z < -40)
        {
            rb.MovePosition(transform.position + Vector3.forward * -80f);
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        float distance = (float) Math.Sqrt(Math.Pow(player.transform.position.x - transform.position.x, 2) + Math.Pow(player.transform.position.z - transform.position.z, 2));

        direction.Normalize();

        FlipDirection(direction);
        Movement(direction, Time.deltaTime, distance);
        LoopMap();
    }

    
    public void TakeDamageWithKnockback(float damage, Vector3 velocityDir, float knockbackForce, StatusEffectType statusEffectType = StatusEffectType.NoEffect)
    {
        velocityDir.y = 0f;
        rb.AddForce(velocityDir.normalized * knockbackForce, ForceMode.Impulse);

        TakeDamage(damage, statusEffectType);
    }

    public virtual void TakeDamage(float damage, StatusEffectType statusEffectType = StatusEffectType.NoEffect)
    {
        health -= damage;

        if (health <= 0)
        {
            Instantiate(exp, transform.position, Quaternion.identity);
            for(int i = 0; i < (int) heldOrbs; ++i)
            {
                Instantiate(exp, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }

        DamagePopupManager.OnDamage(transform.position, damage, statusEffectType);
    }

    public void UpdateTint(Color color)
    {
        if (sr != null)
            sr.color = color;
    }

    private void OnCollisionEnter(Collision hit)
    {
        /*if (hit.gameObject.tag == "EXP")
        {
            Destroy(hit.gameObject);
            heldOrbs++;
        }*/
    }
}
