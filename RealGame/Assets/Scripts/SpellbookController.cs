using UnityEngine;
using UnityEngine.UI;

public class SpellbookController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] letterPrefabs;
    void Start()
    {
        //Creates letter slots on game start equal to the slot count
        for(int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if(i < letterPrefabs.Length)
            {
                GameObject letter = Instantiate(letterPrefabs[i], slot.transform);
                letter.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentLetter = letter;
            }
        }
    }
    
public bool AddLetter(GameObject letterPrefab)
{
    Letter letterScript = letterPrefab.GetComponent<Letter>();

    if (letterScript == null)
    {
        Debug.LogError("Letter prefab is missing Letter script.");
        return false;
    }

    char letter = letterScript.letterValue;

    // Convert Alphabet A-Z a numerical value of 0-25
    int slotIndex = letter - 'A';

    if (slotIndex < 0 || slotIndex >= inventoryPanel.transform.childCount)
    {
        Debug.LogError("Invalid slot index for letter: " + letter);
        return false;
    }
    
    Transform slotTransform = inventoryPanel.transform.GetChild(slotIndex);
    //Gets the script component Slot from slotTransform prefab and sets it to the "slot" variable
    Slot slot = slotTransform.GetComponent<Slot>();

    if (slot != null && slot.currentLetter == null)
    {
        //Creates copy of letter and sets it to newLetter
        GameObject newLetter = Instantiate(letterPrefab, slotTransform);
        newLetter.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //Retrieves image components from slot and and letter prefabs and sets them to variables
        Image slotImage = slotTransform.GetComponent<Image>();
        Image letterImage = newLetter.GetComponent<Image>();

        if (slotImage != null && letterImage != null)
        {
            //Makes slot image component sprite the slot image component of letter image component
            //Sets color of the slots image component to white
            slotImage.sprite = letterImage.sprite;
            slotImage.color = Color.white;
        }
        //Sets the currentLetter of the slot to newLetter prefab
        slot.currentLetter = newLetter;

        return true;
    }

    Debug.Log("Slot already occupied or invalid.");
    return false;
    }
}