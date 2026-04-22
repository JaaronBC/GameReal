using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class SpellbookController : MonoBehaviour
{
    public PlayerState playerState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    Dictionary<char, GameObject> letterPrefabs = new Dictionary<char, GameObject>();
    Dictionary<char, Sprite> letterSprites = new Dictionary<char, Sprite>();
    void Awake()
    {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            GameObject prefab = Resources.Load<GameObject>("Letters/Letter" + c);
            if (prefab != null)
            {
                letterPrefabs[c] = prefab;
                letterSprites[c] = prefab.GetComponent<Image>().sprite;
            }
            else
            {
                Debug.LogError("Missing prefab for letter: " + c);
            }
        }
    }
    void Start()
    {
        //Creates letter slots on game start equal to the slot count
        for(int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (BattleDataHolder.usableLetters[i] != '\0') 
            {
                char letter = BattleDataHolder.usableLetters[i];
                GameObject letterPrefab = letterPrefabs[letter];
                if (letterPrefab != null)
                {
                    AddLetter(letter);
                }
                else
                {
                    Debug.LogError("Letter prefab not found for letter: " + letter);
                }
            }
        }
    }
    
    public bool AddLetter(char letter)
    {
        int slotIndex = letter - 'A';

        if (!letterSprites.ContainsKey(letter)) return false;

        Transform slotTransform = inventoryPanel.transform.GetChild(slotIndex);
        Slot slot = slotTransform.GetComponent<Slot>();

        if (slot.currentLetter == '\0')
        {
            slot.currentLetter = letter;

            Image slotImage = slotTransform.GetComponent<Image>();
            slotImage.sprite = letterSprites[letter];
            slotImage.color = Color.white;

            BattleDataHolder.usableLetters[slotIndex] = letter;
            return true;
        }

        return false;
    }
}