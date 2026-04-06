using UnityEditor.Rendering;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    //object components
    SpriteRenderer spriteRenderer;
    Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D playerDetectionRadius;

    //movement variables
    public float z = 0.5f;
    private int timer = 0;
    float currentSpeed = 0.0f;
    float speed = 2f;
    float chaseSpeed = 3f;
    Vector3 direction = new Vector2(0.0f, 0.0f);

    //state machine
    private string state = "normal";

    //normal
    private int normalStillProbability = 50;
    private int[] normalTimeRange = { 180, 360 };

    //player chase
    Transform target;
    public float chaseRadius = 3.5f;
    private int[] chaseTimeRange = { 60, 120 };

    //battle transition
    private string battleSceneName = "BattleScene";


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
        direction = (target.position - transform.position).normalized;
        DirectionToSprite();
    }

    public bool DetectPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position,
            chaseRadius, LayerMask.GetMask("Player"));
        RaycastHit2D tileRay = Physics2D.Linecast(transform.position, target.position,
            LayerMask.GetMask("Tiles"));

        if (collider != null && tileRay == false) return true;
        return false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //collision is player
        if (collision.gameObject.CompareTag("Player"))
        {
            print("player battle detected");
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (playerMovement.z < z)
                    BattleScreenTransition(battleSceneName);
            }
        }
    }

    public void BattleScreenTransition(string sceneName)
    {
        SceneController sc = FindObjectOfType<SceneController>();
        if (sc != null)
        {
            sc.LoadScene(sceneName);
        }
    }

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
        animator.SetFloat("dir_x", (direction.x));
        animator.SetFloat("dir_y", (direction.y));
        if (currentSpeed > 0) animator.SetBool("move", true);
        else animator.SetBool("move", false);
    }

}
