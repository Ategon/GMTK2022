using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.EventSystems;

public class DiceSelectionItem : MonoBehaviour, ISelectHandler
{
    public DiceBuilder.DiceData diceData { get; private set; }
    [SerializeField] TextMeshProUGUI text;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(DiceBuilder.DiceData diceData)
    {
        this.diceData = diceData;
        text.text = "D" + diceData.numSides.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        DiceBuilder.instance.ChangeSelectedDice(diceData, rectTransform);
    }
}
