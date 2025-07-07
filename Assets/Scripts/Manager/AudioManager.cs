using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] backsounds, sfxSounds;
    public AudioSource backsoundSource, sfxSource;
    private float backsoundFadeDuration = 1.0f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMainThemeBacksound();
    }

    public void PlayBacksound(string name)
    {
        Sound sound = Array.Find(backsounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("BackSound not found");
        }
        else
        {
            backsoundSource.clip = sound.clip;
            backsoundSource.Play();
        }
    }


    public void FadeOutBacksound(Action onFadeComplete = null)
    {
        StartCoroutine(FadeOutBacksoundCoroutine(onFadeComplete));
    }

    private IEnumerator FadeOutBacksoundCoroutine(Action onFadeComplete)
    {
        float startVolume = backsoundSource.volume;
        float elapsed = 0f;

        while (elapsed < backsoundFadeDuration)
        {
            elapsed += Time.deltaTime;
            backsoundSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / backsoundFadeDuration);
            yield return null;
        }

        backsoundSource.Stop();
        backsoundSource.volume = startVolume;

        onFadeComplete?.Invoke();
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("SFX not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void StopBacksound()
    {
        if (backsoundSource.isPlaying)
        {
            backsoundSource.Stop();
        }
    }


    // Play BackSound
    public void PlayExplorationBacksound() => PlayBacksound("Exploration");
    public void PlayMainThemeBacksound() => PlayBacksound("Main Theme");
    public void PlayOfficeBacksound() => PlayBacksound("Office");
    // public void PlayExplorationBacksound()
    // {
    //     Debug.Log("Playing exploration backsound");
    //     PlayBacksound("Exploration");
    // }

    // Play SFX
    // public void PlayClikUI() => PlayBacksound("Click UI");
    public void PlayWalk() => PlaySFX("Walk");
    public void PlayDiving() => PlaySFX("Diving");
    public void PlayRun() => PlaySFX("Run");
    public void SFXCollectItem() => PlaySFX("Collect Item");
    public void SFXMissionCompleted() => PlaySFX("Mission Completed");
    public void SFXChapterCompleted() => PlaySFX("Chapter Completed");
    public void SFXCollectGarbage() => PlaySFX("Collect Garbage");
    public void SFXCameraShutter() => PlaySFX("Camera Shutter");

    public void PlayClikUI()
    {
        PlaySFX("Click UI");
    }
}