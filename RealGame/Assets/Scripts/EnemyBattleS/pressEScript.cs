using UnityEngine;

public class pressEScript : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey) && text != null)
        {
            Destroy(text);
        }
    }
}
