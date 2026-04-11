using UnityEditor.Rendering;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    //object components
    SpriteRenderer spriteRenderer;
    Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D playerDetectionRadius;

    //movement variables
    public float z = 0.5f;
    private int timer = 0;
    float currentSpeed = 0.0f;
    float speed = 2f;
    float chaseSpeed = 3f;
    Vector3 direction = new Vector2(0.0f, 0.0f);

    //state machine
    private string state = "normal";

    //normal
    private int normalStillProbability = 50;
    private int[] normalTimeRange = { 180, 360 };

    //player chase
    Transform target;
    public float chaseRadius = 3.5f;
    private int[] chaseTimeRange = { 60, 120 };

    //battle transition
    private string battleSceneName = "BattleScene";
    public GameObject battlePrefab;
    private bool isTransitioning = false;

    public string enemyID; // Unique identifier for the enemy, can be set in the inspector

    public bool isMovingEnemy;



    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
        if (!BattleDataHolder.enemyDatabase.ContainsKey(enemyID))
        {
            EnemySaveData data = new EnemySaveData
            {
                id = enemyID,
                position = transform.position,
                defeated = false,
                returnablePosition = false
            };
            BattleDataHolder.enemyDatabase.Add(enemyID, data);
            Debug.Log("Logged Enemy: " + enemyID);
        }
        else
        {
            // If already exists, check if defeated
            if (BattleDataHolder.enemyDatabase[enemyID].defeated)
            {
                Destroy(gameObject);
            }
        }
        if (BattleDataHolder.enemyDatabase.ContainsKey(enemyID) && BattleDataHolder.enemyDatabase[enemyID].returnablePosition)
        {
            transform.position = BattleDataHolder.enemyDatabase[enemyID].position;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        switch (state) {
            case "normal":
                if (timer == 0)
                {
                    timer = Random.Range(normalTimeRange[0], normalTimeRange[1]);
                    MoveNormal();
                }
                if (DetectPlayer()) state = "chase";
                break;
            case "chase":
                if (timer == 0 && target)
                {
                    if (!DetectPlayer()) state = "normal";
                    timer = Random.Range(chaseTimeRange[0], chaseTimeRange[1]);
                    MoveToPlayer();
                }
                break;
        }

        //movement
        rb.linearVelocity = new Vector2(direction.x, direction.y) * currentSpeed;

        //main timer reset
        if (timer > 0) timer--;
    }

    public void MoveNormal()
    {
        if (!isMovingEnemy) return;

        currentSpeed = speed;
        if (Random.Range(1, 101) > normalStillProbability)
        {
            direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }
        else
        {
            currentSpeed = 0.0f;
        }
        DirectionToSprite();
    }

    public void MoveToPlayer()
    {
        if (!isMovingEnemy) return;
        currentSpeed = chaseSpeed;
        direction = (target.position - transform.position).normalized;
        DirectionToSprite();
    }

    public bool DetectPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position,
            chaseRadius, LayerMask.GetMask("Player"));
        RaycastHit2D tileRay = Physics2D.Linecast(transform.position, target.position,
            LayerMask.GetMask("Tiles"));

        if (collider != null && tileRay == false) return true;
        return false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTransitioning) return;
        EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

        if (collision.gameObject.CompareTag("Player"))
        {
            isTransitioning = true;
            print("player battle detected");

            foreach (EnemyScript enemy in allEnemies)
            {
                if (BattleDataHolder.enemyDatabase.ContainsKey(enemy.enemyID))
                {
                //Set returnablePosition to true and update position in EnemySaveData for each enemy in the scene
                BattleDataHolder.enemyDatabase[enemy.enemyID].returnablePosition = true;
                BattleDataHolder.enemyDatabase[enemy.enemyID].position = enemy.transform.position;
                }
                else
                {
                    BattleDataHolder.enemyDatabase[enemy.enemyID] = new EnemySaveData
                    {
                    id = enemy.enemyID,
                    position = enemy.transform.position,
                    defeated = false,
                    returnablePosition = false
                    };
                }
            }

            Collider2D[] found = Physics2D.OverlapCircleAll(collision.transform.position, 5f);

            HashSet<EnemyScript> countedEnemies = new HashSet<EnemyScript>();

            List<GameObject> enemies = new List<GameObject>();

            foreach (var col in found)
            {
                if (col.CompareTag("Enemy"))
                {
                    EnemyScript enemy = col.GetComponent<EnemyScript>();

                    if (enemy != null && enemy.battlePrefab != null)
                    {
                        if (!countedEnemies.Contains(enemy))
                        {
                            countedEnemies.Add(enemy);
                            Debug.Log("Adding enemy to battle: " + enemy.name);
                            enemies.Add(enemy.battlePrefab);
                            BattleDataHolder.activeEnemyIDs.Add(enemy.enemyID);
                        }
                    }
                }
            }
            BattleDataHolder.returnSceneName = SceneManager.GetActiveScene().name;
            BattleDataHolder.playerPosition = GameObject.Find("Player").transform.position;
            BattleDataHolder.hasReturnPosition = true;
            BattleDataHolder.enemiesToSpawn = enemies.ToArray();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (playerMovement.z <= z)
                    BattleScreenTransition(battleSceneName);
            }
            BattleScreenTransition(battleSceneName);
        }
    }

    public void BattleScreenTransition(string sceneName)
    {
        SceneController sc = FindObjectOfType<SceneController>();
        if (sc != null)
        {
            sc.LoadScene(sceneName);
        }
    }

    public void DirectionToSprite()
    {
        //flip
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        //sprite animation
        animator.SetFloat("dir_x", (direction.x));
        animator.SetFloat("dir_y", (direction.y));
        if (currentSpeed > 0) animator.SetBool("move", true);
        else animator.SetBool("move", false);
    }

}
