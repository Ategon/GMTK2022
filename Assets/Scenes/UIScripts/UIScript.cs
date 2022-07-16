using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour
{
    private EventSystem es;

    void Start()
    {
        es = EventSystem.current;
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SwitchSelectedButton(GameObject gameObject)
    {
        es.SetSelectedGameObject(gameObject);
    }

    public void Quit()
    {
        if (Screen.fullScreenMode != FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Fullscreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
