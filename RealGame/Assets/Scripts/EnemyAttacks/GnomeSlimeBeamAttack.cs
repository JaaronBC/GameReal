using System.Threading;
using UnityEngine;

public class GnomeSlimeBeamAttack : MonoBehaviour
{
    //stats
    float timer = 5.0f;
    int damage = 2;
    int range = 0;
    float speed = 3f;

    //components
    private Rigidbody2D rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down
        transform.Translate(Vector3.down * (speed * Time.deltaTime), Space.World);

        //timer and death
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
