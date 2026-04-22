using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerCasting : MonoBehaviour
{
    public bool allowAllLetters = false; // Set to true to allow all letters regardless of BattleDataHolder settings
    [SerializeField] BattleScript battleScript;
    [SerializeField] private TextMeshProUGUI spellBuildText;
    Dictionary<string, System.Action<float, string>> shapeActions;
    //Targeting variables
    int currentTargetIndex = 0;
    GameObject currentTarget;

    public bool isActive = false;

    int debugCounter = 0;
    //Spell crafting variables
    public List<string> spellBuilder = new List<string>();
    public string spellWord = "";
    //Visuals for spell crafting
    [SerializeField] GameObject letterPrefab;
    [SerializeField] GameObject[] letterPrefabs;
    Dictionary<char, GameObject> letterMap;
    [SerializeField] Transform currentWordContainer;
    [SerializeField] Transform pastWordsContainer;
    public List<GameObject> currentLetters = new List<GameObject>();
    public float letterBuildX, letterBuildY;
    public List<GameObject> wordToColor = new List<GameObject>();
    public bool elementalNotFound = true;

    //Word Database 
    [SerializeField] WordDatabase wordDatabase;

    public int spellsCast = 0;
    public float backspaceCounter = 0;
    public List<char> allowedLetters;
    //Prefabs for shape words
    [SerializeField] GameObject boltPrefab;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject beamPrefab;
    [SerializeField] GameObject slashPrefab;
    [SerializeField] GameObject spearPrefab;
    [SerializeField] GameObject drillPrefab;
    public void BeginTurn()
    {
        isActive = true;
        if (battleScript.activeEnemies.Count > 0)
        {
            currentTarget = battleScript.activeEnemies[currentTargetIndex];
            HighlightTarget(currentTarget);
        }
    }
    public void CycleTarget()
    {
        //debugCounter++;
        //Debug.Log("debug: cycle target called " + debugCounter);
        var enemies = battleScript.activeEnemies;

        if (enemies.Count == 0) return;

        currentTargetIndex = (currentTargetIndex + 1) % enemies.Count;

        currentTarget = enemies[currentTargetIndex];
        HighlightTarget(currentTarget);
    }

    void HighlightTarget(GameObject target)
    {
        var renderer = target.GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            // Reset all enemies to white
            foreach (var enemy in battleScript.activeEnemies)
            {
                Transform targetUI = enemy.transform.Find("EnemyHealthBar/Target");
                if (targetUI != null)                
                 {
                     Image img = targetUI.GetComponent<Image>();
                     if (img != null)                     {
                         img.color = Color.white;    
                     }
                 }
            }
        }
        Transform selectedTargetUI = target.transform.Find("EnemyHealthBar/Target");
        if (selectedTargetUI != null)
        {
            Image img = selectedTargetUI.GetComponent<Image>();
            if (img != null)
            {
                img.color = Color.red;
            }
        }
    }

    void SpawnProjectile(GameObject prefab, GameObject target, string element, float damage, string shape, bool piercing = false)
    {
        GameObject proj = Instantiate(prefab, new Vector3(4.5f, 3f, 0f), Quaternion.identity);

        SpriteRenderer projRenderer = proj.GetComponentInChildren<SpriteRenderer>();
        if (projRenderer != null)
        {
            Color colorToUse = Color.white; // default

            switch (element?.ToLower()) 
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

        projRenderer.color = colorToUse;
        projRenderer.sortingOrder = 10; 
    }

    // Assign target for projectile movement
    ProjectileScript projectile = proj.GetComponent<ProjectileScript>();
    if (projectile != null && target != null)
    {
        if (element == null) element = "none"; // Handle case where no element is assigned

        //Data passed to projectile for damage calculation and hit effects
        projectile.target = target.transform;
        projectile.targetObject = target;
        projectile.damage = damage;
        projectile.shape = shape;
        projectile.element = element;
        projectile.piercing = piercing;
    }
}

    void CraftSpell()
    {
        Debug.Log("Crafting spell with components: " + string.Join(", ", spellBuilder));
        if (spellBuilder.Count == 0) return;
        HashSet<string> usedMetaWords = new HashSet<string>();
        float baseMultiplier = 1;
        float metaMultiplier = 1;
        float baseDamage = 10;
        string shape = null;
        string element = null;
        foreach (string word in spellBuilder)
        {
            if (wordDatabase.shapeWords.Contains(word))
            {
                shape = word;
            }
            else if (wordDatabase.elementWords.Contains(word))
            {
                if (element == null)
                {
                    element = word;
                }
            }
            else if (wordDatabase.metaWords.Contains(word))
            {
                if (!usedMetaWords.Contains(word))
                {
                    metaMultiplier += 0.5f; // Each meta word increases multiplier by 50%
                    usedMetaWords.Add(word);
                }
            }
        }

        //check all elemental hashmaps for word and if found, make it into element type 
        if (wordDatabase.fireWords.Contains(element))
        {
            element = "fire";
        }
        else if (wordDatabase.waterWords.Contains(element))
        {
            element = "water";
        }
        else if (wordDatabase.earthWords.Contains(element))
        {
            element = "earth";
        }
        else if (wordDatabase.airWords.Contains(element))
        {
            element = "air";
        }
        else if (wordDatabase.shockWords.Contains(element))
        {
            element = "shock";
        }
        else if (wordDatabase.iceWords.Contains(element))
        {
            element = "ice";
        }
        else if (wordDatabase.lightWords.Contains(element))
        {
            element = "light";
        }
        else if (wordDatabase.darkWords.Contains(element))
        {
            element = "dark";
        } 
        else
        {
            element = null; // If element word isn't recognized, treat as no element
            baseMultiplier -= 0.2f; // Unrecognized element reduces base damage by 20%
        }

        baseMultiplier -= 0.2f * spellsCast; // Each spell cast reduces base multiplier by 10%
        baseMultiplier -= 0.1f * backspaceCounter; // Each backspace reduces base multiplier by 10%
        if (baseMultiplier < 0.1f) baseMultiplier = 0.1f; // Minimum damage multiplier of 10%
        float damage = baseDamage * baseMultiplier * metaMultiplier;
        //Switch case for shape to determine attack type
       
        if (shape != null && shapeActions.ContainsKey(shape))
        {
            shapeActions[shape].Invoke(damage, element);
            spellsCast++;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<char> lettersTest = new List<char>();
        foreach (char c in BattleDataHolder.usableLetters)
        {
            if (c != '\0')
            {
                lettersTest.Add(char.ToLower(c));
                allowedLetters.Add(char.ToLower(c));
            }
        }


        Debug.Log("Usable Letters: " + string.Join(", ", lettersTest));
        //Initialize shape action dictionary with corresponding methods for each shape
        shapeActions = new Dictionary<string, System.Action<float, string>>()
        {
            { "bolt", CastBolt },
            { "ball", CastBall },
            { "missile", CastMissile },
            { "beam", CastBeam },
            {"laser", CastBeam},
            {"ray", CastBeam},
            {"slash", CastSlash},
            {"spear", CastSpear},
            {"lance", CastSpear},
            {"javelin", CastSpear},
            {"drill", CastDrill}
        };
        //Initialize letter prefab mapping for spell crafting visuals
        letterMap = new Dictionary<char, GameObject>();
        for (int i = 0; i < 26; i++)
        {
            char c = (char)('a' + i);
            letterMap[c] = letterPrefabs[i];
        }
        //Dbug log validWord hashset
        Debug.Log("Valid Words: " + string.Join(", ", wordDatabase.validWords));
    }

    // Update is called once per frame
    void Update()
    {
        spellBuildText.text = "Current Word:" + spellWord;
        if (!isActive) return;
        //Cycle through targets using left shift key
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CycleTarget();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!string.IsNullOrEmpty(spellWord))
            {
                // Remove last character from string
                backspaceCounter += 1;
                Debug.Log("Backspace pressed " + backspaceCounter + " times");
                spellWord = spellWord.Substring(0, spellWord.Length - 1);
                // Remove last letter object
                if (currentLetters.Count > 0)
                {
                    GameObject lastLetter = currentLetters[currentLetters.Count - 1];
                    currentLetters.RemoveAt(currentLetters.Count - 1);
                    wordToColor.RemoveAt(wordToColor.Count - 1);
                    Destroy(lastLetter);
                    UpdateLetterPositions();
                }
            }
        }

        if (Input.anyKeyDown)
        {   
            foreach (char character in Input.inputString)
            {
                //Allow for uppercase letters by converting to lowercase
                char c = char.ToLower(character);
                if (char.IsLetter(c))
                {   
                    if (allowAllLetters) {
                        spellWord += c;
                        SpawnLetter(c);
                        continue;
                    }
                    if (!allowedLetters.Contains(char.ToLower(c))) 
                    {
                        Debug.Log("Letter '" + c + "' is invalid;"); // Ignore letters that are not in the allowed list
                    } 
                    else 
                    {
                    spellWord += c;
                    SpawnLetter(c);
                    //Debug.Log("Current Spell Word: " + spellWord);
                    }
                }
                else if (c == ' ')
                {
                    if (!string.IsNullOrEmpty(spellWord))
                    {
                        ColorWord();
                        spellBuilder.Add(spellWord);
                        if (wordDatabase.shapeWords.Contains(spellWord))
                        {
                            CraftSpell();
                            spellBuilder.Clear();
                            currentLetters.ForEach(letter => Destroy(letter));
                            currentLetters.Clear();
                            wordToColor.Clear();
                            backspaceCounter = 0;
                            elementalNotFound = true;
                        }
                        Debug.Log("Spell Added: " + spellWord);
                        spellWord = "";
                    }
                }
            }
        }
    }
    void SpawnLetter(char c)
    {
        c = char.ToLower(c);

        if (!letterMap.ContainsKey(c)) return;

        GameObject letterObj = Instantiate(letterMap[c], currentWordContainer);

        currentLetters.Add(letterObj);
        wordToColor.Add(letterObj);

        UpdateLetterPositions();
    }
    void UpdateLetterPositions()
    {
        float spacing = 1f;
        float totalWidth = (currentLetters.Count - 1) * spacing;

        for (int i = 0; i < currentLetters.Count; i++)
        {
            float x = i * spacing - totalWidth / 2f;
            currentLetters[i].transform.localPosition = new Vector3(letterBuildX+x, letterBuildY, 0);
        }
    }
    void ColorWord()
    {
        //Color all letters in wordToColor 
        //Element words match their element color, meta words are pink, shape words remain white
        //Words that are not recognized are colored black

        Color colorToUse = Color.white; 
        if (wordDatabase.shapeWords.Contains(spellWord))
        {
            colorToUse = Color.white;
        }
        else if (wordDatabase.elementWords.Contains(spellWord) && elementalNotFound)
        {
            if (wordDatabase.fireWords.Contains(spellWord))
            {
                colorToUse = new Color(1f, 0.5f, 0f, 1f); // bright orange
            }
            else if (wordDatabase.waterWords.Contains(spellWord))
            {
                colorToUse = Color.blue;
            }
            else if (wordDatabase.earthWords.Contains(spellWord))
            {
                colorToUse = new Color(0.5f, 0.25f, 0f, 1f); // brown
            }
            else if (wordDatabase.airWords.Contains(spellWord))
            {
                colorToUse = Color.gray;
            }
            else if (wordDatabase.shockWords.Contains(spellWord))
            {
                colorToUse = Color.yellow;
            }
            else if (wordDatabase.iceWords.Contains(spellWord))
            {
                colorToUse = Color.cyan;
            }
            else if (wordDatabase.lightWords.Contains(spellWord))
            {
                colorToUse = new Color(1f, 1f, 0.5f, 1f); // light yellow
            }
            else if (wordDatabase.darkWords.Contains(spellWord))
            {
                colorToUse = new Color(0.5f, 0f, 0.5f, 1f); // dark purple
            } else
            {
                colorToUse = Color.black;
            }
            elementalNotFound = false;
        }
        else if (wordDatabase.metaWords.Contains(spellWord))
        {
            colorToUse = Color.magenta;
        } else
        {
            colorToUse = Color.black;
        }
        foreach (GameObject letterObj in wordToColor)        {
            SpriteRenderer renderer = letterObj.GetComponent<SpriteRenderer>();
            if (renderer != null)            {
                renderer.color = colorToUse;    
            }
        }
        wordToColor.Clear();
    }


    //Spell shape methods

    //Bolt-Single Target, hits once
    void CastBolt(float damage, string element)
    {
        SpawnProjectile(boltPrefab, currentTarget, element, damage, "bolt", false);
    }
    //Ball-Single Target, hits once
    void CastBall(float damage, string element)
    {
        SpawnProjectile(ballPrefab, currentTarget, element, damage, "ball", false);
    }
    //Missile-Multi Target, hits 3 times with reduced damage
    void CastMissile(float damage, string element)
    {
        StartCoroutine(SpawnMissilesCoroutine(damage, element));
    }

    private System.Collections.IEnumerator SpawnMissilesCoroutine(float damage, string element)
    {
        int missileCount = 3;
        float delay = 0.2f; 

        for (int i = 0; i < missileCount; i++)
        {
            SpawnProjectile(missilePrefab, currentTarget, element, damage / missileCount, "missile", false);
            yield return new WaitForSeconds(delay);
        }
    }
    //Beam-Single Target, hits multiple times with reduced damage
    void CastBeam(float damage, string element)
    {
        StartCoroutine(SpawnBeamCoroutine(damage, element));
    }
    private System.Collections.IEnumerator SpawnBeamCoroutine(float damage, string element)
    {
        int beamCount = 20;
        float delay = 0.05f; 

        for (int i = 0; i < beamCount; i++)
        {
            SpawnProjectile(beamPrefab, currentTarget, element, damage / beamCount, "beam", false);
            yield return new WaitForSeconds(delay);
        }
    }
    //Slash-Multi Target, hits 2 times with reduced damage
    void CastSlash(float damage, string element)
    {
        StartCoroutine(SpawnSlashCoroutine(damage, element));
    }
    private System.Collections.IEnumerator SpawnSlashCoroutine(float damage, string element)
    {
        int slashCount = 2;
        float delay = 0.1f; 

        for (int i = 0; i < slashCount; i++)
        {
            SpawnProjectile(slashPrefab, currentTarget, element, damage / slashCount, "slash", false);
            yield return new WaitForSeconds(delay);
        }
    }
    //Spear- pierces through all enemies
    void CastSpear(float damage, string element)
    {
        SpawnProjectile(spearPrefab, currentTarget, element, damage, "spear", true);
    }
    //Drill -Pierces and hits multiple times
    void CastDrill(float damage, string element)
    {
        SpawnProjectile(drillPrefab, currentTarget, element, damage/5, "drill", true);  
    }  
}
