using UnityEngine;

public class SpellbookController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    void Start()
    {
        for(int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if(i < itemPrefabs.Length)
            {
                GameObject letter = Instantiate(itemPrefabs[i], slot.transform);
                letter.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentLetter = letter;
            }
        }
    }
}
