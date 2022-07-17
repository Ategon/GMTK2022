using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp
{
    public string name;
    public int level;
    public bool chosenThatTime;
    public string description;
}

public class PowerUpPool : MonoBehaviour
{

    private List<PowerUp> powerups = new List<PowerUp>();

    private GameObject choice1;
    private GameObject choice2;
    private GameObject choice3;
    private GameObject choice4;

    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    private void Awake()
    {
        powerups.Add(new PowerUp { name = "Fire", level = 0, chosenThatTime = false, description = "Explosion, AOE damage, very slightly displacement. Only scales damage." });
        powerups.Add(new PowerUp { name = "Ice", level = 0, chosenThatTime = false, description = "Shoots out ice spears at enemy (5 projectiles). Only Scales damage." });
        powerups.Add(new PowerUp { name = "Poison", level = 0, chosenThatTime = false, description = "Puddle of poison, applies DOT effect to enemies (last 3 seconds. Refreshes while still inside pool). Only scales damage" });
        powerups.Add(new PowerUp { name = "Lightning", level = 0, chosenThatTime = false, description = "Lighting strikes random enemies on screen dealing small AOE damage to all enemies around it. . 3 projectiles to start, scales to 5" });
        powerups.Add(new PowerUp { name = "Time", level = 0, chosenThatTime = false, description = "Creates a circle around your die. Enemies inside the zone are slowed and player attack speed is increased while inside. Only scales in effect strength (Aka enemy movement slow % and player attack speed %)" });
        powerups.Add(new PowerUp { name = "Air", level = 0, chosenThatTime = false, description = "Displacement ability, Pushes enemies away from your die." });

        FillTextWithPowerUps();
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

        Time.timeScale = 1;
        gameObject.SetActive(false);
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