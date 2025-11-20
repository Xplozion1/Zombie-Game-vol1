using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset;

    public void SetHealth(float HP)
    {
        slider.value = HP;
    }

    public void SetMaxHealth(float maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = maxHP;
    }

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
