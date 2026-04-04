using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 4F;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private float involnerable = 0.0f;
    private float involnerableTime = 2.0f;

    public Vector2 movementForce = new Vector2(0.0f, 0.0f);
    private float movementForceAcceleration = 4f;

    private SpriteRenderer spriteRenderer;

    //jump variables
    public float z = 0.0f;
    private float yspd = 0.0f;
    private float grv = 13f;
    private float jumpHeight = 6f;

    public GameObject shadowObj;
    public GameObject spriteObj;
    private GameObject currentSpriteObj;
    private GameObject shadowSpriteObj;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled)
        {
            return;
        }
            rb.linearVelocity = (moveInput * moveSpeed);
        rb.linearVelocity += movementForce;
        movementForce = Vector2.Lerp(movementForce, Vector2.zero, movementForceAcceleration * Time.deltaTime);

        //involerable
        if (involnerable > 0)
        {
            involnerable -= Time.deltaTime;
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && z <= 0.0)
        {
            jump();
        }
        if (spriteObj != null)
        {
            //in air
            yspd -= grv * Time.deltaTime;
            z += yspd * Time.deltaTime;
            //landed
            if (z <= 0.0f)
            {
                jumpReset();
            }
            //transform jump object
            else
            {
                //same position with z offset
                currentSpriteObj.transform.position = new Vector3(transform.position.x, 
                    transform.position.y + z, transform.position.z);
            }
        } 

    }

    void jump()
    {
        currentSpriteObj = Instantiate(spriteObj, transform.position, Quaternion.identity);
        shadowSpriteObj = Instantiate(shadowObj, transform.position, Quaternion.identity);
        currentSpriteObj.transform.SetParent(this.transform);
        shadowSpriteObj.transform.SetParent(this.transform);
        SpriteRenderer sr = currentSpriteObj.GetComponent<SpriteRenderer>();
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

