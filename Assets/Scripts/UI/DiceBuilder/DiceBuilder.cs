using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DiceBuilder : MonoBehaviour
{
    [System.Serializable]
    public class DiceData {
        public int numSides;
        public GameObject dicePrefab;
        [ReadOnly]
        public PowerupSettings[] equippedPowerups;
    }
    
    public PowerupSettings[] availablePowerups;
    public DiceData[] diceSelection;

    [Header("Prefabs")]
    public GameObject diceSelectionItemPrefab;
    public GameObject equippedPowerupItemPrefab;
    public GameObject availablePowerupItemPrefab;

    [Header("Parent Transforms")]
    public RectTransform diceSelectionParent;
    public RectTransform equippedPowerupsParent;
    public RectTransform availablePowerupsParent;

    [Header("Others")]
    public PowerupInfoUI powerupDescription;

    [SerializeField] PowerupInfoUI powerupInfo;

    [SerializeField] RectTransform selectedDiceIndicator;
    [SerializeField] RectTransform selectedDiceConnector;

    [SerializeField] RectTransform selectedEquippedPowerupIndicator;
    [SerializeField] RectTransform selectedAvailablePowerupIndicator;

    [SerializeField] [ReadOnly]
    public DiceData selectedDice;
    [SerializeField] [ReadOnly] PowerupSettings selectedDicePowerup;

    private EquippedPowerupSlot[] equippedPowerupSlots;
    private AvailablePowerupItem[] availablePowerupItems;

    // Singleton
    public static DiceBuilder instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than 1 DiceBuilder. Destroying this. Name: " + name);
            return;
        }
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        ResetDice();
    }

    public void OpenDiceBuilder()
    {
        gameObject.SetActive(true);

        UpdateEquippedPowerups();
        UpdateAvailablePowerups();

        UnityEngine.Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void CloseDiceBuilder()
    {
        gameObject.SetActive(false);

        UnityEngine.Cursor.visible = false;

        Time.timeScale = 1f;
    }

    // When drag and dropped a powerup into one of the equipped powerup slots
    public void ChangeEquippedPowerup(int dicePip, PowerupSettings powerup)
    {
        selectedDice.equippedPowerups[dicePip] = powerup;

        UpdateEquippedPowerups();
        UpdateAvailablePowerups();

        // Select the equipped powerup from the new equipped powerups generated from UpdateEquippedPowerups()
        foreach (RectTransform rectTransform in equippedPowerupsParent)
        {
            DicePowerupDataUI dicePowerupDataUI = rectTransform.GetComponent<DicePowerupDataUI>();
            if (dicePowerupDataUI != null && dicePowerupDataUI.powerupSettings == powerup)
                dicePowerupDataUI.OnSelect(null);
        }
    }

    public void ChangeSelectedDice(DiceData selectedDice, RectTransform rectTransform)
    {
        this.selectedDice = selectedDice;

        UpdateEquippedPowerups();
        UpdateAvailablePowerups();

        selectedDiceIndicator.anchoredPosition = rectTransform.anchoredPosition;
        selectedDiceConnector.position = new Vector3(
            selectedDiceConnector.position.x, 
            rectTransform.position.y,
            selectedDiceConnector.position.z);
    }

    public void OnSelectPowerup(PowerupSettings powerupSettings, Vector2 anchoredPos, bool ifEquippedPowerup)
    {
        powerupInfo.OnSelectPowerupSettings(powerupSettings);

        if (ifEquippedPowerup)
        {
            selectedEquippedPowerupIndicator.gameObject.SetActive(true);
            selectedEquippedPowerupIndicator.anchoredPosition = anchoredPos;

            selectedAvailablePowerupIndicator.gameObject.SetActive(false);
        }
        else
        {
            selectedAvailablePowerupIndicator.gameObject.SetActive(true);
            selectedAvailablePowerupIndicator.anchoredPosition = anchoredPos;

            selectedEquippedPowerupIndicator.gameObject.SetActive(false);
        }
    }

    public void OnDeselectPowerup()
    {
        powerupInfo.OnDeselectPowerupSettings();

        DisablePowerupSelectionIndicators();
    }

    public void DisablePowerupSelectionIndicators()
    {
        selectedEquippedPowerupIndicator.gameObject.SetActive(false);
        selectedAvailablePowerupIndicator.gameObject.SetActive(false);
    }

    private void Init()
    {
        selectedDice = diceSelection[0];

        ResetDice();

        InitDiceSelection();
        InitEquippedPowerups();
        InitAvailablePowerups();

        gameObject.SetActive(false);
    }

    private void InitDiceSelection()
    {
        for (int i = 0; i < diceSelection.Length; i++)
        {
            GameObject diceGameObj = Instantiate(diceSelectionItemPrefab, diceSelectionParent);
            diceGameObj.GetComponent<DiceSelectionItem>().Init(diceSelection[i]);
        }
    }

    private void InitEquippedPowerups()
    {
        int highestDiceSides = -1;

        foreach (DiceData diceData in diceSelection)
        {
            if (diceData.numSides > highestDiceSides)
                highestDiceSides = diceData.numSides;
        }

        equippedPowerupSlots = new EquippedPowerupSlot[highestDiceSides];

        for (int i = 0; i < highestDiceSides; i++)
        {
            GameObject diceGameObj = Instantiate(equippedPowerupItemPrefab, equippedPowerupsParent);

            PowerupSettings powerupSettings = null;
            if (i < selectedDice.numSides)
                powerupSettings = selectedDice.equippedPowerups[i];
            else
                diceGameObj.SetActive(false);

            EquippedPowerupSlot equippedPowerupSlot = diceGameObj.GetComponent<EquippedPowerupSlot>();
            equippedPowerupSlot.Init(i, powerupSettings);
            equippedPowerupSlots[i] = equippedPowerupSlot;
        }
    }

    private void UpdateEquippedPowerups()
    {
        for (int i = 0; i < equippedPowerupSlots.Length; i++)
        {
            if (i < selectedDice.numSides)
            {
                equippedPowerupSlots[i].Init(i, selectedDice.equippedPowerups[i]);
                equippedPowerupSlots[i].gameObject.SetActive(true);
            }
            else
            {
                equippedPowerupSlots[i].dicePowerupDataUI.powerupSettings = null;
                equippedPowerupSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void InitAvailablePowerups()
    {
        availablePowerupItems = new AvailablePowerupItem[availablePowerups.Length];

        for (int i = 0; i < availablePowerups.Length; i++)
        {
            GameObject diceGameObj = Instantiate(availablePowerupItemPrefab, availablePowerupsParent);
            AvailablePowerupItem availablePowerupItem = diceGameObj.GetComponent<AvailablePowerupItem>();
            availablePowerupItem.Init(availablePowerups[i]);
            availablePowerupItems[i] = availablePowerupItem;
        }
    }

    private void UpdateAvailablePowerups()
    {
        for (int i = 0; i < availablePowerupItems.Length; i++)
        {
            // If the player is already equipped with the powerup, don't show it
            if (System.Array.Exists(selectedDice.equippedPowerups, x => x == availablePowerups[i]))
            {
                availablePowerupItems[i].gameObject.SetActive(false);
            }
            else
            {
                availablePowerupItems[i].Init(availablePowerups[i]);
                availablePowerupItems[i].gameObject.SetActive(true);
            }
        }
    }

    private void ResetDice()
    {
        foreach (DiceData diceData in diceSelection)
        {
            diceData.equippedPowerups = new PowerupSettings[diceData.numSides];
        }
    }
}
