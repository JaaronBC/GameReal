using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour {
    [Header("Tilemaps")]
    public Tilemap markerTilemap;

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

    [Header("Optional")]
    public Transform spawnedObjectsParent;
    public Transform player;
    public Transform playerSpawnPoint;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start() {
        GenerateFloor();
    }

    public void GenerateFloor() {
        ClearSpawnedObjects();

        List<Vector3Int> markerPositions = GetMarkerPositions();

        if (markerPositions.Count == 0) {
            Debug.LogWarning("No marker tiles found on MarkerTilemap.");
            return;
        }

        ShuffleList(markerPositions);

        int barrelCount = Random.Range(minBarrels, maxBarrels + 1);
        int chestCount = Random.Range(minChests, maxChests + 1);

        int index = 0;

        for (int i = 0; i < barrelCount && index < markerPositions.Count; i++, index++) {
            SpawnObjectAtCell(barrelPrefab, markerPositions[index]);
        }

        for (int i = 0; i < chestCount && index < markerPositions.Count; i++, index++) {
            SpawnObjectAtCell(chestPrefab, markerPositions[index]);
        }

        if (currentFloor < maxFloors) {
            if (index < markerPositions.Count) {
                SpawnObjectAtCell(holePrefab, markerPositions[index]);
            } else {
                Debug.LogWarning("Not enough marker positions left to place the hole.");
            }
        }

        if (player != null && playerSpawnPoint != null) {
            player.position = playerSpawnPoint.position;
        }

        Debug.Log("Generated floor " + currentFloor);
    }

    List<Vector3Int> GetMarkerPositions() {
        List<Vector3Int> positions = new List<Vector3Int>();

        BoundsInt bounds = markerTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin) {
            if (markerTilemap.HasTile(pos)) {
                positions.Add(pos);
            }
        }

        return positions;
    }

    void SpawnObjectAtCell(GameObject prefab, Vector3Int cellPosition) {
        if (prefab == null) {
            Debug.LogWarning("Tried to spawn a null prefab.");
            return;
        }

        Vector3 worldPosition = markerTilemap.GetCellCenterWorld(cellPosition);
        worldPosition.z = -1;

        GameObject spawned = Instantiate(
            prefab,
            worldPosition,
            Quaternion.identity,
            spawnedObjectsParent
        );

        spawnedObjects.Add(spawned);
    }

    void ClearSpawnedObjects() {
        for (int i = 0; i < spawnedObjects.Count; i++) {
            if (spawnedObjects[i] != null) {
                Destroy(spawnedObjects[i]);
            }
        }

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
}
