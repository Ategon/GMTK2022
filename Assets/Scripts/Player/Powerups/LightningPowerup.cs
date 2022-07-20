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

        foreach (Collider collider in colliders)
        {
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
