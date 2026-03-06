using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    private Interactable currentInteractable;

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
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