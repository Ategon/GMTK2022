using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private SpriteRenderer upper;
    private SpriteRenderer lower;

    void Start()
    {
        upper = transform.Find("Player Upper").GetComponent<SpriteRenderer>();
        lower = transform.Find("Player Lower").GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {

    }
}
