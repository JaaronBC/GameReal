using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    //scene to transition to
    public string battleSceneName = "BattleScene";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BattleScreenTransition(battleSceneName);
        }
    }

    public void BattleScreenTransition(string sceneName)
    {
        SceneController sc = FindObjectOfType<SceneController>();
        if (sc != null)
        {
            sc.LoadScene(sceneName);
        }
    }

}
