using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public AudioClip music;
    public AudioClip snd_bat;
    public AudioClip snd_dash;
    public AudioClip snd_eating;
    public AudioClip[] snd_wisp;

    // Ensures a singleton
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            instance = this;
        //If instance already exists:
        else if (instance != this)
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Start the music
        instance.PlayMusic(GetComponent<AudioSource>());
    }

    // Public Functions
    #region
    // Music
    public void PlayMusic(AudioSource bgSource)
    {
        bgSource.clip = music;
        bgSource.Play();
    }

    // Sound Effects
    public void PlayBat(AudioSource sfxSource)
    {
        sfxSource.clip = snd_bat;
        sfxSource.Play();
    }

    public void PlayDash(AudioSource sfxSource)
    {
        sfxSource.clip = snd_dash;
        sfxSource.Play();
    }

    public void PlayEating(AudioSource sfxSource)
    {
        sfxSource.clip = snd_eating;
        if(!sfxSource.isPlaying) sfxSource.Play();
    }

    public void PlayWisp(AudioSource sfxSource)
    {
        //Randomly select sound
        int i = Random.Range(0, snd_wisp.Length - 1);

        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        sfxSource.clip = snd_wisp[i];

        //Play the clip.
        sfxSource.Play();
    }
    #endregion
}
