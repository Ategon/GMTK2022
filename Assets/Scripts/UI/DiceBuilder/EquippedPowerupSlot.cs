using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class EquippedPowerupSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Sprite lockedGlyph;

    UnityEngine.UI.Image glyph;
    int pipNumber;

    private void Awake()
    {
        glyph = GetComponent<UnityEngine.UI.Image>();
    }

    public void Init(int pipNumber, PowerupSettings powerupSettings)
    {
        this.pipNumber = pipNumber;

        if (powerupSettings == null)
            glyph.sprite = lockedGlyph;
        else
            glyph.sprite = powerupSettings.powerupGlyph;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
            DiceBuilder.instance.ChangeEquippedPowerup(pipNumber, eventData.pointerDrag.GetComponent<DicePowerupDataUI>().powerupSettings);

        print("OnDrop");
    }
}
