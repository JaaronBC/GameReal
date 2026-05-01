using UnityEngine;

public class EndSceneScript : MonoBehaviour
{

    public GameObject dialouge;
    public Animator animator;
    int textIndex = 0;
    bool death = false;

    private DialogueManager dialougeScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialougeScript = dialouge.GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialouge != null)
        {
            textIndex = dialougeScript.currentLineIndex;
            if (textIndex > 1) death = true;
        }
        if (death && textIndex < 1)
        {
            Destroy(gameObject);
        }

        animator.SetInteger("TextIndex", textIndex);
    }
}
