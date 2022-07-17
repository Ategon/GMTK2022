using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private SpriteRenderer upper;
    private SpriteRenderer lower;
    private Animator upperAnim;
    private Animator lowerAnim;
    private PlayerState playerState;

    private float throwBuffer = 0.1f;
    private float throwBufferTimer;

    private float throwingTimer;
    private float throwingTime = 0.5f;

    void Awake()
    {

    }

    public PlayerVisualHandling getHandler()
    {
        return new PlayerVisualHandling(this);
    }

    void FixedUpdate()
    {
        throwBufferTimer -= Time.deltaTime;
        throwingTimer -= Time.deltaTime;
    }

    void Start()
    {
        upper = transform.Find("Player Upper").GetComponent<SpriteRenderer>();
        lower = transform.Find("Player Lower").GetComponent<SpriteRenderer>();
        upperAnim = upper.gameObject.GetComponent<Animator>();
        lowerAnim = lower.gameObject.GetComponent<Animator>();
    }

    public void HandleAnimations(in PlayerInteractionState state)
    {
        if (state.PlayerState.Move.x == 0 && state.PlayerState.Move.y == 0)
        {
            lowerAnim.Play("clover-idle-lower");
            if (state.PlayerAttackState.throwTriggered)
            {
                throwingTimer = throwingTime;
                upperAnim.Play("clover-throw");
            }
            else if(throwingTimer < 0)
            {
                upperAnim.Play("clover-idle");
            }
        }
        else
        {
            lowerAnim.Play("clover-walk-lower");
            if (state.PlayerAttackState.throwTriggered)
            {
                throwingTimer = throwingTime;
                upperAnim.Play("clover-throw");
            }
            else if (throwingTimer < 0)
            {
                upperAnim.Play("clover-walk");
            }
        }

        Vector3 screenPos = state.sharedData.MainCamera.WorldToScreenPoint(transform.position);

        if (Time.timeScale == 0) return;

        if (state.PlayerState.CursorPos.x - screenPos.x > 0) 
        {
            upper.flipX = false;
            lower.flipX = false;
        }
        else
        {
            upper.flipX = true;
            lower.flipX = true;
        }
    }
}
