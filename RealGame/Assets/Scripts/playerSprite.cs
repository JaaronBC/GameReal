using UnityEngine;

public class playerSprite : MonoBehaviour
{
    public float z = 0.0f;
    public float yspd = 0.0f;
    private float grv = 0.5f;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // apply gravity
        if (yspd > 0.0f || z > 0.0f)
        {
            yspd -= grv;
            z += yspd;
            if (z < 0.0f)
            {
                z = 0.0f;
                yspd = 0.0f;
            }
            print(z);
        }

        // update position
        transform.position = new Vector3(player.transform.position.x,
            player.transform.position.y + z, transform.position.z);
    }

    //jump
    public void Jump(float jumpHeight)
    {
        if (z <= 0.0f)
        {
            yspd = jumpHeight;
        }
    }

}
