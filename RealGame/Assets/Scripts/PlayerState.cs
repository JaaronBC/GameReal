using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerState : MonoBehaviour
{
    public int maxHP;
    public int CurrentHP;
    private string gameOverScene = "GameOver";
    private float deathTimer = 2.0f;

    public FlashScript flash;
    private Rigidbody2D rb;
    private float involnerable = 0.0f;
    private float involnerableTime = 1.0f;
    private float knockbackForce = 8f;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private Animator animator;
    Color color;
    Color baseColor;

    public char[] usableLetters;
    public Sprite[] letterSprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flash = GetComponent<FlashScript>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        color = spriteRenderer.color;
        baseColor = color;
    }

    // Update is called once per frame
    void Update()
    {

        if (involnerable > 0.0)
        {
            color.a = Random.Range(0.25f, 0.75f);
            spriteRenderer.color = color;
            involnerable -= Time.deltaTime;
            if (involnerable <= 0.0)
            {
                spriteRenderer.color = baseColor;
            }
        }

        //health gitter
        if (CurrentHP == 0)
        {
            transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        }
    }

    //take damage
    public void TakeDamage(int damage, Vector2 direction)
    {
        if (involnerable > 0.0f) return;

        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0);
        print(CurrentHP);

        //flash, involnerability
        flash.Flash();
        involnerable = involnerableTime;

        //death
        if (CurrentHP <= 0)
        {
            Death();
            return;
        }

        //knockback from direction
        playerMovement.movementForce = (direction * knockbackForce);
    }

    //death
    private void Death()
    {
        involnerable = 1.0f;
        animator.SetBool("death", true);
        Invoke(nameof(DeathTransition), deathTimer);
    }
    private void DeathTransition()
    {
        SceneController sc = FindObjectOfType<SceneController>();
        if (sc != null)
        {
            sc.LoadScene(gameOverScene);
        }
    }

}
