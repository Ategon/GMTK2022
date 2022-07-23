using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataPipeline;
using Spellbound.Managers;

public class RoundIcon : MonoBehaviour, IHandler<PlayerInteractionState>
{
    private Image roundIcon;

    private void Start()
    {
        roundIcon = GetComponent<Image>();
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        switch (data.GameState.roundPhase)
        {
            case RoundPhase.Wave:
                break;
            case RoundPhase.Boss:
                break;
            default:
                break;
        }
    }
}
