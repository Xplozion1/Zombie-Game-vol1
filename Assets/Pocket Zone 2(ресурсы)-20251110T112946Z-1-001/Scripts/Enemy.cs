using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth = 100;
    public int damage;
    public Transform Player;
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    public Vector2 playerSpriteOffset = new Vector2(0f, -2.5f);
    public float minDistance = 2f;
    public GameObject hitEffect;
    public GameObject bloodSplashEffect;
    public GameObject goldPrefab;
    public Image healthBarFill; // Image для заполнения полосы здоровья
    public Vector3 healthBarOffset = new Vector3(0, 1f, 0); // Смещение полосы здоровья над врагом
    private Vector3 initialScale;
    private Animator anim;
    private float lastAttackTime;

    void Awake()
    {
        initialScale = transform.localScale;
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Transform>();
        }
    }

    void Update()
    {
        UpdateHealthBar();

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Transform>();
            return;
        }

        Vector2 playerCenter = (Vector2)Player.position + playerSpriteOffset;
        Vector2 directionToPlayer = (playerCenter - (Vector2)transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerCenter);
        Vector2 targetPosition = playerCenter - directionToPlayer * minDistance;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position.x < playerCenter.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null && attackPoint != null)
        {
            healthBarFill.fillAmount = (float)health / maxHealth;
            healthBarFill.transform.position = Camera.main.WorldToScreenPoint(attackPoint.position + healthBarOffset);
        }
    }


    void Attack()
    {
        anim.SetBool("isAttacking", true);
        lastAttackTime = Time.time;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D playerCollider in hitPlayers)
        {
            Player player = playerCollider.GetComponent<Player>();
            if (player != null)
            {
                player.ChangeHealth(-damage);
                Transform shotPointTransform = player.transform.Find("shotPoint");
                if (shotPointTransform != null)
                {
                    if (bloodSplashEffect != null)
                    {
                        Vector3 effectPosition = shotPointTransform.position + new Vector3(0, -2f, 0);
                        GameObject effectInstance = Instantiate(bloodSplashEffect, effectPosition, Quaternion.identity);
                        Destroy(effectInstance, 0.5f);
                    }
                }
                else
                {
                    if (bloodSplashEffect != null)
                    {
                        Vector3 effectPosition = player.transform.position + new Vector3(0, -2f, 0);
                        GameObject effectInstance = Instantiate(bloodSplashEffect, effectPosition, Quaternion.identity);
                        Destroy(effectInstance, 0.5f);
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (hitEffect != null && attackPoint != null)
        {
            GameObject effectInstance = Instantiate(hitEffect, attackPoint.position, Quaternion.identity);
            Destroy(effectInstance, 0.5f);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (healthBarFill != null && healthBarFill.gameObject != null)
        {
            Destroy(healthBarFill.gameObject);
        }
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
