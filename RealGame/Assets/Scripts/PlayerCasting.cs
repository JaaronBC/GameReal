using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    [SerializeField] BattleScript battleScript;

    int currentTargetIndex = 0;
    GameObject currentTarget;

    public bool isActive = false;

    int debugCounter = 0;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
        CycleTarget();
        }
    }
}
