using System.Collections;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI playText;
    public TextMeshProUGUI quitText;
    public TextMeshProUGUI creditsText;

    [Header("Blink")]
    public float blinkDuration = 1.5f;
    public float blinkSpeed = 0.08f;

    [Header("BGM")]
    public AudioSource bgmSource;
    public float bgmFadeDuration = 1f;

    [Header("Scene")]
    public string tutorialSceneName = "ClassRoom";

    [Header("Credits")]
    public GameObject creditsBox;
    public TextMeshProUGUI creditsBodyText;

    [TextArea(5, 15)]
    public string creditsMessage =
        "SPELL CHECK\n\n" +
        "Created by Goose Gang\n\n" +
        "Programming:\n" +
        "Name Here\n\n" +
        "Art:\n" +
        "Name Here\n\n" +
        "Music and Sound:\n" +
        "Name Here\n\n" +
        "Thank you for playing!";

    public float creditsTypingSpeed = 0.015f;

    private bool busy = false;
    private bool creditsOpen = false;
    private bool creditsTyping = false;
    private Coroutine creditsCoroutine;

    void Start()
    {
        if (creditsBox != null)
        {
            creditsBox.SetActive(false);
        }

        if (creditsBodyText != null)
        {
            creditsBodyText.text = "";
        }
    }

    void Update()
    {
        if (!creditsOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCredits();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (creditsTyping)
            {
                SkipCreditsTyping();
            }
            else
            {
                CloseCredits();
            }
        }
    }

    public void PlayButtonPressed()
    {
        if (!busy && !creditsOpen)
            StartCoroutine(PlayRoutine());
    }

    public void QuitButtonPressed()
    {
        if (!busy && !creditsOpen)
            StartCoroutine(QuitRoutine());
    }

    public void CreditsButtonPressed()
    {
        if (!busy && !creditsOpen)
            StartCoroutine(CreditsRoutine());
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

    private IEnumerator CreditsRoutine()
    {
        busy = true;

        yield return StartCoroutine(BlinkText(creditsText));

        OpenCredits();

        busy = false;
    }

    public void OpenCredits()
    {
        if (creditsBox == null || creditsBodyText == null)
        {
            Debug.LogWarning("Credits UI is not assigned.");
            return;
        }

        creditsOpen = true;
        creditsBox.SetActive(true);

        if (creditsCoroutine != null)
        {
            StopCoroutine(creditsCoroutine);
        }

        creditsCoroutine = StartCoroutine(TypeCredits());
    }

    public void CloseCredits()
    {
        creditsOpen = false;
        creditsTyping = false;

        if (creditsCoroutine != null)
        {
            StopCoroutine(creditsCoroutine);
            creditsCoroutine = null;
        }

        if (creditsBodyText != null)
        {
            creditsBodyText.text = "";
        }

        if (creditsBox != null)
        {
            creditsBox.SetActive(false);
        }
    }

    public void SkipCreditsTyping()
    {
        if (creditsCoroutine != null)
        {
            StopCoroutine(creditsCoroutine);
            creditsCoroutine = null;
        }

        if (creditsBodyText != null)
        {
            creditsBodyText.text = creditsMessage;
        }

        creditsTyping = false;
    }

    private IEnumerator TypeCredits()
    {
        creditsTyping = true;
        creditsBodyText.text = "";

        foreach (char letter in creditsMessage)
        {
            creditsBodyText.text += letter;
            yield return new WaitForSeconds(creditsTypingSpeed);
        }

        creditsTyping = false;
        creditsCoroutine = null;
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