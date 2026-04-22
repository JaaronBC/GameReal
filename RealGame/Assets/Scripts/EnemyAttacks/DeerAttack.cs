using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DeerAttack : MonoBehaviour
{
    //stats
    int damage = 4;

    float z = 0.25f;
    float speed = 2.0f;
    float timer = 4f;

    float angle = -90.0f;
    float angleTime = 0.5f;
    float angleTimeR = 0.4f;
    float angleChange = 0.03f;
    bool playerHitDestroy = true;

    //game objects
    public SpriteRenderer spriteRenderer;
    public BattleScript battleScript;
    public GameObject player;

    private int baseSortingOrder = 0;



    protected virtual void Start()
    {
        battleScript = FindObjectOfType<BattleScript>();
        player = GameObject.Find("PlayerObject");
    }


    // Update is called once per frame
    void Update()
    {
        // rotation towards player
        if (player && angleTime <= 0.0f)
        {
            Vector2 vectorPoint = player.transform.position - transform.position;
            float direction = Mathf.Atan2(vectorPoint.y, vectorPoint.x) * Mathf.Rad2Deg;
            if (direction < 0) direction += 360;
            if (angle < 0) angle += 360;
            angle = Mathf.Lerp(direction, angle, angleChange);

            angleTime = angleTimeR;
        }
        if (angleTime > 0.0f) angleTime -= Time.deltaTime;
        if (!player)
        {
            player = GameObject.Find("PlayerObject");
        }

        // set velocity from current rotation (degrees) and speed
        Vector2 newAngle = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                        Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        transform.Translate(newAngle * (speed * Time.deltaTime), Space.World);

        //depth
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = baseSortingOrder + Mathf.RoundToInt(-transform.position.y * 100f);
        }

        // timer and death
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
