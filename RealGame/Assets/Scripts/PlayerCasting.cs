using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class PlayerCasting : MonoBehaviour
{
    [SerializeField] BattleScript battleScript;
    [SerializeField] private TextMeshProUGUI spellBuildText;

    int currentTargetIndex = 0;
    GameObject currentTarget;

    public bool isActive = false;

    int debugCounter = 0;

    public List<string> spellBuilder = new List<string>();
    public string spellWord = "";
    [SerializeField] WordDatabase wordDatabase;
    public int spellsCast = 0;
    [SerializeField] GameObject boltPrefab;
    [SerializeField] GameObject ballPrefab;
    private Transform castPoint;
    
    
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
        //Debug.Log("Current Target: " + target.name + " at index " + currentTargetIndex);
        //Color target red to indicate it's the current target
        var renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            foreach (var enemy in battleScript.activeEnemies)
            {
                var enemyRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemyRenderer != null)    
                {
                    enemyRenderer.color = Color.white;
                }
            }
            renderer.color = Color.red;
        }
    }

    void SpawnProjectile(GameObject prefab, GameObject target, string element)
    {
        // Instantiate projectile at player's coordinates (replace with your cast point if needed)
        GameObject proj = Instantiate(prefab, new Vector3(4.5f, 3f, 0f), Quaternion.identity);

        // Get SpriteRenderer; handles nested children as well
        SpriteRenderer projRenderer = proj.GetComponentInChildren<SpriteRenderer>();
        if (projRenderer != null)
        {
            Color colorToUse = Color.white; // default

            switch (element?.ToLower()) // ensure lowercase comparison and null safe
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
        projRenderer.sortingOrder = 10; // ensures it appears in front of other sprites
    }

    // Assign target for projectile movement
    ProjectileScript projectile = proj.GetComponent<ProjectileScript>();
    if (projectile != null && target != null)
    {
        projectile.target = target.transform;
    }
}

    void CraftSpell()
    {
        Debug.Log("Crafting spell with components: " + string.Join(", ", spellBuilder));
        if (spellBuilder.Count == 0) return;
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
                metaMultiplier += 0.2f; // Each meta word increases multiplier by 20%
            }
        }
        if (element == null)
        {
            baseMultiplier -= 0.2f; // No element reduces base damage by 20%
        }
        baseMultiplier -= 0.1f * spellsCast; // Each spell cast reduces base multiplier by 10%
        float damage = baseDamage * baseMultiplier * metaMultiplier;
        //Switch case for shape to determine attack type
        switch (shape)
        {            
            case "bolt":
                Attack(damage, element);
                break;
            case "ball":
                AreaAttack(damage, element);
                break;
        }   
    }

    void Attack(float damage, string element)
    {
    Debug.Log($"Attacking with {element} {damage} damage! Target: {currentTarget.name}");
    //For single target attack, apply damage to current target
        if (currentTarget == null)
        {
            Debug.LogWarning("No target selected!");
            return;
        }
        SpawnProjectile(boltPrefab, currentTarget, element);
        
        EnemyState enemyState = currentTarget.GetComponent<EnemyState>();
        if (enemyState != null)
        {
            enemyState.currentHP -= damage; // Subtract HP
            Debug.Log($"Attacked {currentTarget.name} for {damage} damage! Remaining HP: {enemyState.currentHP}");

        
            if (enemyState.currentHP < 0)
            enemyState.currentHP = 0;

            if (enemyState.currentHP <= 0)
            {
                battleScript.activeEnemies.Remove(currentTarget);
                Debug.Log($"{currentTarget.name} defeated!");
                Destroy(currentTarget); // or trigger death animation
                currentTarget = null;
                if (battleScript.activeEnemies.Count > 0)
                {
                    currentTargetIndex = 0;
                    currentTarget = battleScript.activeEnemies[currentTargetIndex];
                    HighlightTarget(currentTarget);
                }
            }
        }
    }
void AreaAttack(float damage, string element)
{
    Debug.Log($"Attacking with {element} area attack for {damage} damage!");
    if (currentTarget == null)
    {
        Debug.LogWarning("No target selected!");
        return;
    }

    var enemies = battleScript.activeEnemies;
    SpawnProjectile(ballPrefab, currentTarget, element);

    for (int i = enemies.Count - 1; i >= 0; i--)
    {
        GameObject enemy = enemies[i];

        if (enemy == null) continue;

        EnemyState enemyState = enemy.GetComponent<EnemyState>();
        if (enemyState != null)
        {
            enemyState.currentHP -= damage;

            Debug.Log($"Attacked {enemy.name} for {damage} damage! Remaining HP: {enemyState.currentHP}");

            if (enemyState.currentHP < 0)
                enemyState.currentHP = 0;

            if (enemyState.currentHP <= 0)
            {
                Debug.Log($"{enemy.name} defeated!");

                enemies.RemoveAt(i);

                Destroy(enemy);
            }
        }
    }

    if (enemies.Count > 0)
    {
        if (currentTargetIndex >= enemies.Count)
            currentTargetIndex = 0;

        currentTarget = enemies[currentTargetIndex];
        HighlightTarget(currentTarget);
    }
    else
    {
        currentTarget = null;
        Debug.Log("All enemies defeated!");
    }
}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        //Start accepting keyboard input for spells
        //When the player presses a letter key append to to spellWord
        //Once player presses space push into spellBuilder and clear spellWord
        //If a shape word from shapeWords is inserted into spellBuilder, 
        //call CraftSpell to determine spell type and apply effects
        //then clear spellBuilder for next spell
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
}
