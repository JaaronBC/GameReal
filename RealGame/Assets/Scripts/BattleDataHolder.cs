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
    /*public static Dictionary<char, GameObject> letterPrefabs = new Dictionary<char, GameObject>
    {
        { 'A', Resources.Load<GameObject>("Letters/LetterA") },
        { 'B', Resources.Load<GameObject>("Letters/LetterB") },
        { 'C', Resources.Load<GameObject>("Letters/LetterC") },
        { 'D', Resources.Load<GameObject>("Letters/LetterD") },
        { 'E', Resources.Load<GameObject>("Letters/LetterE") },
        { 'F', Resources.Load<GameObject>("Letters/LetterF") },
        { 'G', Resources.Load<GameObject>("Letters/LetterG") },
        { 'H', Resources.Load<GameObject>("Letters/LetterH") },
        { 'I', Resources.Load<GameObject>("Letters/LetterI") },
        { 'J', Resources.Load<GameObject>("Letters/LetterJ") },
        { 'K', Resources.Load<GameObject>("Letters/LetterK") },
        { 'L', Resources.Load<GameObject>("Letters/LetterL") },
        { 'M', Resources.Load<GameObject>("Letters/LetterM") },
        { 'N', Resources.Load<GameObject>("Letters/LetterN") },
        { 'O', Resources.Load<GameObject>("Letters/LetterO") },
        { 'P', Resources.Load<GameObject>("Letters/LetterP") },
        { 'Q', Resources.Load<GameObject>("Letters/LetterQ") },
        { 'R', Resources.Load<GameObject>("Letters/LetterR") },
        { 'S', Resources.Load<GameObject>("Letters/LetterS") },
        { 'T', Resources.Load<GameObject>("Letters/LetterT") },
        { 'U', Resources.Load<GameObject>("Letters/LetterU") },
        { 'V', Resources.Load<GameObject>("Letters/LetterV") },
        { 'W', Resources.Load<GameObject>("Letters/LetterW") },
        { 'X', Resources.Load<GameObject>("Letters/LetterX") },
        { 'Y', Resources.Load<GameObject>("Letters/LetterY") },
        { 'Z', Resources.Load<GameObject>("Letters/LetterZ") }
    };
    */
    public static HashSet<char> LettersGained = new HashSet<char>();
}
