using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShapeWordScript : MonoBehaviour
{
    public string word;
    
    public string description;
    public TextMeshProUGUI wordText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wordText.text = word;
    }
}
