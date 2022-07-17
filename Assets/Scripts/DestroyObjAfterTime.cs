using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjAfterTime : MonoBehaviour
{
    [SerializeField] float time;
    private void Start()
    {
        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
