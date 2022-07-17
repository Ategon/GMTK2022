using TMPro;
using UnityEngine;

public class ChipsCounter : MonoBehaviour
{
    public int NumberOfChips = 0;
    public int MaxOfChips = 100;
    public int Level = 1;
    public TextMeshProUGUI lvlText;


    private void Awake()
    {
        lvlText = GetComponentInChildren<TextMeshProUGUI>();
        lvlText.text = "Lvl : " + Level;
        GameObject gameObject = GameObject.Find("ChipBarFilling");
    }
    
    private void Update()
    {
        
    }

    public void SetNewMaxOfChips()
    {
        MaxOfChips = MaxOfChips + 20 * Level;

        lvlText.text = "Lvl : " + Level;
    }
}
