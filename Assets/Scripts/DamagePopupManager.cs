using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject damagePopupPrefab;

    [SerializeField] StatusEffectColor[] statusEffectColors;

    [SerializeField] float minPopupSize;
    [SerializeField] float maxPopupSize;
    // TODO (GnoxNahte): Rename
    // any damage above maxDamagePopupSize will set the text size to maxPopupSize
    [SerializeField] float maxDamagePopupSize;

    [SerializeField] float duration;

    [SerializeField] float damagePopupOffset;

    [System.Serializable]
    public class StatusEffectColor
    {
        public StatusEffectType type;
        public Color color;
    }

    private static Dictionary<StatusEffectType, Color> statusEffectColors_Dictionary;

    // Singleton
    public static DamagePopupManager instance { get; private set; }

    static ObjectPool damagePopupPool;

    private void Awake()
    {
        if (instance == null)
        {
            damagePopupPool = new ObjectPool();
            damagePopupPool.InitPool(transform, damagePopupPrefab, 30);

            statusEffectColors_Dictionary = new Dictionary<StatusEffectType, Color>();
            foreach (StatusEffectColor statusEffectColor in statusEffectColors)
            {

                statusEffectColors_Dictionary.Add(statusEffectColor.type, statusEffectColor.color);
            }
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than 1 DamagePopupManager. Destroying this. Name: " + name);
            return;
        }
    }

    public static void OnDamage(Vector3 position, float damageAmount, StatusEffectType statusEffectType)
    {
        if (damageAmount == 0f)
            return;

        Color color;
        // Try to get the color for the status effect.
        // If can't get the color, default to no effect
        if (!statusEffectColors_Dictionary.TryGetValue(statusEffectType, out color))
            color = statusEffectColors_Dictionary[StatusEffectType.NoEffect];

        GameObject damagePopupObj = damagePopupPool.Get();
        damagePopupObj.transform.position = position + (position - instance.playerTransform.position).normalized * instance.damagePopupOffset;
        damagePopupObj.GetComponent<DamagePopup>().Init(
            damagePopupPool, damageAmount, instance.duration, 
            Mathf.Lerp(instance.minPopupSize, instance.maxPopupSize, damageAmount / instance.maxDamagePopupSize), 
            in color);
    }
}
