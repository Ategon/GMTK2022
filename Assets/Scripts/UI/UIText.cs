using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public void Select()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(0.1223745f, 0.1450872f, 0.2075472f);
    }

    public void Deselect()
    {
        GetComponent<TextMeshProUGUI>().color = new Color(0.0627451f, 0.07843138f, 0.1215686f);
    }
}
