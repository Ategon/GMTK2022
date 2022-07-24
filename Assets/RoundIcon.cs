using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataPipeline;
using Spellbound.Managers;

public class RoundIcon : MonoBehaviour, IHandler<PlayerInteractionState>
{
    private Image roundIcon;
    float bigBreak = 0.5f;
    float bigSize = 0.25f;
    float bigSmallest = -0.25f;
    float bigTimer;
    float sizeBonus;

    private void Start()
    {
        roundIcon = GetComponent<Image>();
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    private void FixedUpdate()
    {
        if (sizeBonus > bigSmallest) sizeBonus -= Time.deltaTime;
        else if (sizeBonus < bigSmallest) sizeBonus = bigSmallest;
        bigTimer += Time.deltaTime;

        if(bigTimer > bigBreak)
        {
            bigTimer -= bigBreak;
            sizeBonus = bigSize;
        }

        transform.localScale = new Vector3(1 + sizeBonus, 1 + sizeBonus, 1);
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
