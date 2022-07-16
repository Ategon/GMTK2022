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
}
