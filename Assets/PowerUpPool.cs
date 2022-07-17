using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUp
{
    public string name;
    public int level;
    public bool chosenThatTime;
}

public class PowerUpPool : MonoBehaviour
{

    private List<PowerUp> powerups = new List<PowerUp>();
    private GameObject choice1;
    private GameObject choice2;
    private GameObject choice3;
    private GameObject choice4;

    // Start is called before the first frame update
    private void Awake()
    {
        powerups.Add(new PowerUp { name = "air",          level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "posession",    level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "shield",       level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "regeneration", level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "fire",         level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "time",         level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "ice",          level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "evil",         level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "poison",       level = 0, chosenThatTime = false });
        powerups.Add(new PowerUp { name = "iron",         level = 0, chosenThatTime = false });

        FillTextWithPowerUps();
    }

    public void FillTextWithPowerUps()
    {
        choice1 = GameObject.Find("Choice1");
        choice2 = GameObject.Find("Choice2");
        choice3 = GameObject.Find("Choice3");
        choice4 = GameObject.Find("Choice4");

        choice1.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();
        choice2.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();
        choice3.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();
        choice4.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();

        //To insure a powerup won't be chosen twice on the same round.
        foreach (var powerUp in powerups)
        {
            powerUp.chosenThatTime = false;
        }
    }
    
    private string FetchPowerUp()
    {
        string fetchedPowerUp;
        
        int randomValue = 0;

        do
        {
            randomValue = Random.Range(0, 9);
        }
        while (powerups[randomValue].chosenThatTime != false);

        fetchedPowerUp = powerups[randomValue].name;
        
        //Logic problem : don't want to repeat twice despite that code
        if (powerups[randomValue].level > 0 && powerups[randomValue].chosenThatTime == false)
        {
            fetchedPowerUp += "++";
        }

        powerups[randomValue].level++;
        powerups[randomValue].chosenThatTime = true;


        return fetchedPowerUp;
    }

    

}
