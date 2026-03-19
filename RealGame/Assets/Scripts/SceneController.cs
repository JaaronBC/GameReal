using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject startTransition;
    [SerializeField] private GameObject endTransition;
    private float transitionDuration = 1f;

    void Start()
    {
        startTransition.SetActive(true);
        endTransition.SetActive(false);
        Invoke(nameof(DisableStartingSceneTransition), transitionDuration);
    }

    private void DisableStartingSceneTransition()
    {
        startTransition.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //scene transition function
    public void LoadScene(string sceneName)
    {
        endTransition.SetActive(true);
        StartCoroutine(LoadSceneAfterDelay(sceneName, transitionDuration));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
    }

}