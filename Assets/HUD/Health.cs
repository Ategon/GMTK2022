using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSR;
    [SerializeField] SpriteRenderer playerSR2;
    private float invulnerableTime = 2;
    private float invulnerableTimer;
    private float flashingTimer;
    private bool dashFlash;
    private int collidingEnemies;

    [SerializeField] private Image chipbar;

    private int exp;
    private int level;

    [SerializeField]
    private int health;
    [SerializeField]
    private int numOfHearts;

    [SerializeField]
    private Image[] hearts;
    
    [SerializeField]
    [Range(0, 3)]
    private int chosenCharacterIndex = 0;

    [SerializeField]
    private Sprite[] fullHearts;

    [SerializeField]
    private Sprite[] emptyHearts;

    [SerializeField]
    private Canvas canvas;

    private Animator animator;

    private float healthBlinkTimer;
    private int healthBlinkIndex;

    private float healthBlinkMult = 2;

    private void FixedUpdate()
    {
        healthBlinkTimer += Time.deltaTime * healthBlinkMult;
        healthBlinkIndex = (int) Math.Floor(healthBlinkTimer) % 4;
        if((int) Math.Round(healthBlinkTimer) % 4 == healthBlinkIndex) hearts[healthBlinkIndex].rectTransform.sizeDelta = new Vector2(120, 120);
        else hearts[healthBlinkIndex].rectTransform.sizeDelta = new Vector2(80, 80);
        hearts[(healthBlinkIndex + 3) % 4].rectTransform.sizeDelta = new Vector2(100, 100);
        hearts[(healthBlinkIndex + 2) % 4].rectTransform.sizeDelta = new Vector2(100, 100);
        hearts[(healthBlinkIndex + 1) % 4].rectTransform.sizeDelta = new Vector2(100, 100);

        chipbar.rectTransform.sizeDelta = new Vector2(640 * (float) exp / (100 + (20 * (level + 1))), 16);
        chipbar.transform.localPosition = new Vector3((float) exp / (100 + 20 * (level + 1)) * 320 - 320, 0, 0);

        if (flashingTimer > 0)
        {
            flashingTimer -= Time.deltaTime;
        }

        if (invulnerableTimer > 0)
        {
            invulnerableTimer -= Time.deltaTime;

            if (flashingTimer <= 0)
            {
                if (dashFlash)
                {
                    dashFlash = false;
                    flashingTimer = 0.25f;
                    playerSR.color = new Color(1, 1, 1);
                    playerSR2.color = new Color(1, 1, 1);
                }
                else
                {
                    dashFlash = true;
                    flashingTimer = 0.25f;
                    playerSR.color = new Color(0, 0, 0);
                    playerSR2.color = new Color(0, 0, 0);
                }
            }

            if (invulnerableTimer <= 0)
            {
                dashFlash = false;
                playerSR.color = new Color(1, 1, 1);
                playerSR2.color = new Color(1, 1, 1);
            }
        }

        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        if (health == 0)
        {
            //TODO - play dead animation
            canvas.GetComponent<PauseMenu>().Defeat();
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHearts[chosenCharacterIndex];
            } else {
                hearts[i].sprite = emptyHearts[chosenCharacterIndex];
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void OnCollisionStay(Collision hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            if (invulnerableTimer <= 0)
            {
                health--;
                invulnerableTimer = invulnerableTime;
            }
        }
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            if (invulnerableTimer <= 0)
            {
                health--;
                invulnerableTimer = invulnerableTime;
            }
        }
        else if (hit.gameObject.tag == "EXP")
        {
            exp += 20;
            Destroy(hit.gameObject);
        }
    }
}
