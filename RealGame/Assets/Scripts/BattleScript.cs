using System.Diagnostics.CodeAnalysis;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro; //text mesh pro
using System.Collections.Generic;
using System.Diagnostics;


public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    EnemySwitch,
    PlayerSwitch
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
    float addedTime = 0f;
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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                    SwitchState(BattleState.EnemyTurn);
                    timer = Random.Range(baseTimeChange[0], baseTimeChange[1]) + addedTime;
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
        } 
       
        state = newState;
    }


    void setUpBattle()
    {
        var currentPlayer = Instantiate(playerPrefab, new Vector3 (playerX, playerY), Quaternion.identity);
        currentPlayer.name = "PlayerObject";
        playerMovement = currentPlayer.GetComponent<PlayerMovement>();
        int randomX = -1;
        HashSet<int> usedXPositions = new HashSet<int>();
        int[] takenColumn = new int[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {   
            do
            {
            randomX = Random.Range(enemyGridMinX, enemyGridMaxX);
            }
            while (usedXPositions.Contains(randomX));
        
            int randomY = Random.Range(enemyGridMinY, enemyGridMaxY);
            usedXPositions.Add(randomX);
            var currentEnemy = Instantiate(enemies[i], new Vector3 (randomX, randomY), Quaternion.identity);
            currentEnemy.name = $"Enemy {i+1}";

            activeEnemies.Add(currentEnemy);
        }
        timer = startTime; 

    }


}
