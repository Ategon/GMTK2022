using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PowerupInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI powerupName;
    [SerializeField] private TextMeshProUGUI powerupDescription;
    [SerializeField] private PowerupUpgrade[] powerupUpgrades;

    private PowerupSettings currPowerupSettings;

    // Singleton
    public static PowerupInfoUI instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than 1 PowerupInfoUI. Destroying this. Name: " + name);
            return;
        }
    }

    public void OnSelectPowerupSettings(PowerupSettings powerupSettings)
    {
        if (powerupSettings == null)
        {
            OnDeselectPowerupSettings();
            return;
        }


        currPowerupSettings = powerupSettings;

        powerupName.text = powerupSettings.effectName;
        powerupDescription.text = powerupSettings.powerupDescription;

        for (int i = 0; i < powerupUpgrades.Length; i++)
        {
            powerupUpgrades[i].Init(i < powerupSettings.level, powerupSettings.powerupUpgradeDescription);
        }
    }

    public void OnDeselectPowerupSettings()
    {
        currPowerupSettings = null;

        powerupName.text = "-";
        powerupDescription.text = "-";

        for (int i = 0; i < powerupUpgrades.Length; i++)
        {
            powerupUpgrades[i].Init(true, "-");
        }
    }
}
