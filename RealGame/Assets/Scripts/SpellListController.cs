using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SpellListController : MonoBehaviour
{
    [SerializeField] private GameObject spellListPage;
    [SerializeField] private WordDatabase wordDatabase;
    [SerializeField] private GameObject shapeWordSlotPrefab;
    [SerializeField] private Sprite[] shapeWordSprites;
    //Text component
    [SerializeField] private TextMeshProUGUI textComponent;
    private List<string> wordsToRemove = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
            foreach (string word in BattleDataHolder.unlockedShapeWords)
            {
                GameObject slot = Instantiate(shapeWordSlotPrefab, spellListPage.transform.Find("ShapeScroll/Content"));

                ShapeWordScript script = slot.GetComponent<ShapeWordScript>();
                script.word = word;

                Image image = slot.GetComponent<Image>();
                image.sprite = shapeWordSprites[BattleDataHolder.shapeSpellSpritesPointer[word]];
            }
            foreach (string word in BattleDataHolder.unlockedElementWords)
            {
                GameObject newText = Instantiate(textComponent.gameObject, spellListPage.transform.Find("ElementScroll/Content"));
                TextMeshProUGUI textForElement = newText.GetComponent<TextMeshProUGUI>();
                textForElement.text = word;
                string element = word.ToLower();
                //Check all elementSets to see which element the word belongs to and color it accordingly, if it belongs to multiple element sets prioritize fire, then shock, then ice, then earth, then air, then water, then light, then dark
                Color colorToUse;
                if (wordDatabase.fireWords.Contains(word)) element = "fire";
                else if (wordDatabase.shockWords.Contains(word)) element = "shock";
                else if (wordDatabase.iceWords.Contains(word)) element = "ice";
                else if (wordDatabase.earthWords.Contains(word)) element = "earth";
                else if (wordDatabase.airWords.Contains(word)) element = "air";
                else if (wordDatabase.waterWords.Contains(word)) element = "water";
                else if (wordDatabase.lightWords.Contains(word)) element = "light";
                else if (wordDatabase.darkWords.Contains(word)) element = "dark";

                switch (element)
                {
                    case "fire":
                        colorToUse = new Color(1f, 0.5f, 0f, 1f); // bright orange
                        break;
                    case "ice":
                        colorToUse = Color.cyan;
                        break;
                    case "earth":
                        colorToUse = new Color(0.5f, 0.25f, 0f, 1f); // brown
                        break;
                    case "shock":
                        colorToUse = Color.yellow;
                        break;
                    case "air":
                        colorToUse = Color.gray;
                        break;
                    case "water":
                        colorToUse = Color.blue;
                        break;
                    case "light":
                        colorToUse = new Color(1f, 1f, 0.5f, 1f); // light yellow
                        break;
                    case "dark":
                        colorToUse = new Color(0.5f, 0f, 0.5f, 1f); // dark purple
                        break;
                    default:
                        colorToUse = Color.white;
                        break;  
                }
                textForElement.color = colorToUse;
                textForElement.fontMaterial = new Material(textForElement.fontMaterial);
                textForElement.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
                textForElement.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            }
            foreach (string word in BattleDataHolder.unlockedPowerWords)
            {
                GameObject newText = Instantiate(textComponent.gameObject, spellListPage.transform.Find("PowerScroll/Content"));
                TextMeshProUGUI textForPower = newText.GetComponent<TextMeshProUGUI>();
                textForPower.text = word;
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
                BattleDataHolder.unlockedShapeWords.Add(word);
                wordsToRemove.Add(word);
            }
            
        }
        //Remove words that have been unlocked from shapeWordsLeft
        foreach (string word in wordsToRemove)
        {
            BattleDataHolder.shapeWordsLeft.Remove(word);
        }
        wordsToRemove.Clear();
        //Check element words and if the player has letters for them put a text element in content of the word and color it based on the element
        foreach (string word in BattleDataHolder.elementWordsLeft)
        {
            if (allowedLetters.IsSupersetOf(BattleDataHolder.elementWordCharacterMap[word]))
            {
                //Instantiate a text element in the ElementScroll/Content and set the text to the word and color it based on the element
                Debug.Log("Player has all letters for element word: " + word);
                GameObject newText = Instantiate(textComponent.gameObject, spellListPage.transform.Find("ElementScroll/Content"));
                TextMeshProUGUI textForElement = newText.GetComponent<TextMeshProUGUI>();
                textForElement.text = word;
                string element = word.ToLower();
                //Check all elementSets to see which element the word belongs to and color it accordingly, if it belongs to multiple element sets prioritize fire, then shock, then ice, then earth, then air, then water, then light, then dark
                Color colorToUse;
                if (wordDatabase.fireWords.Contains(word)) element = "fire";
                else if (wordDatabase.shockWords.Contains(word)) element = "shock";
                else if (wordDatabase.iceWords.Contains(word)) element = "ice";
                else if (wordDatabase.earthWords.Contains(word)) element = "earth";
                else if (wordDatabase.airWords.Contains(word)) element = "air";
                else if (wordDatabase.waterWords.Contains(word)) element = "water";
                else if (wordDatabase.lightWords.Contains(word)) element = "light";
                else if (wordDatabase.darkWords.Contains(word)) element = "dark";

                switch (element)
                {
                case "fire":
                    colorToUse = new Color(1f, 0.5f, 0f, 1f); // bright orange
                    break;
                case "ice":
                    colorToUse = Color.cyan;
                    break;
                case "earth":
                    colorToUse = new Color(0.5f, 0.25f, 0f, 1f); // brown
                    break;
                case "shock":
                    colorToUse = Color.yellow;
                    break;
                case "air":
                    colorToUse = Color.gray;
                    break;
                case "water":
                    colorToUse = Color.blue;
                    break;
                case "light":
                    colorToUse = new Color(1f, 1f, 0.5f, 1f); // light yellow
                    break;
                case "dark":
                    colorToUse = new Color(0.5f, 0f, 0.5f, 1f); // dark purple
                    break;
                default:
                    colorToUse = Color.white;
                    break;  

                }
                textForElement.color = colorToUse; 

                textForElement.fontMaterial = new Material(textForElement.fontMaterial);

                textForElement.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
                textForElement.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);    
                BattleDataHolder.unlockedElementWords.Add(word);    
                wordsToRemove.Add(word);
            }
        }
        //Remove words that have been unlocked from elementWordsLeft
        foreach (string word in wordsToRemove)
        {
            BattleDataHolder.elementWordsLeft.Remove(word);
        }
        wordsToRemove.Clear();
        foreach (string word in BattleDataHolder.powerWordsLeft)
        {
            if (allowedLetters.IsSupersetOf(BattleDataHolder.powerWordCharacterMap[word]))
            {
                Debug.Log("Player has all letters for power word: " + word);
                //Instantiate a text element in the PowerScroll/Content and set the text to the word
                GameObject newText = Instantiate(textComponent.gameObject, spellListPage.transform.Find("PowerScroll/Content"));
                TextMeshProUGUI textForPower = newText.GetComponent<TextMeshProUGUI>();
                textForPower.text = word;
                BattleDataHolder.unlockedPowerWords.Add(word);
                wordsToRemove.Add(word);
            }
        }
        //Remove words that have been unlocked from powerWordsLeft
        foreach (string word in wordsToRemove)
        {
            BattleDataHolder.powerWordsLeft.Remove(word);
        }
        wordsToRemove.Clear();

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
