using UnityEngine;
using System.Collections.Generic;

public enum jaaronBattleState { START, PLAYERTURN, ENEMYTURN, END }
public class BattleSystem : MonoBehaviour
{
    public int playerX, playerY;

    public int enemyGridMinX, enemyGridMaxX;

    public int enemyGridMinY, enemyGridMaxY; 
    public GameObject playerPrefab;
    public GameObject[] enemies;
    public jaaronBattleState state;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = jaaronBattleState.START;
        SetupBattle();
    }


    void SetupBattle()
    {
        //Sets up battle
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
    //Sets the state to Player Turn state
    state = jaaronBattleState.PLAYERTURN;
    PlayerTurn();
    }
    void PlayerTurn()
    {
        
    }

}
