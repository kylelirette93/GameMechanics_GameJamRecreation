using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    GameObject bossCat;
    string bossTag = "Boss";
    public float projectileSpeed = 50f;
    public float acceleration = 10f; 
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

            // Increase speed of projectile over time for homing effect using acceleration.
            projectileSpeed += acceleration * Time.fixedDeltaTime;

            rb.velocity = direction * projectileSpeed;
        }
        else if (isFollowingBoss && bossCat == null)
        {
            // If the boss is destroyed, destroy the projectile.
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
            // Destroy projectile when it hits the boss.
            Destroy(gameObject); 
        }
    }

    void ShootAtBoss()
    {
        isFollowingBoss = true;
    }
}
