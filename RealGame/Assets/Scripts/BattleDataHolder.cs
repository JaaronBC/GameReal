using UnityEngine;
using System.Collections.Generic;

public static class BattleDataHolder
{
    public static GameObject[] enemiesToSpawn;
    public static string returnSceneName;
    public static Vector3 playerPosition;
    public static List<string> activeEnemyIDs = new List<string>();
    public static Dictionary<string, EnemySaveData> enemyDatabase = new Dictionary<string, EnemySaveData>();
    public static bool hasReturnPosition = false;
    public static char[] usableLetters = new char[26];
    public static HashSet<char> ConsonantsLeft = new HashSet<char>("BCDFGHJKLMNPQRSTVWXYZ".ToCharArray());
    public static HashSet<char> VowelsLeft = new HashSet<char>("AEIOU".ToCharArray());
    public static bool startOfRun = true;
    public static Dictionary<string, int> shapeSpellSpritesPointer = new Dictionary<string, int>
    {
        {"bolt", 1},
        {"ball", 0},
        {"missile", 4},
        {"beam", 2},
        {"slash", 3},
        {"spear", 5},
        {"drill", 6},
        {"sword", 7},
        {"dagger", 8},
        {"arrow", 9},
        {"ray", 2},
        {"laser", 2},
        {"cut", 3},
        {"stab", 5},
        {"pierce", 5},
        {"lance", 5},
        {"javelin", 5},
        {"blade", 7},
        {"knife", 8},
        {"bow", 9},
        {"quiver", 9}
    };
    public static HashSet<char> LettersGained = new HashSet<char>();
    public static bool characterMapCreated = false;
    public static Dictionary<string, HashSet<char>> shapeWordCharacterMap;
    public static Dictionary<string, HashSet<char>> elementWordCharacterMap;
    public static Dictionary<string, HashSet<char>> powerWordCharacterMap;
    public static HashSet<string> shapeWordsLeft = new HashSet<string>();
    public static HashSet<string> elementWordsLeft = new HashSet<string>();
    public static HashSet<string> powerWordsLeft = new HashSet<string>();
    public static HashSet<string> unlockedShapeWords = new HashSet<string>();
    public static HashSet<string> unlockedElementWords = new HashSet<string>();
    public static HashSet<string> unlockedPowerWords = new HashSet<string>();
    public static int currentFloor = 1;
    public static int dungeonSeed;
}
