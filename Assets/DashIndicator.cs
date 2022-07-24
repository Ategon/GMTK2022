using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class DashIndicator : MonoBehaviour, IHandler<PlayerInteractionState>
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        if (Time.time - data.PlayerState.LastDodgedTime < data.EntityMovementSettings.DodgeCooldown)
        {
            transform.localScale = new Vector3(1 - ((Time.time - data.PlayerState.LastDodgedTime) / data.EntityMovementSettings.DodgeCooldown), 0.1f, 0.1f);
        }
        else
        {
            transform.localScale = new Vector3(0f, 0.1f, 0.1f);
        }
    }
}
