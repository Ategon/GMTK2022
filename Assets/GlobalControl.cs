using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    [Range(0, 1)]
    public float MasterVolume;

    [Range(0, 1f)]
    public float MusicVolume;

    [Range(0, 1f)]
    public float SoundEffectVolume;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            MasterVolume = 0.5f;
            MusicVolume = 0.5f;
            SoundEffectVolume = 0.5f;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
