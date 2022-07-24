using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using Spellbound.Managers;

public class BossBar : MonoBehaviour, IHandler<PlayerInteractionState>
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject bar;
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
                text.SetActive(false);
                bar.SetActive(false);
                break;
            case RoundPhase.Boss:
                text.SetActive(true);
                bar.SetActive(true);
                break;
            default:
                break;
        }
    }
}
