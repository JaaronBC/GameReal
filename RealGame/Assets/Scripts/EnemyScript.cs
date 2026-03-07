using UnityEditor.Rendering;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //object components
    SpriteRenderer spriteRenderer;
    Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D playerDetectionRadius;

    //movement variables
    private int timer = 0;
    float currentSpeed = 0.0f;
    float speed = 0.8f;
    float chaseSpeed = 1f;
    Vector3 direction = new Vector2(0.0f, 0.0f);

    //state machine
    private string state = "normal";

    //normal
    private int normalStillProbability = 40;
    private int[] normalTimeRange = { 180, 360 };

    //player chase
    Transform target;
    public float chaseRadius = 3.5f;
    private int[] chaseTimeRange = { 90, 180 };



    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {   
        switch (state) {
            case "normal":
                if (timer == 0)
                {
                    timer = Random.Range(normalTimeRange[0], normalTimeRange[1]);
                    MoveNormal();
                }
                if (DetectPlayer()) state = "chase";
                break;
            case "chase":
                if (timer == 0 && target)
                {
                    if (!DetectPlayer()) state = "normal";
                    timer = Random.Range(chaseTimeRange[0], chaseTimeRange[1]);
                    MoveToPlayer();
                }
                break;
        }

        //movement
        rb.linearVelocity = new Vector2(direction.x, direction.y) * currentSpeed;
        //set sprite index to f



        //main timer reset
        if (timer > 0) timer--;
    }

    public void MoveNormal()
    {
        currentSpeed = speed;
        if (Random.Range(1, 101) > normalStillProbability)
        {
            direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }
        else
        {
            currentSpeed = 0.0f;
        }
        DirectionToSprite();
    }
    public void MoveToPlayer()
    {
        currentSpeed = chaseSpeed;
        direction = target.position - transform.position;
        DirectionToSprite();
        print(direction);
        DirectionToSprite();
    }

    public bool DetectPlayer()
    {
        print("detecting player");
        Collider2D collider = Physics2D.OverlapCircle(transform.position,
            chaseRadius, LayerMask.GetMask("Player"));
        RaycastHit2D tileRay = Physics2D.Linecast(transform.position, target.position,
            LayerMask.GetMask("Tiles"));
        if (collider != null && tileRay == false) return true;
        return false;
    }
    /**
    void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "Player" && state == "normal")
        {
            state = "chase";
            timer = 0;
        }
        print("enter");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && state == "chase")
        {
            state = "normal";
            timer = Random.Range(normalTimeRange[0], normalTimeRange[1]);
        }
        print("exit");
    }
    **/

    public void DirectionToSprite()
    {
        //flip
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        //sprite animation
        animator.SetFloat("dir_x", Mathf.Sign(direction.x));
        animator.SetFloat("dir_y", Mathf.Sign(direction.y));
    }

}
