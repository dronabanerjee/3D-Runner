using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * AudioManagerScript class controls audio like the sound generated on diamond collection
 */
public class AudioManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;

    public static AudioManagerScript current;

    private void Awake(){
        current = this;
    }

    /**
     * Plays the audio clip
     */
    public void PlaySound(AudioClip clip){
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

}
