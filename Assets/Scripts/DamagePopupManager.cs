using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] GameObject damagePopupPrefab;

    // Singleton
    public static DamagePopupManager instance { get; private set; }

    static ObjectPool damagePopupPool;

    private void Awake()
    {
        if (instance == null)
        {
            damagePopupPool = new ObjectPool();
            damagePopupPool.InitPool(transform, damagePopupPrefab, 30);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than 1 DamagePopupManager. Destroying this. Name: " + name);
            return;
        }
    }

    public static void OnDamage(Vector3 position, float damageAmount)
    {
        GameObject damagePopupObj = damagePopupPool.Get();
        damagePopupObj.transform.position = position;
        damagePopupObj.GetComponent<DamagePopup>().Init(damagePopupPool, damageAmount, 1);
    }
}
