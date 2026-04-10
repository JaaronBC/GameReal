using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "BattleScene") return;
        if (BattleDataHolder.hasReturnPosition)
        {
            transform.position = BattleDataHolder.playerPosition;
            BattleDataHolder.hasReturnPosition = false; 
        }
    }
}
