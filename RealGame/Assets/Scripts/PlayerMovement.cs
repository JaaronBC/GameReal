using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 4F;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private float involnerable = 0.0f;
    private float involnerableTime = 2.0f;

    private float jump = 0.0f;
    private float z = 0.0f;
    public GameObject playerSpriteObject;
    private playerSprite playerSpriteComponent;

    public Vector2 movementForce = new Vector2(0.0f, 0.0f);
    private float movementForceAcceleration = 4f;

    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        //enable player movement script at start of scene
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSpriteComponent = playerSpriteObject.GetComponent<playerSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;
        rb.linearVelocity = (moveInput * moveSpeed);
        rb.linearVelocity += movementForce;
        movementForce = Vector2.Lerp(movementForce, Vector2.zero, movementForceAcceleration * Time.deltaTime);

        //involerable
        if (involnerable > 0)
        {
            involnerable -= Time.deltaTime;
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerSpriteComponent.Jump(10.0f);
        }
    }

    void OnDisable()
    {
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        movementForce = Vector2.zero;
        animator.SetBool("isWalking", false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!enabled) return;
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

    public void jumpLanding()
    {
        return;
    }

}

