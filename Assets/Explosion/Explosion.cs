using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float timer;

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 0.7) Destroy(gameObject);
    }
}
