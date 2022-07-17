using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    [SerializeField] int NumOfStrikes;

    private List<GameObject> entitiesToStrike = new List<GameObject>();
    private List<int> strikedEnemies; // Index of striked enemies in entitiesToStrike

    private void Update()
    {
        //strikedEnemies.
    }

    private void OnTriggerEnter(Collider other)
    {
        entitiesToStrike.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        entitiesToStrike.Remove(other.gameObject);
    }
}
