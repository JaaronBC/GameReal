using UnityEngine;


public class GridController : MonoBehaviour
{
    [SerializeField] private int width, height;

    [SerializeField] private PlayerTile playerTilePrefab;

    [SerializeField] private EnemyTile enemyTilePrefab;
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid() {
    for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++)
            {
                var playerSpawnedTile = Instantiate(playerTilePrefab, new Vector3(x,y), Quaternion.identity);
                playerSpawnedTile.name = $"PlayerTile ({x}, {y})";

                var enemySpawnedTile = Instantiate(enemyTilePrefab, new Vector3(x,y+height+1), Quaternion.identity);
                enemySpawnedTile.name = $"EnemyTile ({x}, {y})";
            }
        }
    }
}