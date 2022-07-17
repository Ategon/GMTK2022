using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingTerrain : MonoBehaviour
{
    [SerializeField] Vector3 startingPos;
    [SerializeField] Vector3 maxPos;
    [SerializeField] Vector3 minPos;
    [SerializeField] Vector3 loopingAmount;

    GameObject player;

    Vector3 loops;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        if (player.transform.position.x > maxPos.x + loopingAmount.x * loops.x + startingPos.x)
        {
            transform.position += (Vector3)(Vector3.right * loopingAmount.x);
            loops += Vector3.right;
        }

        if (player.transform.position.x < minPos.x + loopingAmount.x * loops.x + startingPos.x)
        {
            transform.position += (Vector3)(Vector3.left * loopingAmount.x);
            loops += Vector3.left;
        }

        if (player.transform.position.z > maxPos.z + loopingAmount.z * loops.z + startingPos.z)
        {
            transform.position += (Vector3)(Vector3.forward * loopingAmount.z);
            loops += Vector3.forward;
        }

        if (player.transform.position.z < minPos.z + loopingAmount.z * loops.z + startingPos.z)
        {
            transform.position += (Vector3)(-Vector3.forward * loopingAmount.z);
            loops += -Vector3.forward;
        }
    }
}
