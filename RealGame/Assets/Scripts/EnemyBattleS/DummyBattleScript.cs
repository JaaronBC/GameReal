using UnityEngine;

public class DummyBattleScript : EnemyBattle
{
    //unique attack stats
    int[] attackCount = { 2, 3 };
    int range = 0;
    public GameObject attackObject2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        // Ensure base initialisation runs so `animator`, `spriteRenderer`, `flash`, and `battleScript` are assigned.
        base.Start();
        cooldownRange = new float[] { 3.0f, 4.0f };
        //IMPORTANT: FOLLOWING LINES MUST BE IN ALL ENEMY BATTLE SCRIPTS
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        battleScript = FindFirstObjectByType<BattleScript>();
    }

    //make attack override
    public override void makeAttack()
    {
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        //generate attack pattern
        int _attackType = Random.Range(0, 2);
        int _count = Random.Range(attackCount[0], attackCount[1] + 1);
        float _offset;
        Vector3 _spawnPosition;

        //generate attack 
        if (attackObject != null && _attackType == 0)
        {
            //generate offset and spawn position
            _offset = Random.Range(-range, range + 1);
            _spawnPosition = transform.position;
            _spawnPosition.x += _offset;
            //spawn
            Instantiate(attackObject, _spawnPosition, Quaternion.identity);

        }
        else if (attackObject2 != null)
        {
            for (int i = 0; i < _count; i++)
            {
                //generate offset and spawn position
                _offset = Random.Range(-range, range + 1);
                _spawnPosition = transform.position;
                _spawnPosition.x += _offset;
                //spawn
                Instantiate(attackObject2, _spawnPosition, Quaternion.identity);
            }
        }
    }

}
