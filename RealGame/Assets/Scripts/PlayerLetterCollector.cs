using UnityEngine;

public class PlayerLetterCollector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpellbookController spellbookController;
    void Start()
    {
        spellbookController = FindObjectOfType<SpellbookController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    /*
    On collision with an object, checks the tag and if 
    Letter tag, destroys component and calls the addLetter method
    in SpellbookController script which adds letter to its corosponding
    Slot
    */
        if (collision.CompareTag("Letter"))
        {
            Debug.Log("Collided with: " + collision.name);
            
            Letter letter = collision.GetComponent<Letter>();
            if(letter != null)
            {
                //add letter inventory
                bool letterAdded = spellbookController.AddLetter(collision.gameObject);
                if (letterAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
