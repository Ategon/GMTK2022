using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class DicePowerupDataUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public PowerupSettings powerupSettings;

    public void OnSelect(BaseEventData eventData)
    {
        PowerupInfoUI.instance.OnSelectPowerupSettings(powerupSettings);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        PowerupInfoUI.instance.OnDeselectPowerupSettings();
    }
}
