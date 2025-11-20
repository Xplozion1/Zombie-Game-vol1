using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private Joystick joystick;
    private Rigidbody2D rb;
    private Animator anim;
    public bool facingRight = true;
    public int maxHealth = 100;
    public int health;
    public Image healthBar;
    private AudioSource PlayerStep;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        PlayerStep = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        Vector2 inputVector = new Vector2(0, 0);
        inputVector.x = joystick.Horizontal;
        inputVector.y = joystick.Vertical;
        /*       if (Input.GetKey(KeyCode.W))
               {
                   inputVector.y = 1f;
               }
               if (Input.GetKey(KeyCode.S))
               {
                   inputVector.y = -1f;
               }
               if (Input.GetKey(KeyCode.A))
               {
                   inputVector.x = -1f;
               }
               if (Input.GetKey(KeyCode.D))
               {
                   inputVector.x = 1f;
               }
        */
        if (inputVector.x == 0)
        {
            anim.SetBool("isRunning", false);
            PlayerStep.Play();
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
        inputVector = inputVector.normalized;
        if (!facingRight && inputVector.x > 0)
        {
            Flip();
        }
        if (facingRight && inputVector.x < 0)
        {
            Flip();
        }

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

    }
    public void ChangeHealth (int healthValue)
    {
        health += healthValue;
        health = Mathf.Clamp(health, 0, maxHealth); // ”бедимс€, что здоровье не выходит за пределы
        UpdateHealthBar();
        Debug.Log("Player health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)health / maxHealth;
        }
    }
    void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject); // ”ничтожаем объект игрока
    }
}
