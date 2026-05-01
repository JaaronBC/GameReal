using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 4f;
    private float moveSpeed = 4f;
    private float airSpeed = 3.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private float involnerable = 0.0f;
    private float involnerableTime = 2.0f;

    public Vector2 movementForce = new Vector2(0.0f, 0.0f);
    private float movementForceAcceleration = 4f;

    private SpriteRenderer spriteRenderer;

    public bool canMove = true;

    private PlayerState playerState;

    // jump variables
    public float z = 0.0f;
    private float yspd = 0.0f;
    private float grv = 15f;
    private float jumpHeight = 6f;

    public GameObject shadowObj;
    public GameObject spriteObj;
    private GameObject currentSpriteObj;
    private GameObject shadowSpriteObj;
    private SpriteRenderer sr;

    // jump sound effect
    public AudioSource audioSource;
    public AudioClip jumpSound;

    // footstep sound effect
    public AudioClip footstepSound;
    public float stepInterval = 0.35f;
    private float stepTimer = 0f;

    void Start()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerState = GetComponent<PlayerState>();

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!enabled)
        {
            return;
        }

        if (!canMove)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            movementForce = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        rb.linearVelocity = moveInput * speed;
        rb.linearVelocity += movementForce;
        movementForce = Vector2.Lerp(movementForce, Vector2.zero, movementForceAcceleration * Time.deltaTime);

        // involnerable
        if (involnerable > 0)
        {
            involnerable -= Time.deltaTime;
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && z <= 0.0f)
        {
            jump();
        }

        if (spriteObj != null)
        {
            // in air
            yspd -= grv * Time.deltaTime;
            z += yspd * Time.deltaTime;

            // landed
            if (z <= 0.0f)
            {
                jumpReset();
            }
            else
            {
                currentSpriteObj.transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + z,
                    transform.position.z
                );

                if (playerState != null && playerState.involnerable > 0.0f)
                {
                    sr.color = new Color(1f, 1f, 1f, Random.Range(0.25f, 0.75f));
                }

                spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        //depth
        spriteRenderer.sortingOrder = (int)((transform.position.y - z) * -100);

        // air and jump speed
        if (z > 0.0f)
            speed = airSpeed;
        else
            speed = moveSpeed;

        // footsteps
        HandleFootsteps();
    }

    void jump()
    {
        // jump sound effect
        if (audioSource != null && jumpSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(jumpSound);
        }

        currentSpriteObj = Instantiate(spriteObj, transform.position, Quaternion.identity);
        shadowSpriteObj = Instantiate(shadowObj, transform.position, Quaternion.identity);
        currentSpriteObj.transform.SetParent(this.transform);
        shadowSpriteObj.transform.SetParent(this.transform);

        sr = currentSpriteObj.GetComponent<SpriteRenderer>();
        Animator a = currentSpriteObj.GetComponent<Animator>();

        sr.flipX = spriteRenderer.flipX;
        a.SetFloat("LastInputX", moveInput.x);
        a.SetFloat("LastInputY", moveInput.y);

        yspd = jumpHeight;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    void jumpReset()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        z = 0.0f;
        yspd = 0.0f;

        if (currentSpriteObj != null) Destroy(currentSpriteObj);
        if (shadowSpriteObj != null) Destroy(shadowSpriteObj);
    }

    // handles the footstep sound effects based on player movement and grounded state
    void HandleFootsteps()
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        bool isGrounded = z <= 0.0f;

        if (!isMoving || !isGrounded || !canMove)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            if (audioSource != null && footstepSound != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(footstepSound);
            }

            stepTimer = stepInterval;
        }
    }

    void OnDisable()
    {
        jumpReset();
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        movementForce = Vector2.zero;
        animator.SetBool("isWalking", false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!enabled) return;

        if (!canMove)
        {
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}