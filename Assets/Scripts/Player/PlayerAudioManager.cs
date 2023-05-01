using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
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
        audioSource.PlayOneShot(laserShootSound);
    }
}
