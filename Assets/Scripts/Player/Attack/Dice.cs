using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    DiceSettingsOld diceSettings;
    float remainingLifetime;
    DiceEffectSettings[] equippedDiceEffects;

    ObjectPool dicePool; // To return itself to the pool 
    
    public void Init(DiceSettingsOld _diceSettings, DiceEffectSettings[] _equippedDiceEffects, ObjectPool _dicePool)
    {
        diceSettings = _diceSettings;
        equippedDiceEffects = _equippedDiceEffects;

        remainingLifetime = diceSettings.Lifetime;

        dicePool = _dicePool;
    }

    private void Update()
    {
        remainingLifetime -= Time.deltaTime;

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
