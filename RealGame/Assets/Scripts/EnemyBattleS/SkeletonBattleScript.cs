using Mono.Cecil.Cil;
using UnityEngine;

public class SkeletonBattleScript : EnemyBattle
{
    //unique attack stats
    int[] attackCount = { 4, 7 };
    float range = 2.25f;

    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        // Ensure base initialisation runs so `animator`, `spriteRenderer`, `flash`, and `battleScript` are assigned.
        base.Start();
        cooldownRange = new float[] { 1.75f, 3f };
        //IMPORTANT: FOLLOWING LINES MUST BE IN ALL ENEMY BATTLE SCRIPTS
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        battleScript = FindFirstObjectByType<BattleScript>();

        //player object 
        player = GameObject.Find("PlayerObject");
    }

    //make attack override
    public override void makeAttack()
    {
        if (!player)
        {
            player = GameObject.Find("PlayerObject");
            if (player) print("player found");
        }
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        //generate attack pattern
        int _count = Random.Range(attackCount[0], attackCount[1] + 1);
        float _offset_x, _offset_y;
        Vector2 _spawnPosition;
        //generate attack 
        if (attackObject != null)
        {
            for (int i = 0; i < _count; i++)
            {
                //generate offset and spawn position
                _offset_x = Random.Range(-range, range);
                _offset_y = Random.Range(-range, range);
                _spawnPosition = player.transform.position;
                _spawnPosition.x += _offset_x;
                _spawnPosition.y += _offset_y;
                //spawn
                Instantiate(attackObject, _spawnPosition, Quaternion.identity);
            }
        }
    }
}
