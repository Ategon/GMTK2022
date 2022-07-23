using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public float knockbackForce;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(float damage, Vector3 dir, float knockbackForce)
    {
        dir.y = 0;
        transform.forward = dir;
        this.damage = damage;
        this.knockbackForce = knockbackForce;
        rb.AddForce(dir.normalized * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
            enemy.TakeDamageWithKnockback(damage, rb.velocity, knockbackForce, StatusEffectType.Slow);
    }
}
