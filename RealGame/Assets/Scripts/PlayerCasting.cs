using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    [SerializeField] BattleScript battleScript;

    int currentTargetIndex = 0;
    GameObject currentTarget;

    public bool isActive = false;

    int debugCounter = 0;
    public int damage;
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
        if (renderer != null)
        {
            foreach (var enemy in battleScript.activeEnemies)
            {
                var enemyRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemyRenderer != null)                {
                    enemyRenderer.color = Color.white;
                }
            }
            renderer.color = Color.red;
        }
    }

    void Attack()
    {
        if (currentTarget == null)
        {
            Debug.LogWarning("No target selected!");
            return;
        }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        //Cycle through targets using left shift key
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CycleTarget();
        }
        //Attack current target using space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();   
        }

    }

}
