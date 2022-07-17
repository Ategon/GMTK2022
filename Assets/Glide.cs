using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glide : MonoBehaviour
{
    public Vector3 dir;

    void FixedUpdate()
    {
        transform.Translate(dir * Time.deltaTime);
    }
}
