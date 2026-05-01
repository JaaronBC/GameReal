using System.Collections;
using UnityEngine;

public class EndSceneScript : MonoBehaviour
{
    public GameObject dialouge;
    public Animator animator;

    [Header("Ending Transition")]
    public float titleScreenDelay = 5.0f;
    public string titleSceneName = "TitleScreen";

    int textIndex = 0;
    bool endingStarted = false;
    bool dialogueWasOpen = false;

    public BoxCollider2D collider;
    private DialogueManager dialougeScript;

    void Start()
    {
        dialougeScript = dialouge.GetComponent<DialogueManager>();
    }

    void Update()
    {
        if (dialouge != null && dialougeScript != null)
        {
            textIndex = dialougeScript.currentLineIndex;

            if (dialougeScript.dialogueBox.activeSelf)
            {
                dialogueWasOpen = true;
            }

            if (!endingStarted && dialogueWasOpen && !dialougeScript.dialogueBox.activeSelf)
            {
                collider.enabled = false;
                endingStarted = true;
                Debug.Log("Ending started. Fading BGM and going to title.");
                StartCoroutine(EndToTitleCoroutine());
            }
        }

        animator.SetInteger("TextIndex", textIndex);
    }

    IEnumerator EndToTitleCoroutine()
    {
        BGMFade bgmFade = FindObjectOfType<BGMFade>();

        if (bgmFade != null)
        {
            bgmFade.FadeOut(5.0f);
        }
        else
        {
            Debug.LogWarning("No BGMFade found in scene. Loading title anyway.");
        }

        yield return new WaitForSeconds(titleScreenDelay);

        SceneController sc = FindObjectOfType<SceneController>();

        if (sc != null)
        {
            sc.LoadScene(titleSceneName);
        }
        else
        {
            Debug.LogWarning("No SceneController found. Could not load title scene.");
        }
    }
}