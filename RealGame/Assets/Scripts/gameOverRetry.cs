using System.Threading;
using UnityEngine;

public class gameOverRetry : MonoBehaviour
{
    private string sceneName = "BattleScene";
    private Animator animator;
    private float timer = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Restart(sceneName);
        }
    }

    public void Restart(string sceneName)
    {
        if (animator)
        {
            animator.SetBool("restart", true);
        }
        Invoke(nameof(RestartTransition), timer);
    }

    private void RestartTransition() {
        SceneController sc = FindObjectOfType<SceneController>();
        if (sc != null)
        {
            sc.LoadScene(sceneName);
        }
    }

}
