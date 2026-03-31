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
    private string currentLine;
    private bool isTyping = false;

    public void ShowDialogue(string text)
    {
        dialogueBox.SetActive(true);

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.canMove = false;
        }

        currentLine = text;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in currentLine)
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
            // finish instantly
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentLine;
            isTyping = false;
        }
        else
        {
            // close dialogue
            HideDialogue();
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

        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }
    }
}