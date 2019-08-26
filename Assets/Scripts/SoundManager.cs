using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource soundFxManager, soundFxManager2;
    public AudioSource themeSoundManager;

    public AudioClip[] themeSongs;

    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!themeSoundManager.isPlaying)
        {
            themeSoundManager.clip = themeSongs[Random.Range(0, themeSongs.Length)];
            themeSoundManager.volume = Random.Range(0.3f, 0.4f);
            themeSoundManager.Play();
        }
    }

    public void PlaySoundFx(AudioClip audioclip, float volume)
    {
        if (!soundFxManager.isPlaying)
        {
            soundFxManager.clip = audioclip;
            soundFxManager.volume = volume;
            soundFxManager.Play();
        }
        else
        {
            soundFxManager2.clip = audioclip;
            soundFxManager2.volume = volume;
            soundFxManager2.Play();
        }
    }

    public void PlayRandomSoundFX(AudioClip[] audioclips)
    {
        if (!soundFxManager.isPlaying)
        {
            soundFxManager.clip = audioclips[Random.Range(0, audioclips.Length)];
            soundFxManager.volume = Random.Range(0.3f, 0.6f);
            soundFxManager.Play();
        }
    }
}
