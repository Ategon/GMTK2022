using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class DicePowerupDataUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public PowerupSettings powerupSettings;
    RectTransform rectTransform;

    bool ifEquippedPowerupSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (GetComponent<EquippedPowerupSlot>() != null)
            ifEquippedPowerupSlot = true;
        else if (GetComponent<AvailablePowerupItem>() != null)
            ifEquippedPowerupSlot = false;
        // Unknown if is equipped powerup slot or availabe powerup slot.
        // Need to know for yellow selection square
        else
            Debug.LogError("Unknown powerup data type"); 
    }

    public void OnSelect(BaseEventData eventData)
    {
        DiceBuilder.instance.OnSelectPowerup(powerupSettings, rectTransform.anchoredPosition, ifEquippedPowerupSlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DiceBuilder.instance.OnDeselectPowerup();
    }
}
