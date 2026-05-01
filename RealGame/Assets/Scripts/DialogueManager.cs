using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    private PlayerMovement playerMovement;

    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private string[] currentLines;
    public int currentLineIndex = 0;
    private bool isTyping = false;

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

        foreach (char letter in line)
        {
            dialogueText.text += letter;
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