using UnityEngine;
public class SettingsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void QuitButtonPressed()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }


}
