using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public BattleScript battleScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHP;
    public float currentHP;

    public void Damaged(float damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
        //Call die function if HP is 0 or less
        if (currentHP <= 0) Die();
    }
    public void Die()
    {
        
    if (battleScript != null)
        {
            battleScript.activeEnemies.Remove(gameObject);
        }

    Destroy(gameObject);
    }
}
