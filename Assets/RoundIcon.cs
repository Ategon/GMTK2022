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
    float sizeBonus = 0.25f;

    private void Start()
    {
        roundIcon = GetComponent<Image>();
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
        StartCoroutine(BlinkIcon());
    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator BlinkIcon()
    {
        while (true)
        {
            if (sizeBonus > bigSmallest) sizeBonus -= Time.fixedDeltaTime * 0.145f;
            else if (sizeBonus < bigSmallest) sizeBonus = bigSmallest;
            bigTimer += Time.fixedDeltaTime * 0.145f;

            if (bigTimer > bigBreak)
            {
                bigTimer -= bigBreak;
                sizeBonus = bigSize;
            }

            transform.localScale = new Vector3(1 + sizeBonus, 1 + sizeBonus, 1);
            yield return null;
        }
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
