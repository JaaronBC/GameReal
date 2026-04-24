using UnityEngine;

public class Letter : MonoBehaviour
{
    public char letterValue; //A, B, C, etc...
    void Start()
    {
        //If letterValue is in BattleDataHHolder.LettersGained, and Scene is named RogueLikeScene, destroy the letter
        if (BattleDataHolder.LettersGained.Contains(letterValue) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "RogueLikeScene")
        {
            Destroy(gameObject);
        }
    }
}

