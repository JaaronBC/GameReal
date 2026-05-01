using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    public int maxHP;
    public int CurrentHP;
    private string gameOverScene = "GameOver";
    private float deathTimer = 1.0f;
    public float z = 0.0f;

    public FlashScript flash;
    private Rigidbody2D rb;
    public float involnerable = 0.0f;
    private float involnerableTime = 1.0f;
    private float knockbackForce = 8f;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private Animator animator;
    Color color;
    Color baseColor;

    public char[] usableLetters;
    public Sprite[] letterSprites;
    public PlayerMovement movementScript;

    // sfx for taking damage
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathHitSound; // when player dies, plays dramatic hit sound

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

        if (involnerable > 0.0 && z <= 0.0f)
        {
            color.a = Random.Range(0.25f, 0.75f);
            spriteRenderer.color = color;
            if (involnerable <= 0.0)
            {
                spriteRenderer.color = baseColor;
            }
        }
        if (involnerable > 0.0) involnerable -= Time.deltaTime;

        //health gitter
        if (CurrentHP == 0)
        {
            transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        }

        //z
        z = playerMovement.z;
    }

    //take damage
    public bool TakeDamage(int damage, Vector2 direction, float zAttack)
    {
        if (involnerable > 0.0f) return true;
        if (z > zAttack) return false;

        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        // play hit sound with random pitch
        if (audioSource != null)
        {
            if (CurrentHP <= 0 && deathHitSound != null)
            {
                audioSource.pitch = 0.85f;   // deeper = heavier
                audioSource.volume = 1.2f;   // slightly louder
                audioSource.PlayOneShot(deathHitSound);
                StartCoroutine(EchoDeathHit());
            }
            else if (hitSound != null)
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.volume = 1.0f;
                audioSource.PlayOneShot(hitSound);
            }
        }

        //flash, involnerability
        flash.Flash();
        involnerable = involnerableTime;

        //death
        if (CurrentHP <= 0)
        {
            Death();
            return true;
        }

        //knockback from direction
        playerMovement.movementForce = (direction * knockbackForce);
        return true;
    }

    // echo effect for final death hit
    IEnumerator EchoDeathHit()
    {
        yield return new WaitForSeconds(0.08f);

        if (audioSource != null && deathHitSound != null)
        {
            audioSource.pitch = 0.7f;
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(deathHitSound);
            audioSource.volume = 1.0f;
        }
    }

    //death
    private void Death()
    {
        involnerable = 1.0f;
        movementScript.enabled = false;
        animator.SetBool("death", true);
        Invoke(nameof(DeathTransition), deathTimer);
    }

    private void DeathTransition()
    {
        StartCoroutine(DeathTransitionCoroutine());
    }

    IEnumerator DeathTransitionCoroutine()
    {
        FindObjectOfType<BGMFade>()?.FadeOut(1.0f);

        yield return new WaitForSeconds(1.0f);

        SceneController sc = FindObjectOfType<SceneController>();
        if (sc != null)
        {
            sc.LoadScene(gameOverScene);
        }
    }
}