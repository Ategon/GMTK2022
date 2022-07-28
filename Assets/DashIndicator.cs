using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;

public class DashIndicator : MonoBehaviour, IHandler<PlayerInteractionState>
{
    // Start is called before the first frame update
    Vector3 size;

    void Start()
    {
        size = transform.localScale;
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
    }

    public void Handle(in PlayerInteractionState data)
    {
        float coolDown = 1 - ((Time.time - data.PlayerState.LastDodgedTime) / data.EntityMovementSettings.DodgeCooldown);
        coolDown = (coolDown > 0) ? coolDown : 0.0f;
        size.x = coolDown;
        transform.localScale = size;
    }
}
