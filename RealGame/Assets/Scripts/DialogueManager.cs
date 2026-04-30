using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    private PlayerMovement playerMovement;

    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private string[] currentLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    // for NPC dialogue sound
    public AudioSource audioSource;
    public AudioClip dialogueSound;

    [Header("Dialogue Sound Settings")]
    public int soundInterval = 2; // play sound every X characters
    public float minPitch = 0.6f;
    public float maxPitch = 0.9f;

    public void ShowDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        dialogueBox.SetActive(true);

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.canMove = false;
        }

        currentLines = lines;
        currentLineIndex = 0;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(currentLines[currentLineIndex]));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        // NPC dialogue sound
        int charIndex = 0;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            if (audioSource != null && dialogueSound != null && letter != ' ' && soundInterval > 0 && charIndex % soundInterval == 0)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(dialogueSound);
            }

            charIndex++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void HandleInput()
    {
        if (!dialogueBox.activeSelf)
            return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            currentLineIndex++;

            if (currentLines != null && currentLineIndex < currentLines.Length)
            {
                typingCoroutine = StartCoroutine(TypeText(currentLines[currentLineIndex]));
            }
            else
            {
                HideDialogue();
            }
        }
    }

    public void HideDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isTyping = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        currentLines = null;
        currentLineIndex = 0;

        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }
    }
}