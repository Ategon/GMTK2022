using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp
{
    public PowerupType powerupType;
    public string name;
    public string description;
    public bool chosenThatTime = false;
    public int level = 0;

    public PowerUp(PowerupType powerupType, string description)
    {
        this.powerupType = powerupType;
        this.name = powerupType.ToString();
        this.description = description;
    }
}

public class PowerUpPool : MonoBehaviour
{

    private List<PowerUp> powerups = new List<PowerUp>();

    private GameObject choice1;
    private GameObject choice2;
    private GameObject choice3;
    private GameObject choice4;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] List<PowerupSettings> powerupSettings;

    // Start is called before the first frame update
    private void Awake()
    {
        powerups.Add(new PowerUp(PowerupType.Fire, "Creates an explosion that deals damage to nearby enemies"));
        powerups.Add(new PowerUp(PowerupType.Ice, "Fires 5 ice spears at nearby enemies, piercing through."));
        powerups.Add(new PowerUp(PowerupType.Poison, "Spawns a pool of poison which poisons enemies trapped inside."));
        powerups.Add(new PowerUp(PowerupType.Lightning, "Lightning strikes down on random enemies dealing damage to all those around."));
        powerups.Add(new PowerUp(PowerupType.MagicCircle, "Spawns a magic circle that slows enemies and hastens player attack speed."));
        powerups.Add(new PowerUp(PowerupType.Gravity, "Creates a black hole, pulling all enemies near inside."));

        FillTextWithPowerUps();

        UpdatePowerUps();
    }

    public void FillTextWithPowerUps()
    {
        choice1 = FetchHelper("Choice1");
        choice2 = FetchHelper("Choice2");
        choice3 = FetchHelper("Choice3");
        choice4 = FetchHelper("Choice4");

        //To insure a powerup won't be chosen twice on the same round.
        foreach (var powerUp in powerups)
        {
            powerUp.chosenThatTime = false;
        }
    }

    public GameObject FetchHelper(string choice)
    {
        GameObject choiceHelper = transform.Find("Panel").Find("choices").Find(choice).gameObject;
        int fetched = FetchPowerUp();
        string fetchedPowerup = powerups[fetched].name;
        if (powerups[fetched].level > 0)
        {
            fetchedPowerup += "++";
        }
        choiceHelper.GetComponentInChildren<TextMeshProUGUI>().text = fetchedPowerup;
        choiceHelper.transform.Find("Image").GetComponent<Image>().sprite = sprites[fetched];

        return choiceHelper;
    }
    
    private int FetchPowerUp()
    {
        string fetchedPowerUp;
        int randomValue = 0;

        do
        {
            randomValue = Random.Range(0, powerups.Count);
        }
        while (powerups[randomValue].chosenThatTime != false);

        powerups[randomValue].chosenThatTime = true;

        return randomValue;
    }

    public void choosePowerup(int buttonId)
    {
        switch (buttonId)
        {
            case 1:
                powerups[powerups.FindIndex(x => x.name == choice1.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice1.GetComponentInChildren<TextMeshProUGUI>().text)].level++;
                break;
            case 2:
                powerups[powerups.FindIndex(x => x.name == choice2.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice2.GetComponentInChildren<TextMeshProUGUI>().text)].level++;
                break;
            case 3:
                powerups[powerups.FindIndex(x => x.name == choice3.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice3.GetComponentInChildren<TextMeshProUGUI>().text)].level++;
                break;
            case 4:
            default:
                powerups[powerups.FindIndex(x => x.name == choice4.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice4.GetComponentInChildren<TextMeshProUGUI>().text)].level++;
                break;
        }

        UpdatePowerUps();

        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    [ContextMenu("Reset Powerups")]
    public void UpdatePowerUps()
    {
        foreach (PowerupSettings effectSetting in powerupSettings)
        {
            foreach (PowerUp powerUp in powerups)
            {
                if (effectSetting.powerupType == powerUp.powerupType)
                {
                    switch (effectSetting.powerupType)
                    {
                        case PowerupType.Fire: effectSetting.floatMultiplier = 1 + 0.2f * powerUp.level; break;
                        case PowerupType.Ice: effectSetting.floatMultiplier = 1 + 0.2f * powerUp.level; break;
                        case PowerupType.Poison: effectSetting.floatMultiplier = 1 + 0.2f * powerUp.level; break;
                        case PowerupType.Lightning:
                            effectSetting.floatMultiplier = 1;
                            effectSetting.intValue = 3 + powerUp.level; 
                            break;
                        case PowerupType.MagicCircle: effectSetting.floatMultiplier = 1 + 0.2f * powerUp.level; break;
                        case PowerupType.Gravity: /* Do Nothing */ break;
                        default: Debug.LogError("PowerUpPool.UpdatePowerUps(): Can't find name"); break;
                    }

                    effectSetting.level = powerUp.level;
                    effectSetting.ifEnabled = powerUp.level != 0;
                }

            }
        }
    }

    public void hoverOption(int buttonId)
    {
        switch (buttonId)
        {
            case 1:
                transform.Find("Panel2").Find("DescText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice1.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice1.GetComponentInChildren<TextMeshProUGUI>().text)].description;
                transform.Find("Panel2").Find("TitleText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice1.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice1.GetComponentInChildren<TextMeshProUGUI>().text)].name;
                break;
            case 2:
                transform.Find("Panel2").Find("DescText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice2.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice2.GetComponentInChildren<TextMeshProUGUI>().text)].description;
                transform.Find("Panel2").Find("TitleText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice2.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice2.GetComponentInChildren<TextMeshProUGUI>().text)].name;
                break;
            case 3:
                transform.Find("Panel2").Find("DescText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice3.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice3.GetComponentInChildren<TextMeshProUGUI>().text)].description;
                transform.Find("Panel2").Find("TitleText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice3.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice3.GetComponentInChildren<TextMeshProUGUI>().text)].name;
                break;
            case 4:
            default:
                transform.Find("Panel2").Find("DescText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice4.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice4.GetComponentInChildren<TextMeshProUGUI>().text)].description;
                transform.Find("Panel2").Find("TitleText").GetComponent<TextMeshProUGUI>().text = powerups[powerups.FindIndex(x => x.name == choice4.GetComponentInChildren<TextMeshProUGUI>().text || x.name + "++" == choice4.GetComponentInChildren<TextMeshProUGUI>().text)].name;
                break;
        }
    }
}
