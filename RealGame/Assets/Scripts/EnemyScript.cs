using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    SpriteRenderer spriteRenderer;
    Animator animator;
    private int timer = 0;
    private int dir_x = 1;
    private int dir_y = 1;
    private float speed = 2.0f;
    private float direction = 0.0f;
    private Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer == 0)
        {
            timer = Random.Range(180, 300);
            Move();
            Direction();
        }
        if (timer > 0) timer--;

    }

    public void Move()
    {
        dir_x = Random.Range(-1, 2);
        dir_y = Random.Range(-1, 2);
        direction = Mathf.Atan2(dir_y, dir_x);
        rb.linearVelocity = new Vector2(Mathf.Cos(direction) * speed, Mathf.Sin(direction) * speed);
    }

    public void Direction()
    {
        //flip
        if (dir_x < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetFloat("dir_x", dir_x);
        }
        else if (dir_x > 0)
        {
            spriteRenderer.flipX = false;
        }
        //sprite animation
        animator.SetFloat("dir_x", (float)dir_x);
        animator.SetFloat("dir_y", (float)dir_y);
        print(dir_y);
    }

}
