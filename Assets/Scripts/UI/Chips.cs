using TMPro;
using UnityEngine;

public class Chips : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    [SerializeField]
    private int numberOfChips;
    
    private void Awake()
    {
        numberOfChips = 0;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "Chips : ";
    }
    
    private void Update()
    {
        textMeshPro.text = "Chips : " + numberOfChips;
    }
}
