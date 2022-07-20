using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataPipeline;

public class PlayerAttackGenerator : IGenerator<PlayerInteractionState>
{
    public void Start()
    {

    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        Handle(ref data);
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }

    public void Handle(ref PlayerInteractionState data)
    {
        data.PlayerAttackState.throwTriggered = false;
        Debug.Log("2 playerAttackSpeedMultiplier: " + data.PlayerAttackState.playerAttackSpeedMultiplier);

        float timeBetweenAttacks = 1 / (data.PlayerAttackSettings.AttackSpeed * data.PlayerAttackState.playerAttackSpeedMultiplier);
        if (data.PlayerState.Fire && Time.time - data.PlayerAttackState.lastAttackedTime >= timeBetweenAttacks)
        {
            RaycastHit hitInfo;
            // Calculate the direction to throw the dice
            Ray ray = data.sharedData.MainCamera.ScreenPointToRay(data.PlayerState.CursorPos);
            if (Physics.Raycast(ray, out hitInfo, 1000000, data.PlayerAttackSettings.MouseRaycastLayerMask.value))
            {
                Vector3 shootDir = hitInfo.point - data.sharedData.PlayerTransform.position;
                shootDir.y = 0f;
                if (shootDir.sqrMagnitude < Mathf.Epsilon)
                    data.PlayerAttackState.ShootDir = Vector3.zero;
                else
                    data.PlayerAttackState.ShootDir = shootDir.normalized;

                data.PlayerAttackState.lastAttackedTime = Time.time;

                data.PlayerAttackState.throwTriggered = true;

                return;
            }
        }

        data.PlayerAttackState.ShootDir = Vector3.zero;

    }
}
