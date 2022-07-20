using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerAttackHandler : IHandler<PlayerInteractionState>
{
    public void Handle(in PlayerInteractionState data)
    {
        if (data.PlayerAttackState.ShootDir != Vector3.zero)
        {
            Vector3 shootDir = data.PlayerAttackState.ShootDir;
            GameObject diceObj = data.PlayerAttackState.dicePool.Get();
            //diceObj.SetActive(true);
            diceObj.transform.position = data.sharedData.PlayerTransform.position + shootDir;
            diceObj.transform.rotation = Random.rotationUniform;
            Dice dice = diceObj.GetComponent<Dice>();
            dice.Init(data.DiceAttackSettings, data.PlayerAttackState.equippedEffects, data.PlayerAttackState.dicePool, shootDir);
        }
    }
}
