using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public float blackholeStrength;

    private List<Rigidbody> entitiesToPullIn = new List<Rigidbody>();


    private void Update()
    {
        // Will make further entities pull in faster
        foreach (Rigidbody rb in entitiesToPullIn)
        {
            if (rb != null)
                rb.AddForce(blackholeStrength * Time.deltaTime * (transform.position - rb.position).normalized);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        entitiesToPullIn.Add(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        entitiesToPullIn.Remove(other.attachedRigidbody);
    }
}
