using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public DialogueManager dialogueManager;
    private Interactable currentInteractable;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("E pressed");

            if (dialogueManager != null && dialogueManager.dialogueBox.activeSelf)
            {
                dialogueManager.HandleInput();
                return;
            }

            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    public void ClearCurrentInteractable(Interactable interactable)
    {
        if (currentInteractable == interactable)
        {
            currentInteractable = null;
        }
    }
}