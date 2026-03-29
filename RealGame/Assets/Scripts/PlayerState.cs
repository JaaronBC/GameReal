using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerState : MonoBehaviour
{
    public int maxHP;
    public int CurrentHP;

    public FlashScript flash;
    private Rigidbody2D rb;
    private float involnerable = 0.0f;
    private float involnerableTime = 1.0f;
    private float knockbackForce = 8f;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    Color color;
    Color baseColor;

    public char[] usableLetters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flash = GetComponent<FlashScript>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
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

        //knockback from direction
        playerMovement.movementForce = (direction * knockbackForce);
    }

}
