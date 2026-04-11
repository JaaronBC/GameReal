using UnityEngine;

public class NPCDialogue : Interactable
{
    [TextArea(3, 6)]
    public string[] dialogueLines;

    public override void Interact()
    {
        PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();

        if (player != null && player.dialogueManager != null)
        {
            player.dialogueManager.ShowDialogue(dialogueLines);
        }
    }
}