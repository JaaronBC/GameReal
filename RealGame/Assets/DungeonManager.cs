using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour {
    [Header("Floor Layouts")]
    public GameObject[] floorLayoutPrefabs; // Assign your layout prefabs in order

    [Header("Prefabs")]
    public GameObject barrelPrefab;
    public GameObject chestPrefab;
    public GameObject holePrefab;

    [Header("Spawn Settings")]
    public int minBarrels = 2;
    public int maxBarrels = 5;
    public int minChests = 1;
    public int maxChests = 2;

    [Header("Dungeon Settings")]
    public int currentFloor = 1;
    public int maxFloors = 5;

    [Header("References")]
    public Transform spawnedObjectsParent;
    public Transform player;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject currentLayoutInstance;
    private Tilemap currentMarkerTilemap;
    [SerializeField] SpellbookController spellbookController; // Reference to the SpellbookController script

    //Array for Enemies
    public GameObject[] enemyPrefabs;
    //Counter for enemy IDs
    int enemyIDcounter = 1;
    void Start() {
        GenerateFloor();
        spellbookController.AddLetter('B');
        spellbookController.AddLetter('O');
        spellbookController.AddLetter('L');
        spellbookController.AddLetter('T');
        spellbookController.AddLetter('A');
        BattleDataHolder.ConsonantsLeft.Remove('B');
        BattleDataHolder.VowelsLeft.Remove('O');
        BattleDataHolder.ConsonantsLeft.Remove('L');
        BattleDataHolder.ConsonantsLeft.Remove('T');
        BattleDataHolder.VowelsLeft.Remove('A');
        //Give the player 3 random consonants
        for (int i = 0; i < 3; i++) {
            if (BattleDataHolder.ConsonantsLeft.Count > 0) {
                char randomConsonant = GetRandomCharFromSet(BattleDataHolder.ConsonantsLeft);
                spellbookController.AddLetter(randomConsonant);
                BattleDataHolder.ConsonantsLeft.Remove(randomConsonant);
            }
        }
        //Give the player a random vowel
        if (BattleDataHolder.VowelsLeft.Count > 0) {
            char randomVowel = GetRandomCharFromSet(BattleDataHolder.VowelsLeft);
            spellbookController.AddLetter(randomVowel);
            BattleDataHolder.VowelsLeft.Remove(randomVowel);
        }
    }
    char GetRandomCharFromSet(HashSet<char> charSet) {
        int index = Random.Range(0, charSet.Count);
        foreach (char c in charSet) {
            if (index == 0) return c;
            index--;
        }
        return '\0'; // Should never reach here
    }

    public void GenerateFloor() {
        ClearSpawnedObjects();
        LoadFloorLayout();

        List<Vector3Int> markerPositions = GetMarkerPositions();
        if (markerPositions.Count == 0) {
            Debug.LogWarning("No marker tiles found.");
            return;
        }

        ShuffleList(markerPositions);

        int barrelCount = Random.Range(minBarrels, maxBarrels + 1);
        int chestCount = Random.Range(minChests, maxChests + 1);
        int index = 0;

        for (int i = 0; i < barrelCount && index < markerPositions.Count; i++, index++)
            SpawnObjectAtCell(barrelPrefab, markerPositions[index]);

        for (int i = 0; i < chestCount && index < markerPositions.Count; i++, index++)
            SpawnObjectAtCell(chestPrefab, markerPositions[index]);

        if (currentFloor < maxFloors) {
            if (index < markerPositions.Count)
                SpawnObjectAtCell(holePrefab, markerPositions[index]);
            else
                Debug.LogWarning("Not enough marker positions for the hole.");
        } else {
            Debug.Log("Final floor reached!");
        }

        // Spawn player at the marker tilemap's center
        if (player != null) {
            Vector3 center = currentMarkerTilemap.localBounds.center;
            center.z = 0;
            player.position = center;
        }

        Debug.Log("Generated floor " + currentFloor);
    }

    void LoadFloorLayout() {
        // Destroy old layout
        if (currentLayoutInstance != null)
            Destroy(currentLayoutInstance);

        // Pick layout � cycle through available prefabs
        int layoutIndex = (currentFloor - 1) % floorLayoutPrefabs.Length;
        GameObject prefab = floorLayoutPrefabs[layoutIndex];

        currentLayoutInstance = Instantiate(prefab, new Vector3(0,0,1), Quaternion.identity);

        // Find the marker tilemap inside the new layout
        Tilemap[] tilemaps = currentLayoutInstance.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tm in tilemaps) {
            if (tm.gameObject.name.Contains("Marker")) {
                currentMarkerTilemap = tm;
            } else if (tm.gameObject.name.Contains("EnemySpawn")) {
                // Spawn enemies at EnemySpawn tilemap positions
                SpawnEnemiesAtTilemap(tm);
            }
        }

        if (currentMarkerTilemap == null)
            Debug.LogError("No Marker tilemap found in floor layout prefab!");
    }

    List<Vector3Int> GetMarkerPositions() {
        List<Vector3Int> positions = new List<Vector3Int>();
        BoundsInt bounds = currentMarkerTilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin) {
            if (currentMarkerTilemap.HasTile(pos))
                positions.Add(pos);
        }
        return positions;
    }

    void SpawnObjectAtCell(GameObject prefab, Vector3Int cellPosition) {
        if (prefab == null) return;
        Vector3 worldPosition = currentMarkerTilemap.GetCellCenterWorld(cellPosition);
        worldPosition.z = -1;
        GameObject spawned = Instantiate(prefab, worldPosition, Quaternion.identity, spawnedObjectsParent);
        spawnedObjects.Add(spawned);
    }

    void ClearSpawnedObjects() {
        foreach (GameObject obj in spawnedObjects)
            if (obj != null) Destroy(obj);
        spawnedObjects.Clear();
    }

    void ShuffleList(List<Vector3Int> list) {
        for (int i = 0; i < list.Count; i++) {
            int randomIndex = Random.Range(i, list.Count);
            Vector3Int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void GoToNextFloor() {
        if (currentFloor < maxFloors) {
            currentFloor++;
            GenerateFloor();
        } else {
            Debug.Log("You reached the bottom of the cavern!");
        }
    }
    void SpawnEnemiesAtTilemap(Tilemap enemySpawnTilemap) {
        BoundsInt bounds = enemySpawnTilemap.cellBounds; 
        foreach (Vector3Int pos in bounds.allPositionsWithin) {
            if (enemySpawnTilemap.HasTile(pos)) {
                Vector3 worldPosition = enemySpawnTilemap.GetCellCenterWorld(pos);
                worldPosition.z = -1;
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                var enemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity, spawnedObjectsParent);
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                if (enemyScript != null)                {
                    enemyScript.enemyID = "enemy" + enemyIDcounter;
                    enemyIDcounter++;
                }
            }
        }
    }
    public bool CheckForEnemies() {
        bool enemiesRemaining = false;
        FindObjectsOfType<EnemyScript>();
        foreach (EnemyScript enemy in FindObjectsOfType<EnemyScript>()) {
            if (enemy != null) {
                enemiesRemaining = true;
                break;
            }
        }
        return enemiesRemaining;
    }
}
