using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class SpellListController : MonoBehaviour
{
    [SerializeField] private GameObject spellListPage;
    [SerializeField] private WordDatabase wordDatabase;
    [SerializeField] private GameObject shapeWordSlotPrefab;
    [SerializeField] private GameObject elementWordSlotPrefab;
    [SerializeField] private Sprite[] shapeWordSprites;
    private List<string> wordsToRemove = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkForWords();
        //print lettersGained
        /*
        for (int i = 0; i < 10; i++)
        {
            Instantiate(shapeWordSlotPrefab, spellListPage.transform.Find("ShapeScroll/Content"));
        }
        */
        foreach (GameObject slot in BattleDataHolder.ShapeSlotsFilled)
        {
            Instantiate(shapeWordSlotPrefab, spellListPage.transform.Find("ShapeScroll/Content"));
        }
    }
    public void checkForWords()
    {
        HashSet<char> allowedLetters = new HashSet<char>();
        foreach (char c in BattleDataHolder.usableLetters)
        {
            if (c != '\0')
            {
                allowedLetters.Add(char.ToLower(c));
            }
        }
        Debug.Log("Allowed Letters: " + string.Join(", ", allowedLetters));
        foreach (string word in BattleDataHolder.shapeWordsLeft)
        {
            if (allowedLetters.IsSupersetOf(BattleDataHolder.shapeWordCharacterMap[word]))
            {
                Debug.Log("Player has all letters for shape word: " + word);
                // Instantiate a shaoeword slot and set the text to the word
                GameObject slot = Instantiate(shapeWordSlotPrefab, spellListPage.transform.Find("ShapeScroll/Content"));
                //Get slot ShapeWordScript component and set the word
                ShapeWordScript shapeWordScript = slot.GetComponent<ShapeWordScript>();
                shapeWordScript.word = word;
                //Get image component
                Image image = slot.GetComponent<Image>();
                image.sprite = shapeWordSprites[BattleDataHolder.shapeSpellSpritesPointer[word]];
                BattleDataHolder.ShapeSlotsFilled.Add(slot);
                wordsToRemove.Add(word);
            }
            
        }
        foreach (string word in wordsToRemove)
        {
            BattleDataHolder.shapeWordsLeft.Remove(word);
        }
        wordsToRemove.Clear();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
