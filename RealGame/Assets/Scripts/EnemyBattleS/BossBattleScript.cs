using System;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossBattleScript : EnemyBattle
{
    GameObject player;
    float bossAddTime = 3.0f;
    int damageIncrease = 2;

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
    string lastAttack = "slimes";
    protected string[] stateList = { "tornado", "explosion", "slimes" };
    float[] stateListTimers = { 2.5f, 0.75f, 0.5f };

    //tornado
    int tornadoCount = 2;
    int tornadoBlastCount = 2;

    //slime
    int slimeCount = 0;
    int slimeMax = 3;
    int slimeMinimum = 1;
    GameObject[] slimes = { };
    Vector3[] slimeVectors = new Vector3[3];

    //explosion
    int explosionCount = 12;
    float explosionRange = 2.75f;
    int bulletCount = 15;
    float bulletAngleRange = 60;

    //attack objects
    public GameObject tornadoAttack;
    public GameObject beamAttack;
    public GameObject explosionAttack;
    public GameObject ballAttack;
    public GameObject gnomeSlime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        flash = GetComponent<FlashScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        battleScript = FindFirstObjectByType<BattleScript>();
        xBoundryDiff = xBoundry[1] - xBoundry[0];
        player = GameObject.Find("PlayerObject");
        transform.position = new Vector3(5f, 6f, 0f);
        battleScript.addedTime = bossAddTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!battleScript) battleScript = FindFirstObjectByType<BattleScript>();
        //check if enemy turn and activate/deactivate
        if (battleScript.state == BattleState.EnemyTurn && !active)
        {
            active = true;
            //slime count and info
            slimes = GameObject.FindGameObjectsWithTag("GnomeBattle");
            slimeCount = slimes.Length;
            for (int i = 0; i < slimeCount; i ++)
            {
                slimeVectors[i] = slimes[i].transform.position;
            }
            //state selection
            state = lastAttack;
            if (slimeCount > slimeMinimum)
            {
                while (state == lastAttack) {
                    state = stateList[UnityEngine.Random.Range(0, 2)];
                    print(state);
                }
                lastAttack = state;
            } else
            {
                while (state == lastAttack)
                {
                    state = stateList[UnityEngine.Random.Range(0, 3)];
                    print(state);
                }
                lastAttack = state;
            }
            attackNum = 0;
            print("");

            //state variables
            switch (state)
            {
                case ("tornado"):
                    cooldown = stateListTimers[0];
                    
                    break;
                case ("slimes"):
                    cooldown = stateListTimers[2];

                    break;
                case ("explosion"):
                    cooldown = stateListTimers[1];

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
        switch (state)
        {
            case ("tornado"):
                tornadoStateAttack(attackNum);
                break;
            case ("slimes"):
                cooldown = stateListTimers[1];
                if (attackNum == 0) slimeSummonAttack();
                else slimeStateAttack();

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
        if (_attackNum %2 == 0)
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
        if (_attackNum != 0)
        {
            int _beamCoordinateLimit = xBoundry[1] - tornadoBlastCount + 1;
            int _beamCoordinateX = UnityEngine.Random.Range(xBoundry[0], _beamCoordinateLimit);
            for (int x = 0; x < tornadoBlastCount; x ++)
            {
                Instantiate(beamAttack, new Vector2((float) _beamCoordinateX + x,
                    transform.position.y), Quaternion.identity);
                GameObject beam = Instantiate(beamAttack, transform.position, Quaternion.identity);
            } 
        }
    }

    //slime attack
    private void slimeSummonAttack()
    {
        for (int i = 0; i < slimeMax - slimeCount; i ++)
        {
            while (true) {
                Vector3 _spawnPosition;
                int _offset_x;
                //generate offset and spawn position
                _offset_x = UnityEngine.Random.Range(xBoundry[0], xBoundry[1]+1);
                _spawnPosition = transform.position;
                _spawnPosition.x = _offset_x;
                //check if spawn position is okay
                bool _invalid = false;
                for (int j = 0; j < slimeCount; j ++)
                {
                    if (_spawnPosition == slimeVectors[j])
                    {
                        _invalid = true;
                        break;
                    }
                }
                if (_spawnPosition == transform.position) _invalid = true;
                if (_invalid) break;
                //check if spawn position is valid
                GameObject _gnomeSlime = Instantiate(gnomeSlime, _spawnPosition, Quaternion.identity);
                battleScript.activeEnemies.Add(gnomeSlime);
                break;
            }
        }
    }

    //ball attack
    private void slimeStateAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            float _xOffset = UnityEngine.Random.Range(-1.5f, 1.5f);
            Vector2 _spawnPosition = transform.position;
            _spawnPosition.x += _xOffset;
            Instantiate(ballAttack, _spawnPosition, Quaternion.identity);
        }
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
                _offset_x = UnityEngine.Random.Range(-explosionRange, explosionRange);
                _offset_y = UnityEngine.Random.Range(-explosionRange/1.5f, explosionRange/1.5f);
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
