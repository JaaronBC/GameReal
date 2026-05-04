using UnityEngine;

public class SkeletonAttackStartScript : MonoBehaviour
{
    public GameObject attackObject;
    public SpriteRenderer spriteRenderer;
    public BattleScript battleScript;
    float timer = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleScript = FindObjectOfType<BattleScript>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Vector2 newPos = transform.position;
            newPos.y -= 0.5f;
            Instantiate(attackObject, newPos, Quaternion.identity);
            Destroy(gameObject);
        }

        //die if not in enemy turn
        if (battleScript != null) if (battleScript.state != BattleState.EnemyTurn)
        {
            Destroy(this.gameObject);
        }

        Color alpha = spriteRenderer.color;
        alpha.a += Time.deltaTime;
        spriteRenderer.color = alpha;
    }
}
