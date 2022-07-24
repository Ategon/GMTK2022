using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class VictoryScreen : MonoBehaviour, IHandler<PlayerInteractionState>
{
    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        if(data.GameState.numRounds == data.GameState.roundsElapsed)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
