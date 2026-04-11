using UnityEngine;

public abstract class EnemyBattle : MonoBehaviour
{
    //stats
    public float[] cooldownRange = { 1f, 1.5f };
    public float timer = 0f;
    public bool active = false;

    //flash
    public float flashTimerRange = 0.5f;

    //object components
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public BattleScript battleScript;
    public FlashScript flash;
    public GameObject attackObject;

    protected virtual void Start()
    {
        flash = GetComponent<FlashScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        battleScript = GetComponent<BattleScript>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //check if enemy turn and activate/deactivate
        if (battleScript.state == BattleState.EnemyTurn)
        {
            active = true;
        }
        else
        {
            if (active == true) deactivate();
        }
        //animatior
        if (animator != null) animator.SetBool("attacking", active);
        //attack timer 
        if (active)
        {
            if (timer <= 0f)
            {
                makeAttack();
            }
            else
            {
                timer -= Time.deltaTime;
            }
            //flash
            if (timer <= flashTimerRange)
            {
                flash.Flash();
            }
        }
    }

    public virtual void makeAttack()
    {
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
        if (attackObject != null)
        {
            Instantiate(attackObject, transform.position, Quaternion.identity);
        }
    }

    public virtual void deactivate()
    {
        active = false;
        if (animator != null) animator.SetBool("attacking", active);
        timer = Random.Range(cooldownRange[0], cooldownRange[1]);
    }
}
