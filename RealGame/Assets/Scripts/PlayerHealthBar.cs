using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider slider;

    public PlayerState unit;



    // Update is called once per frame
    void Update()
    {
        slider.value = (float)unit.CurrentHP/unit.maxHP;
    }
}
