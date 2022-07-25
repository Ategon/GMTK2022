using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public int playerIndex;

    private SpriteRenderer upper;
    private SpriteRenderer lower;
    private Animator upperAnim;
    private Animator lowerAnim;
    private PlayerState playerState;

    private float throwBuffer = 0.1f;
    private float throwBufferTimer;

    private float throwingTimer;
    private float throwingTime = 0.15f;

    void Awake()
    {

    }


    bool paused;
    float pauseBreak;
    float pauseBreakTimer;

    public PlayerVisualHandling getHandler()
    {
        return new PlayerVisualHandling(this);
    }

    void FixedUpdate()
    {
        throwBufferTimer -= Time.deltaTime;
        throwingTimer -= Time.deltaTime;
        pauseBreakTimer -= Time.deltaTime;
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

        if(state.PlayerState.Pause == true && pauseBreakTimer <= 0)
        {
            if (paused)
            {
                UnityEngine.Cursor.visible = false;
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                pauseBreakTimer = pauseBreak;
            } else
            {
                UnityEngine.Cursor.visible = true;
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                pauseBreakTimer = pauseBreak;
            }
        }

        if (state.PlayerState.Move.x == 0 && state.PlayerState.Move.y == 0)
        {
            if(playerIndex == 1) lowerAnim.Play("scarlet-idle-lower");
            else lowerAnim.Play("clover-idle-lower");

            if (state.PlayerAttackState.throwTriggered)
            {
                throwingTimer = throwingTime;
                if (playerIndex == 1) upperAnim.Play("scarlet-throw");
                else upperAnim.Play("clover-throw");
            }
            else if(throwingTimer < 0)
            {
                if (playerIndex == 1) upperAnim.Play("scarlet-idle");
                else upperAnim.Play("clover-idle");
            }
        }
        else
        {
            if (playerIndex == 1) lowerAnim.Play("scarlet-walk-lower");
            else lowerAnim.Play("clover-walk-lower");
            if (state.PlayerAttackState.throwTriggered)
            {
                throwingTimer = throwingTime;
                if (playerIndex == 1) upperAnim.Play("scarlet-throw");
                else upperAnim.Play("clover-throw");
            }
            else if (throwingTimer < 0)
            {
                if (playerIndex == 1) upperAnim.Play("scarlet-walk");
                else upperAnim.Play("clover-walk");
            }
        }

        Vector3 screenPos = state.sharedData.MainCamera.WorldToScreenPoint(transform.position);

        if (Time.timeScale == 0) return;

        if (state.PlayerState.CursorPos.x - screenPos.x > 0) 
        {
            upper.flipX = false;
            lower.flipX = false;

            if (state.PlayerState.Move.x < 0)
            {
                if (!state.PlayerAttackState.throwTriggered)
                {
                    upperAnim.StartPlayback();
                    upperAnim.speed = -1;
                }
                lowerAnim.StartPlayback();
                lowerAnim.speed = -1;
            } else
            {
                upperAnim.speed = 1;
                lowerAnim.speed = 1;
            }
        }
        else
        {
            upper.flipX = true;
            lower.flipX = true;

            if (state.PlayerState.Move.x > 0)
            {
                if (!state.PlayerAttackState.throwTriggered)
                {
                    upperAnim.StartPlayback();
                    upperAnim.speed = -1;
                }
                lowerAnim.StartPlayback();
                lowerAnim.speed = -1;
            } else
            {
                upperAnim.speed = 1;
                lowerAnim.speed = 1;
            }
        }
    }
}
