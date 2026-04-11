using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider slider;

    public EnemyState unit;



    // Update is called once per frame
    void Update()
    {
        slider.value = (float)unit.currentHP/unit.maxHP;
    }
}
