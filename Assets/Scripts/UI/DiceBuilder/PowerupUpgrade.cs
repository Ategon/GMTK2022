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

    public void Init(bool ifUnlocked, string text)
    {
        ifLockImage.color = ifUnlocked ? unlockedColor : lockColor;
        upgradeText.text = text;
    }
}
