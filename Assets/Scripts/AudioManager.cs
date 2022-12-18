using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    public static AudioManager instance {get; private set;}

    public AudioClip musicClip;

    private void Awake()
    {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this){
            Destroy(gameObject);
        }
    }

    // void Start(){
    //     AudioManager.instance.PlayMusic(musicClip);
    // }

    public void Play(AudioClip sound)
    {
        EffectsSource.clip = sound;
		EffectsSource.Play();
    }

    public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
	}
}
