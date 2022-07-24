using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPowerup : MonoBehaviour
{
    public int NumOfStrikes;
    public float Damage;

    public GameObject lightningPrefab;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        List<Collider> colliders = mainCamera.transform.GetComponentInChildren<StoreColliders>().colliders;

        // Limit the number of strikes if there isn't enough enemies
        int WorkingNumOfStrikes = Mathf.Min(NumOfStrikes, colliders.Count);

        int[] randomIndexOfColliders = new int[WorkingNumOfStrikes];

        for (int i = 0; i < NumOfStrikes; )
        {
            int randomIndex = Random.Range(0, colliders.Count);

            // Check if it is already in the array
            foreach (int j in randomIndexOfColliders)
            {
                // Set to -1 to indicate 
                if (j == randomIndex)
                {
                    randomIndex = -1;
                    break;
                }
            }

            // If it isn't in the array,
            // Assign it to the array and increment i
            if (randomIndex != -1)
                randomIndexOfColliders[i++] = randomIndex;
        }

        foreach (int i in randomIndexOfColliders)
        {
            Collider collider = colliders[i];

            if (collider == null)
                continue;

            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Instantiate(lightningPrefab, enemy.transform.position, Quaternion.identity);
                enemy.TakeDamage(Damage);
            }
        }
    }
}
