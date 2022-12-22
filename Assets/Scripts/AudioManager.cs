using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource EffectsSource;

    public AudioSource MusicSource;

    public static AudioManager instance { get; private set; }

    public AudioClip musicClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else if (instance != this)
        {
            Destroy (gameObject);
        }
    }

    public void Play(AudioClip sound, float vol = 1.0f)
    {
        EffectsSource.clip = sound;
        EffectsSource.volume = vol;
        EffectsSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
