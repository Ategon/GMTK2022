using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    DiceSettingsOld diceSettings;
    float remainingLifetime;
    DiceEffectSettings[] equippedDiceEffects;

    ObjectPool dicePool; // To return itself to the pool 

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        // Reset it
        remainingLifetime = -1f;

        // Reset rigidbody

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Init(DiceSettingsOld _diceSettings, DiceEffectSettings[] _equippedDiceEffects, ObjectPool _dicePool, Vector3 throwDirection)
    {
        diceSettings = _diceSettings;
        equippedDiceEffects = _equippedDiceEffects;

        remainingLifetime = diceSettings.Lifetime;

        dicePool = _dicePool;
        
        rb.AddForce((throwDirection + Vector3.up * 0.5f) * diceSettings.Speed, ForceMode.VelocityChange);
        rb.angularVelocity = Random.onUnitSphere * Random.Range(diceSettings.SpinSpeedRange.x, diceSettings.SpinSpeedRange.y);
    }

    private void Update()
    {
        remainingLifetime -= Time.deltaTime;

        rb.angularVelocity -= rb.angularVelocity * diceSettings.SpinSlowDown * Time.deltaTime;

        if (remainingLifetime < 0f)
        {
            int chosenSide = Random.Range(0, DiceSettingsOld.numOfSides - 1);
            print($"ChosenSide: {chosenSide}");
            SpawnEffect(Random.Range(0, DiceSettingsOld.numOfSides - 1));

            dicePool.Release(this.gameObject);
        }
    }

    private void SpawnEffect(int numberRolled)
    {
        DiceEffectSettings diceEffect = equippedDiceEffects[numberRolled];

        if (diceEffect == null)
            return;

        // TODO (GnoxNahte): Replace with pool
        GameObject.Instantiate(diceEffect.diceEffectPrefab, transform.position, Quaternion.identity);
    }
}
