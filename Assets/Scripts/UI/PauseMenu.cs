using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject powerUpChoiceMenu;
    [SerializeField]
    private GameObject defeatMenu;

    public void Pause()
    {
        UnityEngine.Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Defeat()
    {
        UnityEngine.Cursor.visible = true;
        defeatMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        UnityEngine.Cursor.visible = false;
        pauseMenu.SetActive(false);
        powerUpChoiceMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
