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
}
