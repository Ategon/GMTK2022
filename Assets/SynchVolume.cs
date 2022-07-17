using UnityEngine;
using UnityEngine.UI;

public class SynchVolume : MonoBehaviour
{
    private Slider master;
    private Slider music;
    private Slider sfx;
    private GlobalControl globalControl;

    // Start is called before the first frame update
    private void Start()
    {
        globalControl = GameObject.Find("GlobalMusicSFXController").GetComponent<GlobalControl>();
        master = GameObject.Find("MasterSlider").GetComponent<Slider>();
        music = GameObject.Find("MusicSlider").GetComponent<Slider>();
        sfx = GameObject.Find("SFXSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        globalControl.MasterVolume = master.value;
        globalControl.MusicVolume = music.value;
        globalControl.SoundEffectVolume = sfx.value;
    }

}
