using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBGM : MonoBehaviour
{
    private static TutorialBGM instance;

    // scenes where this music is allowed
    private string[] allowedScenes = {
        "ClassBathroom",
        "ClassRoom",
        "SchoolHall",
        "SchoolHallToRougeLike"
    };

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (string s in allowedScenes)
        {
            if (scene.name == s)
                return; // still in tutorial → keep playing
        }

        Destroy(gameObject); // left tutorial → stop music
    }
}