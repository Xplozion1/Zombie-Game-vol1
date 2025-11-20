using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float speed = 20f;         // Скорость пули
    public float lifetime = 2f;       // Время жизни пули
    public float distance = 0.5f;     // Дистанция луча для проверки столкновений
    public int damage = 1;            // Урон пули
    public LayerMask whatIsSolid;     // Слой, с которым пуля может сталкиваться

    void Update()
    {
        // Двигаем пулю вперед в зависимости от её поворота
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Проверяем столкновения с помощью луча
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);

        if (hitInfo.collider != null)
        {
            Debug.Log(hitInfo.collider.name); // Отладочное сообщение для проверки столкновения
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }

        // Уничтожаем пулю по истечении времени жизни
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
