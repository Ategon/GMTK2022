using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreColliders : MonoBehaviour
{
    public List<Collider> colliders = new List<Collider>();

    private void Update()
    {
        for (int i = 0; i < colliders.Count;)
        {
            if (colliders[i] == null)
                colliders.RemoveAt(i);
            else
                i++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
}
