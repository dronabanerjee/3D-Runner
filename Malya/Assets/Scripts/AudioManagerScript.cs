using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioManagerScript current;

    private void Awake()
    {
        current = this;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
