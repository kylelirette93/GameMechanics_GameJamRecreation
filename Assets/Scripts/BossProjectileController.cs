using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    GameObject bossCat;
    string bossTag = "Boss";
    public float projectileSpeed = 50f;
    public float acceleration = 10f; // Higher acceleration for better homing
    private Rigidbody2D rb;
    private bool isFollowingBoss = false;

    private void Start()
    {
        bossCat = GameObject.FindWithTag(bossTag);
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isFollowingBoss && bossCat != null && rb != null)
        {
            Vector2 direction = (bossCat.transform.position - transform.position).normalized;

            // Increase speed over time (optional)
            projectileSpeed += acceleration * Time.fixedDeltaTime;

            // Set velocity directly toward the boss
            rb.velocity = direction * projectileSpeed;
        }
        else if (isFollowingBoss && bossCat == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShootAtBoss();
        }
        else if (other.gameObject == bossCat && isFollowingBoss)
        {
            Destroy(gameObject); // Destroy on collision with boss
        }
    }

    void ShootAtBoss()
    {
        isFollowingBoss = true;
    }
}
