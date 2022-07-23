using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvailablePowerupItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    GameObject lockedImage;

    private RectTransform rectTransform;
    private Transform originalParent;
    private DiceBuilder diceBuilder;
    private Canvas canvas;
    private Image image;
    private Button button;
    public DicePowerupDataUI dicePowerupDataUI { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        diceBuilder = GetComponentInParent<DiceBuilder>();
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        dicePowerupDataUI = GetComponent<DicePowerupDataUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        diceBuilder.DisablePowerupSelectionIndicators();
        image.raycastTarget = false;
        rectTransform.SetParent(diceBuilder.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        rectTransform.SetParent(originalParent.transform);

        // If eventData == null, means the player put the powerup item into one of the equipped slots
        // Else, it means the player dropped it in a place with no IDropHandler, so select the dice 
        if (eventData != null)
        {
            // Wait for next frame when the layout group refreshes
            StartCoroutine(WaitForNextFrame(()=>dicePowerupDataUI.OnSelect(null)));
        }

        gameObject.SetActive(eventData != null);
    }

    public void Init(PowerupSettings powerupSettings)
    {
        image.sprite = powerupSettings.powerupGlyph;

        button.interactable = powerupSettings.ifEnabled;
        image.raycastTarget = powerupSettings.ifEnabled;
        lockedImage.SetActive(!powerupSettings.ifEnabled);

        dicePowerupDataUI.powerupSettings = powerupSettings;
    }

    private IEnumerator WaitForNextFrame(System.Action action)
    {
        yield return null;
        action.Invoke();
    }
}
