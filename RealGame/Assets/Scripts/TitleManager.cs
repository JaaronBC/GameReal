using System.Collections;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI playText;
    public TextMeshProUGUI quitText;

    [Header("Blink")]
    public float blinkDuration = 1.5f;
    public float blinkSpeed = 0.08f;

    [Header("BGM")]
    public AudioSource bgmSource;
    public float bgmFadeDuration = 1f;

    [Header("Scene")]
    public string tutorialSceneName = "ClassRoom";

    private bool busy = false;

    public void PlayButtonPressed()
    {
        if (!busy)
            StartCoroutine(PlayRoutine());
    }

    public void QuitButtonPressed()
    {
        if (!busy)
            StartCoroutine(QuitRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        busy = true;

        yield return StartCoroutine(BlinkText(playText));
        yield return StartCoroutine(FadeOutBGM());

        SceneController sc = FindObjectOfType<SceneController>();

        if (sc != null)
        {
            sc.LoadScene(tutorialSceneName);
        }
        else
        {
            Debug.LogWarning("No SceneController found in title scene.");
        }
    }

    private IEnumerator QuitRoutine()
    {
        busy = true;

        yield return StartCoroutine(BlinkText(quitText));
        yield return StartCoroutine(FadeOutBGM());

        Debug.Log("Quit Game");
        Application.Quit();
    }

    private IEnumerator BlinkText(TextMeshProUGUI targetText)
    {
        float timer = 0f;

        while (timer < blinkDuration)
        {
            if (targetText != null)
                targetText.gameObject.SetActive(!targetText.gameObject.activeSelf);

            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed;
        }

        if (targetText != null)
            targetText.gameObject.SetActive(true);
    }

    private IEnumerator FadeOutBGM()
    {
        if (bgmSource == null)
            yield break;

        float startVolume = bgmSource.volume;
        float timer = 0f;

        while (timer < bgmFadeDuration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / bgmFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();
    }
}