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
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;

    [SerializeField]
    private Canvas canvas;

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
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
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
