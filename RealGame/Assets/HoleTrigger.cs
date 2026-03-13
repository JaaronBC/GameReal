using UnityEngine;

public class HoleTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Something entered the hole: " + other.name);

        if (other.CompareTag("Player")) {
            Debug.Log("Player entered the hole!");

            DungeonManager manager = FindFirstObjectByType<DungeonManager>();

            if (manager != null) {
                manager.GoToNextFloor();
            } else {
                Debug.LogWarning("DungeonManager not found.");
            }
        }
    }
}