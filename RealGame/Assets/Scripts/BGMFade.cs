using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class BGMFade : MonoBehaviour
{
    private AudioSource audioSource;
    public float normalVolume;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume; // adjust bgm in inspector (obsolete atm)
    }

    public void FadeOut(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(0f, duration));
    }

    public void FadeIn(float duration)
    {
        StopAllCoroutines();

        if (!audioSource.isPlaying)
            audioSource.Play();

        StartCoroutine(FadeTo(normalVolume, duration));
    }

    IEnumerator FadeTo(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0f)
            audioSource.Pause();
    }

    void Start()
    {
        audioSource.volume = 0f;   // start silent
        audioSource.Play();        // ensure it's playing
        FadeIn(1.0f);              // fade in over 1 second
    }
}