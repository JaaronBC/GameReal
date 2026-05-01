using UnityEngine;

public class HoleTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Something entered the hole: " + other.name);

        if (other.CompareTag("Player")) {

            Debug.Log("Player entered the hole!");

            DungeonManager manager = FindFirstObjectByType<DungeonManager>();
            SpellbookController spellbook = FindFirstObjectByType<SpellbookController>();
            RandomLetter randomLetter = FindFirstObjectByType<RandomLetter>();

            if (manager != null) {
                if (!manager.CheckForEnemies()) {
                    manager.GoToNextFloor();
                    if (BattleDataHolder.VowelsLeft.Count > 0)
                    {
                        char randomVowel = randomLetter.RandomVowel();
                        if (randomVowel != '\0')                        {
                            spellbook.AddLetter(randomVowel);
                        }
                    }
                } else {
                    Debug.Log("Cannot go to the next floor, enemies are still remaining.");
                }
            } else {
                Debug.LogWarning("DungeonManager not found.");
            }
        }
    }
}