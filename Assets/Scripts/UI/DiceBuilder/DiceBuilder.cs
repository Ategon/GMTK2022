using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class DiceBuilder : MonoBehaviour
{
    [System.Serializable]
    public class DiceData {
        public int numSides;
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

    [SerializeField] [ReadOnly] DiceData selectedDice;
    [SerializeField] [ReadOnly] PowerupSettings selectedDicePowerup;

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

        ResetDice();
    }

    private void Start()
    {
        powerupDescription = PowerupInfoUI.instance;

        print("InitDIceSelection");
        Init(); // For debug only now (GnoxNahte)
    }

    private void OnDestroy()
    {
        ResetDice();
    }

    public void ChangeEquippedPowerup(int dicePip, PowerupSettings powerup)
    {
        selectedDice.equippedPowerups[dicePip] = powerup;
        UpdateEquippedPowerups();
        UpdateAvailablePowerups();
    }

    public void ChangeSelectedDice(DiceData selectedDice)
    {
        this.selectedDice = selectedDice;

        UpdateDiceSelection();
        UpdateEquippedPowerups();
        UpdateAvailablePowerups();
    }

    private void Init()
    {
        selectedDice = diceSelection[0];

        UpdateDiceSelection();
        UpdateEquippedPowerups();
        UpdateAvailablePowerups();
    }

    private void UpdateDiceSelection()
    {
        DestroyAllChildren(diceSelectionParent);

        foreach (DiceData diceData in diceSelection)
        {
            GameObject diceGameObj = Instantiate(diceSelectionItemPrefab, diceSelectionParent);
            diceGameObj.GetComponent<DiceSelectionItem>().Init(diceData);
        }
    }

    private void UpdateEquippedPowerups()
    {
        DestroyAllChildren(equippedPowerupsParent);
        
        for (int i = 0; i < selectedDice.equippedPowerups.Length; i++)
        {
            PowerupSettings powerupSettings = selectedDice.equippedPowerups[i];
            GameObject diceGameObj = Instantiate(equippedPowerupItemPrefab, equippedPowerupsParent);
            diceGameObj.GetComponent<DicePowerupDataUI>().powerupSettings = powerupSettings;
            diceGameObj.GetComponent<EquippedPowerupSlot>().Init(i, powerupSettings);
        }
    }

    private void UpdateAvailablePowerups()
    {
        DestroyAllChildren(availablePowerupsParent);

        foreach (PowerupSettings powerup in availablePowerups)
        {
            if (System.Array.Exists(selectedDice.equippedPowerups, x => x == powerup))
                continue;

            GameObject diceGameObj = Instantiate(availablePowerupItemPrefab, availablePowerupsParent);
            diceGameObj.GetComponent<DicePowerupDataUI>().powerupSettings = powerup;
            diceGameObj.GetComponent<AvailablePowerupItem>().Init(powerup);
        }
    }

    private void DestroyAllChildren(Transform parentTransform)
    {
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(parentTransform.GetChild(i).gameObject);
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
