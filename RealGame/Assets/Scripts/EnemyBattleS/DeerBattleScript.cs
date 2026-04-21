using UnityEngine;

public class DeerBattleScript : EnemyBattle
{
    //unique attack stats
    int[] attackCount = { 1, 1 };
    int range = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        // Ensure base initialisation runs so `animator`, `spriteRenderer`, `flash`, and `battleScript` are assigned.
        base.Start();
        cooldownRange = new float[] { 2f, 3.75f };
        //IMPORTANT: FOLLOWING LINES MUST BE IN ALL ENEMY BATTLE SCRIPTS
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        battleScript = FindFirstObjectByType<BattleScript>();
    }

    //make attack override
    public override void makeAttack()
    {
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        //generate attack pattern
        int _count = Random.Range(attackCount[0], attackCount[1] + 1);
        float _offset;
        Vector3 _spawnPosition;
        //generate attack 
        if (attackObject != null)
        {
            for (int i = 0; i < _count; i++)
            {
                //generate offset and spawn position
                _offset = Random.Range(-range, range + 1);
                _spawnPosition = transform.position;
                //spawn
                Instantiate(attackObject, _spawnPosition, Quaternion.identity);
            }
        }
    }
}
