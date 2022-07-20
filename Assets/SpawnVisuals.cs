using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVisuals : MonoBehaviour
{
    [SerializeField] GameObject playerVisuals;

    void Start()
    {
        GameObject selected = GameObject.Find("SelectedId");

        int? playerIndex = selected?.GetComponent<SelectCanvas>()?.userIndex;

        if (playerIndex != null)
        {
            GameObject.Find("Player").GetComponent<Health>().chosenCharacterIndex = (int) playerIndex;

            GameObject.Find("Visuals").GetComponent<PlayerVisuals>().playerIndex = (int) playerIndex;
        }
    }
}
