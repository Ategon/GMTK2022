using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class CanvasScreens : MonoBehaviour, IHandler<PlayerInteractionState>
{
    [SerializeField] GameObject victoryScreen;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        if (data.GameState.numRounds == data.GameState.roundsElapsed && data.GameState.numRounds != 0)
        {
            UnityEngine.Cursor.visible = true;
            victoryScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            victoryScreen.SetActive(false);
        }
    }
}
