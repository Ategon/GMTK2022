using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAudio : MonoBehaviour
{

    class AudioTrack
    {
        public AudioTrackType type;
        public AudioSource source;

        public AudioTrack(AudioTrackType type, AudioSource source)
        {
            this.type = type;
            this.source = source;
        }
    }

    [SerializeField] private AudioClipSection[] audioClips;

    [System.Serializable]
    class AudioClipSection
    {
        public string name;
        public AudioClip[] clips;
    }

    List<AudioTrack> audioTracks = new List<AudioTrack>();

    private void Awake()
    {
        foreach (AudioTrackType i in Enum.GetValues(typeof(AudioTrackType)))
        {
            GameObject newSource = new GameObject();
            AudioSource source = (AudioSource) newSource.AddComponent(typeof(AudioSource));
            newSource.transform.SetParent(transform);
            newSource.name = Enum.GetName(typeof(AudioTrackType), i);

            Volume volume = (Volume)newSource.AddComponent(typeof(Volume));
            volume.volume = 0.5f;
            volume.editAudioVolume = true;

            if (newSource.name == "Soundtrack")
            {
                source.loop = true;
                volume.soundType = Volume.type.Music;
            }
            else
            {
                volume.soundType = Volume.type.SoundEffect;
            }

            

            AudioTrack track = new AudioTrack(i, source);
            audioTracks.Add(track);
        }
    }

    private void Start()
    {
        PlaySound("Level Theme", AudioTrackType.Soundtrack);
    }

    public void PlaySound(string sound, AudioTrackType trackType)
    {
        AudioTrack track = audioTracks.Find(e => e.type == trackType);

        AudioClipSection clipSection = Array.Find(audioClips, e => e.name == sound);

        track.source.clip = clipSection.clips[UnityEngine.Random.Range(0, clipSection.clips.Length)];
        track.source.Play();
    }

    public void StopSound(AudioTrackType trackType)
    {
        AudioTrack track = audioTracks.Find(e => e.type == trackType);

        track.source.Stop();
    }
}

public enum AudioTrackType
{
    PlayerDice,
    EnemyDie,
    Footsteps,
    Soundtrack
}