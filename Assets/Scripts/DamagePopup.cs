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
        transform.forward = Camera.main.transform.forward;
    }

    public void Init(ObjectPool objectPool, float damageAmount, float duration, float fontSize, in Color color)
    {
        this.objectPool = objectPool;
        text.text = ((int)damageAmount).ToString();

        this.duration = duration;
        this.remainingDuration = duration;
        text.fontSize = fontSize;
        text.color = color;
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

        text.color = new Color(text.color.r, text.color.g, text.color.b, remainingDuration / duration);
    }
}
