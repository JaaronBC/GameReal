using System.Diagnostics.CodeAnalysis;
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




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = BattleState.Start;
        setUpBattle();

    }

    // Update is called once per frame
    void Update()
    {
        selfTextComponent.text = "timer: " + timer + " state: " + state;

        //timer and set states
        if (timer > 0.0) timer -= Time.deltaTime;
        else {
            switch (state) {
                case BattleState.Start:
                    state = BattleState.EnemyTurn;
                    timer = Random.Range(baseTimeChange[0], baseTimeChange[1]) + addedTime;
                    break;
                case BattleState.EnemyTurn:
                    state = BattleState.EnemySwitch;
                    timer = 1f;
                    break;
                case BattleState.EnemySwitch:
                    state = BattleState.PlayerTurn;
                    timer = 5f;
                    break;
                case BattleState.PlayerTurn:
                    state = BattleState.PlayerSwitch;
                    timer = 1f;
                    break;
                case BattleState.PlayerSwitch:
                    state = BattleState.EnemyTurn;
                    timer = Random.Range(baseTimeChange[0], baseTimeChange[1]) + addedTime;
                    break;
            }
        }


    }


    void setUpBattle()
    {
        var currentPlayer = Instantiate(playerPrefab, new Vector3 (playerX, playerY), Quaternion.identity);
        currentPlayer.name = "PlayerObject";
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
        }
        timer = startTime; 

    }


}
