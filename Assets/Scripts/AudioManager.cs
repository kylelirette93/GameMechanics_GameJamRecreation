using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource effectSource;
    public AudioClip gameplayMusic;
    public AudioClip impact;
    public AudioClip bossMusic;
    public AudioClip dashSound;

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();      
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip == dashSound)
        {
            // Do a random pitch only for dash.
            effectSource.pitch = Random.Range(0.8f, 1.2f);          
        }
        else
        {
            effectSource.pitch = 1;
        }
        effectSource.PlayOneShot(clip);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
