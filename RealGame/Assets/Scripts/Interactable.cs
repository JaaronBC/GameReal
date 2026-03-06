using UnityEngine;

// handles showing the E prompt and interaction behavior
// script is attached to any object the player can interact with
// also handles showing the E prompt
public class Interactable : MonoBehaviour
{
    public GameObject prompt;

    private void Start()
    {
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
    }
    // runs when the player presses E
    // note: player interaction script calls this function
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
    // runs automatically when player enters trigger area
    // checks if the object entering is the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
            }

            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.SetCurrentInteractable(this);
            }
        }
    }
    // runs when something leaves the trigger area
    // when player walks away > remove prompt and clear interaction
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (prompt != null)
            {
                prompt.SetActive(false);
            }

            PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.ClearCurrentInteractable(this);
            }
        }
    }
}