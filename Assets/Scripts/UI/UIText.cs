using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public void Select()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(0.9098039f, 0.7568628f, 0.4392157f);
    }

    public void Deselect()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(0.75f, 0.75f, 0.75f);
    }
}
