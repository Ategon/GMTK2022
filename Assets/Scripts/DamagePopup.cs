using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Init(float damageAmount)
    {
        text.text = damageAmount.ToString();
    }
}
