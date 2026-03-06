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
    float speed = 1.0f;
    float chaseSpeed = 1.25f;
    Vector3 direction = new Vector2(0.0f, 0.0f);

    //state machine
    private bool still = false;
    private string state = "normal";
    private float[] direction_field = 
        { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

    //normal
    private int normalStillProbability = 40;
    private int[] normalTimeRange = { 180, 360 };

    //player chase
    Transform target;
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
                break;
            case "chase":
                if (timer == 0 && target)
                {
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
        DirectionToSpriteXY();
    }

    public void MoveToPlayer()
    {
        currentSpeed = chaseSpeed;
        direction = target.position - transform.position;
        DirectionToSpriteXY();
        print(direction);
        DirectionToSpriteXY();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && state == "normal")
        {
            state = "chase";
            timer = 0;
        }
        print("state");
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

    public void DirectionToSpriteXY() //change to dir_x and dir_y
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
