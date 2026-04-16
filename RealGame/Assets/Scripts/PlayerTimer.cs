using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider slider;

    public BattleScript battleScript;
    public float maxTime = 10f;
    // Update is called once per frame
    void Update()
    {
        slider.value = (float)battleScript.playerTimer/maxTime;
    }
}
