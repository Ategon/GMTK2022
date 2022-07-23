using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class DiceSelectionItem : MonoBehaviour, ISelectHandler
{
    public DiceBuilder.DiceData diceData { get; private set; }

    public void Init(DiceBuilder.DiceData diceData)
    {
        this.diceData = diceData;
    }

    public void OnSelect(BaseEventData eventData)
    {
        DiceBuilder.instance.ChangeSelectedDice(diceData);
    }
}
