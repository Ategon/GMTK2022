using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using Spellbound.Managers;

public class BossBar : MonoBehaviour, IHandler<PlayerInteractionState>
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        switch (data.GameState.roundPhase)
        {
            case RoundPhase.Wave:
                gameObject.SetActive(false);
                break;
            case RoundPhase.Boss:
                gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
