using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPopupManager : MonoBehaviour
{
    [SerializeField] GameObject powerupPopupPrefab;

    [SerializeField] float duration;
    [SerializeField] float spawnHeight;

    private static Dictionary<PowerupType, Sprite> powerupGlyphs;

    // Singleton
    public static PowerupPopupManager instance { get; private set; }

    private ObjectPool powerupPopupPool;

    private void Awake()
    {
        if (instance == null)
        {
            Init();

            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than 1 PowerupPopupManager. Destroying this. Name: " + name);
            return;
        }
    }

    private void Init()
    {
        powerupPopupPool = new ObjectPool();
        powerupPopupPool.InitPool(transform, powerupPopupPrefab, 10);

        DiceBuilder diceBuilder = FindObjectOfType<DiceBuilder>();
        powerupGlyphs = new Dictionary<PowerupType, Sprite>(diceBuilder.availablePowerups.Length);
        foreach (PowerupSettings powerup in diceBuilder.availablePowerups)
            powerupGlyphs.Add(powerup.powerupType, powerup.powerupGlyph);
    }

    public static void OnSpawnPowerup(PowerupType powerupType, Vector3 spawnPos)
    {
        GameObject powerupObj = instance.powerupPopupPool.Get();

        spawnPos.y = instance.spawnHeight;
        powerupObj.transform.position = spawnPos;

        powerupObj.GetComponent<PowerupPopup>().Init(instance.powerupPopupPool, powerupGlyphs[powerupType], instance.duration);
    }

}
