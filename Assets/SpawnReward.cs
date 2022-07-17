using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnReward : MonoBehaviour
{
    [SerializeField]
    private GameObject dice;

    private void OnCollisionEnter(Collision col)
    {
        //layer 6 = Player
        if (col.gameObject.layer == 6)
        {
            Debug.Log("Collide");

            //Ass animation I guess?

            Instantiate(dice, gameObject.transform.position, gameObject.transform.rotation);

            Destroy(gameObject, 0f);
        }
    }
}
