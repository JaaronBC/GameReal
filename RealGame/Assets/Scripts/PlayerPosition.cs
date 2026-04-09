using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (BattleDataHolder.hasReturnPosition)
        {
            transform.position = BattleDataHolder.playerPosition;
            BattleDataHolder.hasReturnPosition = false; 
        }
    }
}
