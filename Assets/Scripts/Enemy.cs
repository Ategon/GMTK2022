using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    SpriteRenderer sr;
    CharacterController controller;

    [SerializeField] private float walkingSpeed = 1f;
    [SerializeField] private float maxHealth = 100;

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        direction.Normalize();

        if (direction.x < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        controller.Move(direction * 2f * walkingSpeed * Time.deltaTime);

        if (player.transform.position.x - transform.position.x > 40)
        {
            controller.Move(Vector3.right * 80f);
        }
        if (player.transform.position.x - transform.position.x < -40)
        {
            controller.Move(Vector3.right * -80f);
        }
        if (player.transform.position.z - transform.position.z > 40)
        {
            controller.Move(Vector3.forward * 80f);
        }
        if (player.transform.position.z - transform.position.z < -40)
        {
            controller.Move(Vector3.forward * -80f);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
