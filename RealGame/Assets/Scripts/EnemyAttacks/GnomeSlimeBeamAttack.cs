using System.Threading;
using UnityEngine;



public class GnomeSlimeBeamAttack : MonoBehaviour
{
    //stats
    public float timer = 5.0f;
    protected float z = 0.5f;
    int damage = 2;
    public float speed = 3f;
    bool playerHitDestroy = true;
    public string state = "normal";

    //components
    private Rigidbody2D rb;
    public BattleScript battleScript;

    //"angle" state variant
    public float angle = 0.0f;
    float angleSpeed = 2.0f;

    protected virtual void Start()
    {
        battleScript = FindObjectOfType<BattleScript>();
        angle = Random.Range(-160f, -30f);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //move down
        if (state == "normal") {
            transform.Translate(Vector3.down * (speed * Time.deltaTime), Space.World);
        } else if (state == "angle")
        {
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
            transform.Translate(direction * (angleSpeed * Time.deltaTime), Space.World);
        }

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
