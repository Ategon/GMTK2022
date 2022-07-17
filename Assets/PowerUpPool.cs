using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUp
{
    public string name;
    public bool chosen;
}

public class PowerUpPool : MonoBehaviour
{

    private List<PowerUp> powerups = new List<PowerUp>();

    // Start is called before the first frame update
    private void Awake()
    {
        //Time.timeScale = 0f;
        powerups.Add(new PowerUp { name = "air",          chosen = false });
        powerups.Add(new PowerUp { name = "posession",    chosen = false });
        powerups.Add(new PowerUp { name = "shield",       chosen = false });
        powerups.Add(new PowerUp { name = "regeneration", chosen = false });
        powerups.Add(new PowerUp { name = "fire",         chosen = false });
        powerups.Add(new PowerUp { name = "time",         chosen = false });
        powerups.Add(new PowerUp { name = "ice",          chosen = false });
        powerups.Add(new PowerUp { name = "evil",         chosen = false });
        powerups.Add(new PowerUp { name = "poison",       chosen = false });
        powerups.Add(new PowerUp { name = "iron",         chosen = false });


        GameObject choice1 = GameObject.Find("Choice1");
        GameObject choice2 = GameObject.Find("Choice2");
        GameObject choice3 = GameObject.Find("Choice3");
        GameObject choice4 = GameObject.Find("Choice4");

        choice1.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();
        choice2.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();
        choice3.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();
        choice4.GetComponentInChildren<TextMeshProUGUI>().text = FetchPowerUp();

        /*string test = FetchPowerUp();
        Debug.Log(test);*/
    }

    public string FetchPowerUp()
    {
        int randomValue = Random.Range(0, 9);
        do
        {
            randomValue = Random.Range(0, 9);
        }
        while (powerups[randomValue].chosen != false);

        powerups[randomValue].chosen = true;
        
        return powerups[randomValue].name;
    }

    

}
