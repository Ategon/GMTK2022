using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    [Range(0, 1)]
    public float MasterVolume;

    [Range(0, 1)]
    public float MusicVolume;

    [Range(0, 1)]
    public float SoundEffectVolume;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            MasterVolume = 1;
            MusicVolume = 1;
            SoundEffectVolume = 1;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
