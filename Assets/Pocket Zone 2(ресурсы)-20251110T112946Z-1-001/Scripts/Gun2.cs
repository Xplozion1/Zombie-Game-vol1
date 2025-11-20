using UnityEngine;
using UnityEngine.UI;

public class Gun2 : MonoBehaviour
{
    public GameObject bullet;          // Префаб пули
    public Transform shotPoint;       // Точка вылета пули
    public Button shootButton;        // Кнопка для стрельбы
    public Player player;             // Ссылка на скрипт Player

    private float timeBtwShots = 0f;  // Таймер между выстрелами
    public float startTimeBtwShots;   // Начальное время между выстрелами
    private AudioSource SoundBullet; 

        void Start()
    {
        SoundBullet = GetComponent<AudioSource>();
        if (shootButton != null)
        {
            shootButton.onClick.AddListener(Shoot);

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if (timeBtwShots > 0)
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (timeBtwShots <= 0)
        {
            // Получаем направление персонажа
            Vector3 bulletDirection = player.facingRight ? Vector3.right : Vector3.left;

            // Создаем пулю с правильным направлением
            GameObject newBullet = Instantiate(bullet, shotPoint.position, Quaternion.identity);

            // Поворачиваем пулю в зависимости от направления персонажа
            if (!player.facingRight)
            {
                newBullet.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            timeBtwShots = startTimeBtwShots;
            if (SoundBullet != null)
            {
                SoundBullet.Play();
            }
        }
    }
}
