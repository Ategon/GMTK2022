using UnityEngine;

public class Volume : MonoBehaviour
{
    public enum type
    {
        Music,
        SoundEffect
    }

    private GlobalControl globalControl;

    public type soundType;

    [Range(0, 1)]
    public float volume;

    private AudioSource[] audios;

    public bool editAudioVolume;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponents<AudioSource>();
        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].volume = volume;
        }

        GameObject globalGameController = GameObject.Find("GlobalMusicSFXController");

        if (globalGameController != null)
        {
            globalControl = globalGameController.GetComponent<GlobalControl>();
            if (soundType == type.Music)
            {
                volume = globalControl.MusicVolume;
            }
            if (soundType == type.SoundEffect)
            {
                volume = globalControl.SoundEffectVolume;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (globalControl != null)
        {
            if (soundType == type.Music)
            {
                volume = globalControl.MusicVolume;
            }
            if (soundType == type.SoundEffect)
            {
                volume = globalControl.SoundEffectVolume;
            }
        }

        if (editAudioVolume)
        {
            for (int i = 0; i < audios.Length; i++)
            {
                audios[i].volume = volume;

            }
        }
    }
}
