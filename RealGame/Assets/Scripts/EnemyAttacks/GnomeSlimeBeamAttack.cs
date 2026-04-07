using System.Threading;
using UnityEngine;



public class GnomeSlimeBeamAttack : MonoBehaviour
{
    //stats
    float timer = 5.0f;
    protected float z = 0.5f;
    int damage = 2;
    float speed = 3f;
    bool playerHitDestroy = true;

    //components
    private Rigidbody2D rb;
    public BattleScript battleScript;

    protected virtual void Start()
    {
        battleScript = FindObjectOfType<BattleScript>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //move down
        transform.Translate(Vector3.down * (speed * Time.deltaTime), Space.World);

        //timer and death
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(this.gameObject);
        }

        //die if not in enemy turn
        if (battleScript != null) if (battleScript.state != BattleState.EnemyTurn)
        {
            Destroy(this.gameObject);
        }
    }

    //player collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //destroy on collision
            bool col = false;
            //damage player
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            col = collision.gameObject.GetComponent<PlayerState>().TakeDamage(damage, direction, z);
            //destroy self
            if (playerHitDestroy && col) Destroy(this.gameObject);
        }
    }
}
