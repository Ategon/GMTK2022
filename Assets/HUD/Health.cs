using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
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

    private void Update()
    {
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
}
