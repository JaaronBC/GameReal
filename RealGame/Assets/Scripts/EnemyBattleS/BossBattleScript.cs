using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class BossBattleScript : EnemyBattle
{
    GameObject player;

    //boundries
    int[] xBoundry = { 2, 7 };
    int xBoundryDiff;

    //unique attack stats
    int[] attackCount = { 1, 1 };
    int range = 1;
    float cooldown = 0.0f;

    //states
    string state = "";
    int attackNum = 0;
    int rangeNum = 3;
    protected string[] stateList = { "tornado", "slimes", "explosion"};
    float[] stateListTimers = { 2.0f, 2.0f, 1.25f };

    //tornado
    int tornadoCount = 2;
    int tornadoBlastCount = 2;

    //explosion
    int explosionCount = 9;
    float explosionRange = 2.25f;
    int bulletCount = 12;
    float bulletAngleRange = 60;


    //attack objects
    public GameObject tornadoAttack;
    public GameObject beamAttack;
    public GameObject explosionAttack;
    public GameObject ballAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        flash = GetComponent<FlashScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        battleScript = FindFirstObjectByType<BattleScript>();
        xBoundryDiff = xBoundry[1] - xBoundry[0];
        player = GameObject.Find("PlayerObject");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!battleScript) battleScript = FindFirstObjectByType<BattleScript>();
        //check if enemy turn and activate/deactivate
        if (battleScript.state == BattleState.EnemyTurn && !active)
        {
            active = true;
            //pick state
            state = stateList[2];
            attackNum = 0;
            //state variables
            switch (state)
            {
                case ("tornado"):
                    cooldown = stateListTimers[0];
                    
                    break;
                case ("slimes"):
                    cooldown = stateListTimers[1];
                    print("");

                    break;
                case ("explosion"):
                    cooldown = stateListTimers[2];
                    print("");

                    break;
                default:
                    break;

            }
        }
        if (battleScript.state != BattleState.EnemyTurn) deactivate();
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

    //
    public override void makeAttack()
    {
        print(attackNum);
        switch (state)
        {
            case ("tornado"):
                tornadoStateAttack(attackNum);
                break;
            case ("slimes"):
                cooldown = stateListTimers[1];
                print("");

                break;
            case ("explosion"):
                explosionStateAttack(attackNum);

                break;
            default:
                break;

        }
        timer = cooldown;
        attackNum++;
    }


    //tornado attack
    private void tornadoStateAttack(int _attackNum)
    {
        
        //tornado
        if (_attackNum == 0)
        {
            for (int x = 0; x < tornadoCount; x ++)
            {
                GameObject tornado = Instantiate(tornadoAttack, transform.position, Quaternion.identity);
                if (tornado)
                {
                    DeerAttack newScript = tornado.GetComponent<DeerAttack>();
                    if (newScript) newScript.angle = (x * 90.0f) + 45.0f;
                }
            }
        }
        // beam walls
        else
        {
            int _beamCoordinateLimit = xBoundry[1] - tornadoBlastCount + 1;
            int _beamCoordinateX = Random.Range(xBoundry[0], _beamCoordinateLimit);
            for (int x = 0; x < tornadoBlastCount; x ++)
            {
                print("make attack beam! " + _beamCoordinateX);
                Instantiate(beamAttack, new Vector2((float) _beamCoordinateX + x,
                    transform.position.y), Quaternion.identity);
                GameObject beam = Instantiate(beamAttack, transform.position, Quaternion.identity);
                if (beam) print("made beam!");
                else print("no beam");
            } 
        }
    }

    //slime attack
    private void slimeStateAttack()
    {

    }

    //explosion attack
    private void explosionStateAttack(int _attackNum)
    {
        if (!player) player = GameObject.Find("PlayerObject");
        if (!player) return;

        //even turns explode
        if (_attackNum % 2 == 0)
        {
            Vector2 _spawnPosition;
            float _offset_x, _offset_y;
            for (int i = 0; i < explosionCount; i++)
            {
                //generate offset and spawn position
                _offset_x = Random.Range(-explosionRange, explosionRange);
                _offset_y = Random.Range(-explosionRange, explosionRange);
                _spawnPosition = player.transform.position;
                _spawnPosition.x += _offset_x;
                _spawnPosition.y += _offset_y;
                //spawn
                Instantiate(explosionAttack, _spawnPosition, Quaternion.identity);
            }
        }
        //uneven turns bullets
        else
        {
            for (int i = 0; i < bulletCount; i++)
            {
                Instantiate(ballAttack, transform.position, Quaternion.identity);
            }
        }
    }

}
