using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPopup : MonoBehaviour
{
    float duration;
    float remainingDuration;
    [SerializeField] float moveUpSpeed;

    SpriteRenderer spriteRenderer;

    ObjectPool objectPool;

    private float baseScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        baseScale = transform.localScale.x;
    }

    public void Init(ObjectPool objectPool, Sprite glyph, float duration)
    {
        this.objectPool = objectPool;
        spriteRenderer.sprite = glyph;

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
        transform.localScale = Vector3.one * baseScale * ((remainingDuration / duration) * 0.5f + 0.5f);

        spriteRenderer.color = new Color(1f, 1f, 1f, remainingDuration / duration);
    }
}