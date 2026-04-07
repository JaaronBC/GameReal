using System;
using System.Threading;
using UnityEngine;

public class DummyAttack : GnomeSlimeBeamAttack
{
    public int count = 10;
    private int countMax = 10;

    public float angle;
    private float angleOffset = 20.0f;
    private float offset = 0.5f;
    private GameObject player;

    private void Awake()
    {
        countMax = count;
        angle = -90.0f;
        z = 1.0f;
    }


    // Update is called once per frame
    protected override void Update()
    {
        //die if not in enemy turn
        if (battleScript != null) if (battleScript.state != BattleState.EnemyTurn)
        {
            Destroy(this.gameObject);
        }
    }

    //
    public void OnAnimationComplete()
    {
        player = GameObject.Find("Player");
        print(player);
        if (player)
        {
            float direction = Mathf.Atan2(player.transform.position.y - transform.position.y,
                player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            angle = direction + UnityEngine.Random.Range(-angleOffset, angleOffset);
        }

        if (count > 0)
        {
            Vector3 spawnOffset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 
                Mathf.Sin(angle * Mathf.Deg2Rad), this.transform.position.z) * offset;
            Vector3 spawnPosition = (Vector3)transform.position + spawnOffset;
            GameObject newSelf = Instantiate(this.gameObject, spawnPosition, Quaternion.identity);
            DummyAttack newScript = newSelf.GetComponent<DummyAttack>();
            newScript.count = count - 1;
            newScript.angle = angle;
        }

        print(count);

        Destroy(this.gameObject);
    }

}
