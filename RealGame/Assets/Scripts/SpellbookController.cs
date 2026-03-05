using UnityEngine;

public class SpellbookController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] letterPrefabs;
    void Start()
    {
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
    public bool AddLetter(GameObject itemPrefab)
    {
        //look for empty slot
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentLetter ==null)
            {
                GameObject newLetter =Instantiate(itemPrefab, slotTransform);
                newLetter.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentLetter = newLetter;
                return true;
            }
        }
        Debug.Log("Inventory is Full");
        return false;
    }
}
