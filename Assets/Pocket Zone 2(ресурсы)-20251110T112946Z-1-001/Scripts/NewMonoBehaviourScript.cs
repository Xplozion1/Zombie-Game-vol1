using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class LightFollow2D : MonoBehaviour
{
    public Transform player; // Ссылка на персонажа
    private Light2D light2D; // Компонент света

    private void Start()
    {
        light2D = GetComponent<Light2D>(); // Получаем компонент света
    }

    private void Update()
    {
        // Обновляем позицию света по позиции персонажа
        transform.position = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );
    }
}
