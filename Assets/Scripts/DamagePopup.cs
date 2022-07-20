using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DamagePopup : MonoBehaviour
{
    float duration;
    float remainingDuration;
    [SerializeField] float moveUpSpeed;

    TextMeshPro text;
    ObjectPool objectPool;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Init(ObjectPool objectPool, float damageAmount, float duration)
    {
        this.objectPool = objectPool;
        text.text = damageAmount.ToString();

        this.duration = duration;
        this.remainingDuration = duration;
    }

    private void Update()
    {
        remainingDuration -= Time.deltaTime;
        if (remainingDuration < 0)
        {
            objectPool.Release(this.gameObject);
            return;
        }
        transform.localPosition += new Vector3(0, 0, moveUpSpeed * Time.deltaTime);
        transform.localScale = Vector3.one * remainingDuration / duration;
    }
}
