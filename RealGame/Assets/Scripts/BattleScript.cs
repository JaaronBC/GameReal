using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro; //text mesh pro
using System.Collections.Generic;


public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    EnemySwitch,
    PlayerSwitch,
    BattleEnd
}

public class BattleScript : MonoBehaviour
{

    private float timer = 0.0f;
    //start turn variables
    private float startTime = 3f;
    //switch variables
    private float switchTime = 1f;
    //enemy turn variables
    private float[] baseTimeChange = { 4f, 5f };
    float addedTime = 10.0f;
    //turn order
    public BattleState state;
    //objects
    public TextMeshProUGUI selfTextComponent;
    //Coordinates for player spawn
    public float playerX, playerY;
    //Coordinates for enemy spawn range
    public int enemyGridMinX, enemyGridMaxX;

    public int enemyGridMinY, enemyGridMaxY; 
    //prefab for player object
    public GameObject playerPrefab;
    //array of enemy prefabs to be spawned
    public GameObject[] enemies;
    //List of active enemies to be used for targeting and checking if all enemies are defeated
    public List<GameObject> activeEnemies = new List<GameObject>();
    //reference to player movement script to enable and disable during turns
    public PlayerMovement playerMovement;
    Vector3 targetPosition;
    bool movePlayer = false;
    [SerializeField] PlayerCasting playerCasting;
    //reference to player object to move during player turn
    //will be set in start function after player prefab is instantiated
    GameObject playerObject;
    //Health Bar
    public PlayerHealthBar playerHealthBar;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BattleDataHolder.enemiesToSpawn != null)
        {
            enemies = BattleDataHolder.enemiesToSpawn;
        }
        //For each enemyID in BattleDataHolder.activeEnemyIDs, set the corresponding EnemySaveData's defeated value to true
        HashSet<string> countedEnemies = new HashSet<string>();
        foreach (string enemyID in BattleDataHolder.activeEnemyIDs)
        {
            if (BattleDataHolder.enemyDatabase.ContainsKey(enemyID))
            {
                EnemySaveData enemy = BattleDataHolder.enemyDatabase[enemyID];
                if (!enemy.defeated && !countedEnemies.Contains(enemyID))
                {                    enemy.defeated = true; 
                }
            }
        }
        state = BattleState.Start;
        setUpBattle();
        playerObject = GameObject.Find("PlayerObject");
    }

    // Update is called once per frame
    void Update()
    {   
        //for moving player to the center of the screen during player turn
        if (movePlayer) {
        GameObject playerObject = GameObject.Find("PlayerObject");

        playerObject.transform.position = Vector3.MoveTowards(
            playerObject.transform.position,
            targetPosition,
            5f * Time.deltaTime
        );

        if (Vector3.Distance(playerObject.transform.position, targetPosition) < 0.01f) {
            movePlayer = false;
        }
    }
        selfTextComponent.text = "timer: " + timer + " state: " + state + " Movement:" + playerMovement.enabled;

        //timer and set states
        if (timer > 0.0) timer -= Time.deltaTime;
        else {
            switch (state) {
                case BattleState.Start:
                    SwitchState(BattleState.EnemyTurn);
                    timer = Random.Range(baseTimeChange[0], baseTimeChange[1]) + addedTime;
                    break;
                case BattleState.EnemyTurn:
                    SwitchState(BattleState.EnemySwitch);

                    timer = 1f;
                    break;
                case BattleState.EnemySwitch:
                    SwitchState(BattleState.PlayerTurn);
                    
                    timer = 10f;
                    break;
                case BattleState.PlayerTurn:
                    SwitchState(BattleState.PlayerSwitch);
                    timer = 1f;
                    break;
                case BattleState.PlayerSwitch:
                    if (activeEnemies.Count == 0) 
                    {
                        SwitchState(BattleState.BattleEnd);
                        timer = 1f;
                    }
                    else 
                    {
                    SwitchState(BattleState.EnemyTurn);
                    timer = Random.Range(baseTimeChange[0], baseTimeChange[1]) + addedTime;
                    }
                    break;
                case BattleState.BattleEnd:
                    BattleEnd();
                    break;
            }
        }


    }
    void SwitchState(BattleState newState) {
        if (newState == BattleState.PlayerTurn) {
            playerMovement.enabled = false;

            targetPosition = new Vector3(playerX, playerY, playerObject.transform.position.z);
            movePlayer = true;
            playerCasting.BeginTurn();
        } else {
            playerCasting.isActive = false;
            playerMovement.enabled = true;
            playerCasting.spellsCast = 0;
            playerCasting.spellWord = "";
            playerCasting.spellBuilder.Clear();
            playerCasting.currentLetters.ForEach(letter => Destroy(letter));
            playerCasting.currentLetters.Clear();
            playerCasting.backspaceCounter = 0;
            playerCasting.wordToColor.Clear();
            playerCasting.elementalNotFound = true;
        } 
       
        state = newState;
    }


    void setUpBattle()
    {
        Debug.Log("Setup Battle");
        var currentPlayer = Instantiate(playerPrefab, new Vector3 (playerX, playerY), Quaternion.identity);
        currentPlayer.name = "PlayerObject";
        // Set player health bar reference
        PlayerState playerState = currentPlayer.GetComponent<PlayerState>();
        if (playerState != null)        {
            playerHealthBar.unit = playerState;
        }
        playerMovement = currentPlayer.GetComponent<PlayerMovement>();
        int randomX = -1;
        bool toggleRow = false;
        int randomY = Random.Range(enemyGridMinY, enemyGridMaxY);
        Dictionary<int, int> validPositions = new Dictionary<int, int>();
        HashSet<int> usedXPositions = new HashSet<int>();
        for (int x = enemyGridMinX; x <= enemyGridMaxX; x++) {
            // Alternate between minY and maxY for each column to ensure enemies are not spawned adjacent to each other
            validPositions.Add(x, randomY);
            randomY = toggleRow ? enemyGridMinY : enemyGridMaxY;
            toggleRow = !toggleRow;
        }
        int[] takenColumn = new int[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {   
            //Spawn enemy in a validPosition from Dictionary
            //Then add X position to usedXPositions to prevent spawning another enemy in the same column
            do
            {
            randomX = Random.Range(enemyGridMinX, enemyGridMaxX);
            }
            while (usedXPositions.Contains(randomX));
            usedXPositions.Add(randomX);
            var currentEnemy = Instantiate(enemies[i], new Vector3 (randomX, validPositions[randomX]), Quaternion.identity);
            currentEnemy.name = $"Enemy {i+1}";

            EnemyState enemyState = currentEnemy.GetComponent<EnemyState>();
            if (enemyState != null)
            {
                enemyState.battleScript = this;
            }

            activeEnemies.Add(currentEnemy);
        }
        timer = startTime; 

    }
    void BattleEnd()
    {
        Debug.Log("Battle Ended!");
        //Transition to previous scene
        SceneManager.LoadScene(BattleDataHolder.returnSceneName);
    }


}
