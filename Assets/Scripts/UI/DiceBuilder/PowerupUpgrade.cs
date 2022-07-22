using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PowerupUpgrade : MonoBehaviour
{
    [SerializeField] Color lockColor;
    [SerializeField] Color unlockedColor;

    [SerializeField] Image ifLockImage;
    [SerializeField] TextMeshProUGUI upgradeText;

    public void Init(bool ifLocked, string text)
    {
        ifLockImage.color = ifLocked ? lockColor : unlockedColor;
        upgradeText.text = text;
    }
}
