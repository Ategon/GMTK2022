using UnityEngine;
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

    private void FixedUpdate()
    {
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

        if (invulnerableTimer <= 0 && collidingEnemies > 0)
        {
            health--;
            invulnerableTimer = invulnerableTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collidingEnemies++;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            collidingEnemies--;
        }
    }
}
