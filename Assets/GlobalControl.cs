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
            MasterVolume = 1;
            MusicVolume = 0.8797727f;
            SoundEffectVolume = 0.8797727f;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
