using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class PlayerCasting : MonoBehaviour
{
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
    [SerializeField] WordDatabase wordDatabase;
    public int spellsCast = 0;
    //Prefabs for shape words
    [SerializeField] GameObject boltPrefab;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject beamPrefab;
    [SerializeField] GameObject slashPrefab;
    
    public void BeginTurn()
    {
        isActive = true;
        if (battleScript.activeEnemies.Count > 0)
        {
            currentTarget = battleScript.activeEnemies[currentTargetIndex];
            HighlightTarget(currentTarget);
        }
    }
    void CycleTarget()
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
                var enemyRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemyRenderer != null)
                {
                    enemyRenderer.color = Color.white;
                }
            }
        }
        // Highlight current target
        renderer.color = Color.red;
    }

    void SpawnProjectile(GameObject prefab, GameObject target, string element, float damage, string shape)
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
                case "water":
                    colorToUse = Color.blue;
                    break;
                case "light":
                    colorToUse = Color.white;
                    break;
                case "dark":
                    colorToUse = Color.black;
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
                    metaMultiplier += 0.2f; // Each meta word increases multiplier by 20%
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
        shapeActions = new Dictionary<string, System.Action<float, string>>()
        {
            { "bolt", CastBolt },
            { "ball", CastBall },
            { "missile", CastMissile },
            { "beam", CastBeam },
            {"laser", CastBeam},
            {"ray", CastBeam},
            {"slash", CastSlash}
        };
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

        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsLetter(c))
                {
                    spellWord += c;
                    //Debug.Log("Current Spell Word: " + spellWord);
                }
                else if (c == ' ')
                {
                    if (!string.IsNullOrEmpty(spellWord))
                    {
                        spellBuilder.Add(spellWord);
                        if (wordDatabase.shapeWords.Contains(spellWord))
                        {
                            CraftSpell();
                            spellBuilder.Clear();
                        }
                        Debug.Log("Spell Added: " + spellWord);
                        spellWord = "";
                    }
                }
            }
        }
    }


    //Spell shape methods

    //Bolt-Single Target, hits once
    void CastBolt(float damage, string element)
    {
        SpawnProjectile(boltPrefab, currentTarget, element, damage, "bolt");
    }
    //Ball-Single Target, hits once
    void CastBall(float damage, string element)
    {
        SpawnProjectile(ballPrefab, currentTarget, element, damage, "ball");
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
            SpawnProjectile(missilePrefab, currentTarget, element, damage / missileCount, "missile");
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
            SpawnProjectile(beamPrefab, currentTarget, element, damage / beamCount, "beam");
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
            SpawnProjectile(slashPrefab, currentTarget, element, damage / slashCount, "slash");
            yield return new WaitForSeconds(delay);
        }
    }
}
