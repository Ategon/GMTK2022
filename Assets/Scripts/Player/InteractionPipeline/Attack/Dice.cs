using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    DiceAttackSettings diceSettings;
    float remainingLifetime;
    DiceEffectSettings[] equippedDiceEffects;

    ObjectPool dicePool; // To return itself to the pool 

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Reset the die
    private void OnDisable()
    {
        remainingLifetime = -1f;

        // Reset rigidbody

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Init(DiceAttackSettings _diceSettings, DiceEffectSettings[] _equippedDiceEffects, ObjectPool _dicePool, Vector3 throwDirection)
    {
        diceSettings = _diceSettings;
        equippedDiceEffects = _equippedDiceEffects;

        remainingLifetime = diceSettings.Lifetime;

        dicePool = _dicePool;
        
        rb.AddForce(throwDirection * diceSettings.Speed, ForceMode.VelocityChange);
        rb.angularVelocity = Random.onUnitSphere * Random.Range(diceSettings.SpinSpeedRange.x, diceSettings.SpinSpeedRange.y);
    }

    private void Update()
    {
        remainingLifetime -= Time.deltaTime;

        rb.angularVelocity -= rb.angularVelocity * diceSettings.SpinSlowDown * Time.deltaTime;

        if (remainingLifetime < 0f)
        {
            int chosenSide = GetRolledNumber();

            SpawnEffect(chosenSide);

            dicePool.Release(this.gameObject);
        }
    }

    // TODO (GnoxNahte): Change to comparing the up vector 
    private int GetRolledNumber()
    {
        return Random.Range(0, DiceAttackSettings.numOfSides - 1);
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
