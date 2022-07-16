using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public void Select()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1);
    }

    public void Deselect()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(0.75f, 0.75f, 0.75f);
    }
}
