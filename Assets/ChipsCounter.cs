using System;
using TMPro;
using UnityEngine;

public class ChipsCounter : MonoBehaviour
{
    [SerializeField]
    private int NumberOfChips = 1;

    [SerializeField]
    private float MaxOfChips = 100;

    [SerializeField]
    private int Level = 1;

    [SerializeField]
    private GameObject powerUpCanvas;
    

    private TextMeshProUGUI lvlText;
    private float expectedSizeXPbar = 0;

    private void Awake()
    {
        lvlText = GetComponentInChildren<TextMeshProUGUI>();
        lvlText.text = "Lvl : " + Level;
        ChangeChipBarSize();
    }
    
    private void Update()
    {
        ChangeChipBarSize();

        PauseGameIfLevelUp();
    }

    private void ChangeChipBarSize()
    {
        GameObject chipBarFilling = GameObject.Find("ChipBarFilling");

        RectTransform chip = chipBarFilling.GetComponent<RectTransform>();

        // The size of the chipbar-filling shall be represented with that math : Number of chips * 2 divided by the current max (120 for Level 1)
        expectedSizeXPbar = (NumberOfChips * 2) / MaxOfChips;

        chip.localScale = new Vector3(expectedSizeXPbar, 1, 1);
    }

    public void SetNewMaxOfChips()
    {
        MaxOfChips = MaxOfChips + 20 * Level;

        lvlText.text = "Lvl : " + Level;
    }

    private void PauseGameIfLevelUp()
    {
        if(NumberOfChips == MaxOfChips)
        {
            powerUpCanvas.SetActive(true);
            Time.timeScale = 0f;
            Level++;
            NumberOfChips = 1;
            SetNewMaxOfChips();
        }
    }
}
