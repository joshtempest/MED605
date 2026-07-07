using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Settings")]
    public AudioMixer mainMixer;
    public string musicParam = "MusicVol";
    public string sfxParam = "SFXVol";     

    [Header("References")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio Clips")]
    public Sound[] musicSounds, sfxSounds;

    //to remember which instructions to play in the current level
    private Tuple<bool, bool> hasPlatesOrForks;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadVolume();
        //AudioManager.Instance.PlaySFX("intro");

        // AudioManager.Instance.PlaySFXWithDelay("PickUp", 5f);

        // AudioManager.Instance.PlayMusic("Music");
    }

    //  LOGIC FOR PLAYING SOUNDS

    public void PlayClip (AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayClips(AudioClip[] clips)
    {
        foreach (AudioClip c in clips)
        {
            PlayClip(c);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        musicSource.clip = s.clip;
        musicSource.loop = s.loop;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }
        sfxSource.PlayOneShot(s.clip);
    }

    public void PlaySFXWithDelay(string name, float delay)
    {
        StartCoroutine(WaitAndPlay(name, delay));
    }

    private IEnumerator WaitAndPlay(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySFX(name);
    }

    //  VOLUME AND SAVING 

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat(musicParam, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SavedMusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat(sfxParam, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SavedSFXVol", value);
    }

    private void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("SavedMusicVol", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SavedSFXVol", 0.75f);

        if (musicSlider != null) musicSlider.value = musicVol;
        if (sfxSlider != null) sfxSlider.value = sfxVol;

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void PlayVRInstructions(float delay)
    {
        PlaySFXWithDelay("v3", delay);
        PlaySFXWithDelay("v4", delay + 2f);
    }

    //instructions based on whether there are plates or forks in the scene
    //simple overload to allow for button access
    public void PlayPracticeInstructions(float delay)
    {

        if (hasPlatesOrForks.Item1 && hasPlatesOrForks.Item2)
        {
            PlaySFXWithDelay("t3", delay);
            PlaySFXWithDelay("t4", delay + 2f);

        }
        else if (hasPlatesOrForks.Item1)
        {
            PlaySFXWithDelay("t3", delay);

        }
        else
        {
            PlaySFXWithDelay("t4", delay);
        }
    }
    public void PlayPracticeInstructions(float delay, bool hasPlates, bool hasForks)
    {
        hasPlatesOrForks = new Tuple<bool, bool>(hasPlates, hasForks);

        if (hasPlates && hasForks)
        {
            PlaySFXWithDelay("t3", delay);
            PlaySFXWithDelay("t4", delay + 2f);
            
        }
        else if (hasPlates)
        {
            PlaySFXWithDelay("t3", delay);
            
        }
        else
        {
            PlaySFXWithDelay("t4", delay);
        }
    }

}
