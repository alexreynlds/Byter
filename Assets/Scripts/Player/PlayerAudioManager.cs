using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    
    [Range(0.1f, 0.5f)]
    public float volumeChangeMultiplier = 0.2f;
    [Range(0.1f, 0.5f)]
    public float pitchChangeMultiplier = 0.2f;
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip takeDamageSound;

    [SerializeField]
    private AudioClip itemPickupSound;

    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private AudioClip laserShootSound;



    private void Start() { }

    public void TakeDamageSound()
    {
        audioSource.volume = Random.Range(1 - volumeChangeMultiplier, 1);
        audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1);
        audioSource.PlayOneShot(takeDamageSound);
    }

    public void ItemPickupSound()
    {
        audioSource.PlayOneShot(itemPickupSound);
    }

    public void DeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }

    public void PlayShootSound()
    {
        audioSource.volume = Random.Range(1 - volumeChangeMultiplier, 1);
        audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1);
        audioSource.PlayOneShot(laserShootSound);
    }
}
