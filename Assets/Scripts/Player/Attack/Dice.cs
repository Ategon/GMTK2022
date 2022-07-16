using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    DiceSettingsOld diceSettings;
    float remainingLifetime;
    DiceEffectSettings[] equippedDiceEffects;
    
    public void Init(DiceSettingsOld _diceSettings, DiceEffectSettings[] _equippedDiceEffects)
    {
        diceSettings = _diceSettings;
        equippedDiceEffects = _equippedDiceEffects;

        remainingLifetime = diceSettings.Lifetime;
    }

    private void Update()
    {
        remainingLifetime -= Time.deltaTime;

        if (remainingLifetime < 0f)
        {
            int chosenSide = Random.Range(0, DiceSettingsOld.numOfSides - 1);
            print($"ChosenSide: {chosenSide}");
            SpawnEffect(Random.Range(0, DiceSettingsOld.numOfSides - 1));

            Destroy(this.gameObject);
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
