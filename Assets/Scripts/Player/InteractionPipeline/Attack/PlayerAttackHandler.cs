using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using DataPipeline;

public class PlayerAttackHandler : IHandler<PlayerInteractionState>
{
    public void Handle(in PlayerInteractionState data)
    {
        if (data.PlayerAttackState.ShootDir != Vector3.zero)
        {
            ObjectPool dicePool = data.PlayerAttackState.dicePools[data.PlayerAttackState.equippedPowerups.Length];
            Vector3 shootDir = data.PlayerAttackState.ShootDir;
            GameObject diceObj = dicePool.Get();
            //diceObj.SetActive(true);
            diceObj.transform.position = data.sharedData.PlayerTransform.position + shootDir;
            diceObj.transform.rotation = Random.rotationUniform;
            Dice dice = diceObj.GetComponent<Dice>();
            dice.Init(data.DiceAttackSettings, data.PlayerAttackState.equippedPowerups, dicePool, shootDir);
            data.sharedData.VirtualCamera.GetComponent<ScreenShakeController>().StartShake(0.1f, 1f);
            data.sharedData.GameAudio.PlaySound("DiceSend", AudioTrackType.PlayerDice);
        }
    }
}
