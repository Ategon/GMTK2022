using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class AvailablePowerupItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    Transform originalParent;
    DiceBuilder diceBuilder;
    Canvas canvas;
    UnityEngine.UI.Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        diceBuilder = GetComponentInParent<DiceBuilder>();
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        rectTransform.SetParent(diceBuilder.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        rectTransform.SetParent(originalParent.transform);
    }

    public void Init(PowerupSettings powerupSettings)
    {
        image.sprite = powerupSettings.powerupGlyph;
    }
}
