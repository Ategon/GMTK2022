using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class EquippedPowerupSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Sprite lockedGlyph;

    UnityEngine.UI.Image glyph;
    int pipNumber;

    public DicePowerupDataUI dicePowerupDataUI;

    private void Awake()
    {
        glyph = GetComponent<UnityEngine.UI.Image>();
        dicePowerupDataUI = GetComponent<DicePowerupDataUI>();
    }

    public void Init(int pipNumber, PowerupSettings powerupSettings)
    {
        this.pipNumber = pipNumber;

        if (powerupSettings == null)
            glyph.sprite = lockedGlyph;
        else
            glyph.sprite = powerupSettings.powerupGlyph;

        dicePowerupDataUI.powerupSettings = powerupSettings;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            AvailablePowerupItem availablePowerupItem = eventData.pointerDrag.GetComponent<AvailablePowerupItem>();
            if (availablePowerupItem != null)
            {
                DiceBuilder.instance.ChangeEquippedPowerup(pipNumber, availablePowerupItem.dicePowerupDataUI.powerupSettings);
                // Need to set here because 
                availablePowerupItem.OnEndDrag(null);
            }

        }

    }
}
