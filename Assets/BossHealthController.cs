using UnityEngine;
using DG.Tweening;

public class BossHealthController : MonoBehaviour
{
    public int Health { get; private set; } = 7;
    Vector3 endRotation = new Vector3(90, 0, 0);

    void LoseHealth()
    {
        transform.DORotate(endRotation, Time.time, RotateMode.Fast);
        Health -= 1;
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(transform.parent);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            LoseHealth();
        }
    }
}
